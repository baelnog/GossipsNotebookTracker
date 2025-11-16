using ChecklistTracker.Config;
using ChecklistTracker.Controls;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.Layout.GossipNotebook;
using ChecklistTracker.Layout.HashFrog.Elements;
using ChecklistTracker.ViewModel;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SharpHook;
using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private Config.TrackerConfig Config;
        private TaskPoolGlobalHook GlobalHooks;
        private ScreenCaptureManager ScreenCaptureManager;

        public MainWindow(Config.TrackerConfig config)
        {
            Config = config;
            ScreenCaptureManager = new ScreenCaptureManager(Config.UserConfig);
            this.InitializeComponent();

            GlobalHooks = new TaskPoolGlobalHook();
            GlobalHooks.RunAsync();

            AppWindow.Closing += (o, e) => GlobalHooks.Stop();
        }

        private bool SetupWindowSizeHanders = false;
        private (int width, int height)? ConstrainedSize { get; set; }


        private void SetWindowSize(int width, int height)
        {
            this.Layout.Width = width;
            this.Layout.Height = height;

            ConstrainedSize = (width, height);
            var setSize = () =>
            {
                var titleBarHeight = 32;
                if (this.AppWindow.TitleBar.Height != 0)
                {
                    titleBarHeight = this.AppWindow.TitleBar.Height;
                }
                var menuHeight = Double.IsNaN(this.Menu.ActualHeight) ? 0 : this.Menu.ActualHeight;

                var size = new SizeInt32
                {
                    Height = (int)(menuHeight) + (int)((ConstrainedSize.Value.height + titleBarHeight) * this.Layout.XamlRoot.RasterizationScale),
                    Width = (int)((ConstrainedSize.Value.width + 12) * this.Layout.XamlRoot.RasterizationScale)
                };

                if (this.AppWindow.Size != size)
                {
                    this.AppWindow.Resize(size);
                }
            };
            if (this.Layout.XamlRoot != null)
            {
                setSize();
            }
            if (!SetupWindowSizeHanders)
            {
                SetupWindowSizeHanders = true;
                RoutedEventHandler handler = (object s, RoutedEventArgs e) =>
                {
                    setSize();
                };
                this.Layout.Loaded += handler;
                this.SizeChanged += (object s, Microsoft.UI.Xaml.WindowSizeChangedEventArgs e) => { setSize(); };
            }
        }

        private void ShowOrHideMenuBar()
        {
            Menu.Visibility = Config.UserConfig.ShowMenuBar ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetupMenus()
        {
            Config.UserConfig.OnPropertyChanged(nameof(UserConfig.ShowMenuBar), (s, e) => ShowOrHideMenuBar());

            foreach (var oldItem in LayoutsMenu.Items.Where(item => item.Tag?.ToString() == "LayoutPath").ToList())
            {
                LayoutsMenu.Items.Remove(oldItem);
            }

            foreach (string layoutPath in Config.UserConfig.LayoutHistory)
            {
                if (layoutPath != Config.UserConfig.LayoutPath)
                {
                    var item = new MenuFlyoutItem()
                    {
                        Text = layoutPath,
                        Tag = "LayoutPath"
                    };
                    item.Click += (s, e) => { OpenLayout(layoutPath); };
                    this.LayoutsMenu.Items.Insert(this.LayoutsMenu.Items.Count - 4, item);
                }
            }

            foreach (string settingsPresetPath in Config.UserConfig.SettingsPresets)
            {
                if (!SettingsMenu.Items.Any(item => (item as MenuFlyoutItem)?.Text == settingsPresetPath))
                {
                    var item = new MenuFlyoutItem()
                    {
                        Text = settingsPresetPath,
                        Tag = "SettingsPath"
                    };
                    item.Click += (s, e) => { LoadSettings(settingsPresetPath); };
                    this.SettingsMenu.Items.Add(item);
                }
            }

            // Dynamically create MenuFlyout for ScreenCaptureMenu
            var screenFlyout = ScreenCaptureMenu;
            void PopulateScreenFlyout()
            {
                screenFlyout.Items.Clear(); 
                foreach (var display in ScreenCaptureManager.AvailableDisplays)
                {
                    var item = new RadioMenuFlyoutItem { Text = display.DeviceName, IsChecked = display.Index == ScreenCaptureManager.SelectedScreenIndex };
                    item.Click += (s, e) => ScreenCaptureManager.SelectedScreenIndex = display.Index;
                    screenFlyout.Items.Add(item);
                }
            }
            // Initial population
            PopulateScreenFlyout();
            // Re-populate when screens change
            ScreenCaptureManager.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ScreenCaptureManager.AvailableDisplays))
                {
                    PopulateScreenFlyout();
                }
            };
            Config.UserConfig.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(UserConfig.ScreenShotScreen))
                {
                    PopulateScreenFlyout();
                }
            };

            this.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, ShowOrHideMenuBar);
        }

        public void LayoutDesign(TrackerWindow layout, StyleConfig style)
        {
            Contract.Assert(CheckListViewModel.GlobalInstance != null);

            this.Layout.Children.Clear();

            this.Layout.CanDrag = false;
            this.AppWindow.SetIcon(@"Assets/notebook.ico");

            var windowStyle = new CoalescedStyle(layout.Style, style);

            SetupMenus();

            this.Content.SetValue(FrameworkElement.WidthProperty, windowStyle.Width);
            this.Content.SetValue(FrameworkElement.HeightProperty, windowStyle.Height);

            var width = windowStyle.Width ?? throw new ArgumentNullException("Window width is not set.");
            var height = windowStyle.Height ?? throw new ArgumentNullException("Window height is not set.");

            SetWindowSize(width, height);

            this.Layout.Background = new SolidColorBrush(windowStyle.BackgroundColor?.ToColor() ?? throw new ArgumentNullException("BackgroundColor is not set."));
            this.Title = style.Title;

            foreach (var component in layout.Components)
            {
                if (component is Layout.HashFrog.Elements.Label compLabel)
                {
                    var padding = compLabel.padding.Split(" ").Select(str => double.Parse(str.Replace("px", ""))).ToArray();
                    var paddingObj = padding.Length == 1 ? new Thickness(padding[0]) : new Thickness(padding[1], padding[0], padding[1], padding[0]);
                    var label = new TextBlock()
                    {
                        Text = compLabel.text,
                        Foreground = new SolidColorBrush(compLabel.color.ToColor()),
                        FontSize = compLabel.fontSize,
                        Padding = paddingObj
                    };
                    label.SetValue(Canvas.LeftProperty, compLabel.position[0]);
                    label.SetValue(Canvas.TopProperty, compLabel.position[1]);

                    this.Layout.Children.Add(label);
                }
                else if (component is ElementTable compTable)
                {
                    var columns = compTable.columns;
                    var rows = (int)Math.Ceiling(1.0 * compTable.elements.Count() / columns);
                    var grid = new VariableSizedWrapGrid();
                    grid.MaximumRowsOrColumns = columns;
                    grid.Orientation = Orientation.Horizontal;

                    var padding = compTable.padding.Split(" ").Select(str => double.Parse(str.Replace("px", ""))).ToArray();
                    var paddingObj = padding.Length == 1 ? new Thickness(padding[0]) : new Thickness(padding[1], padding[0], padding[1], padding[0]);

                    foreach (var element in compTable.elements)
                    {
                        var item = ResourceFinder.FindItem(element);
                        var type = item?.type ?? ItemType.Hint;

                        var layoutParams = new LayoutParams(compTable.elementsSize[1], compTable.elementsSize[0], paddingObj);

                        UIElement elementControl;
                        switch (type)
                        {
                            case ItemType.Song:
                                if (item == null)
                                {
                                    throw new ArgumentNullException(nameof(item));
                                }
                                elementControl = new SongControl(new SongViewModel(item, CheckListViewModel.GlobalInstance), layoutParams);
                                break;
                            case ItemType.Reward:
                                if (item == null)
                                {
                                    throw new ArgumentNullException(nameof(item));
                                }
                                elementControl = new RewardControl(new RewardViewModel(item, CheckListViewModel.GlobalInstance, "dungeons", 3), layoutParams);
                                break;
                            case ItemType.Hint:
                                elementControl = new HintStoneControl(new HintStoneViewModel(CheckListViewModel.GlobalInstance, element), layoutParams);
                                break;
                            default:
                                if (item == null)
                                {
                                    throw new ArgumentNullException(nameof(item));
                                }
                                elementControl = new ElementControl(new ItemViewModel(item, CheckListViewModel.GlobalInstance), layoutParams);
                                break;

                        }
                        elementControl.SetValue(FrameworkElement.MarginProperty, paddingObj);
                        grid.Children.Add(elementControl);
                    }
                    grid.Width = compTable.columns * (paddingObj.Left + paddingObj.Right + compTable.elementsSize[1]) + 10;
                    grid.SetValue(Canvas.LeftProperty, compTable.position[1]);
                    grid.SetValue(Canvas.TopProperty, compTable.position[0]);
                    this.Layout.Children.Add(grid);
                }
                else if (component is Layout.HashFrog.Elements.HintTable hintTable)
                {
                    var padding = hintTable.padding.Split(" ").Select(str => double.Parse(str.Replace("px", ""))).ToArray();
                    var paddingObj = padding.Length == 1 ? new Thickness(padding[0]) : new Thickness(padding[1], padding[0], padding[1], padding[0]);

                    double elementWidth = hintTable.width;

                    if (hintTable.hintType == HintType.Location)
                    {
                        var locationHintTable = hintTable as ILocationHintTable;

                        var textParams = new TextParams
                        {
                            FontColor = locationHintTable.color.ToColor(),
                            FontSize = locationHintTable.fontSize ?? 10,
                            BackgroundColor = locationHintTable.backgroundColor.ToColor(),
                        };

                        var tableControl = new HintTableControl(
                                hintCount: locationHintTable.hintNumber,
                                hintColumns: locationHintTable.columns,
                                totalWidth: locationHintTable.width,
                                leftItems: locationHintTable.showBoss ? locationHintTable.bossCount : 0,
                                rightItems: locationHintTable.showItems ? locationHintTable.itemCount : 0,
                                itemWidth: locationHintTable.itemSize[1],
                                itemHeight: locationHintTable.itemSize[0],
                                padding: paddingObj,
                                textParams: textParams,
                                leftIconSet: locationHintTable.bossIconSet,
                                rightIconSet: locationHintTable.itemIconSet,
                                labelSet: locationHintTable.labels,
                                labelsFilter: locationHintTable.labelsSet,
                                allowOverflow: locationHintTable.allowScroll,
                                placeholderText: locationHintTable.placeholderText);

                        tableControl.SetValue(Canvas.LeftProperty, hintTable.position[1]);
                        tableControl.SetValue(Canvas.TopProperty, hintTable.position[0]);
                        this.Layout.Children.Add(tableControl);
                    }
                    else if (hintTable.hintType == HintType.Entrance)
                    {
                        var entranceHintTable = hintTable as IEntranceTable;

                        var layoutParams = new LayoutParams(
                            width: entranceHintTable.width,
                            height: 0,
                            padding: paddingObj
                        );

                        var itemLayoutParams = new LayoutParams(
                            width: entranceHintTable.itemSize[1],
                            height: entranceHintTable.itemSize[0],
                            padding: paddingObj
                        );

                        var textParams = new TextParams
                        {
                            FontColor = entranceHintTable.color.ToColor(),
                            FontSize = entranceHintTable.fontSize ?? 10,
                            BackgroundColor = entranceHintTable.backgroundColor.ToColor(),
                        };

                        var viewMode = new EntranceTableViewModel(
                            CheckListViewModel.GlobalInstance,
                            entranceHintTable.icons,
                            entranceHintTable.labels,
                            entranceHintTable.labelsSet,
                            layoutParams,
                            itemLayoutParams,
                            textParams);

                        var tableControl = new EntranceTableControl
                        {
                            ViewModel = viewMode,
                            Layout = layoutParams
                        };

                        tableControl.SetValue(Canvas.LeftProperty, hintTable.position[1]);
                        tableControl.SetValue(Canvas.TopProperty, hintTable.position[0]);
                        this.Layout.Children.Add(tableControl);
                    }
                    else
                    {
                        var tableControl = new VariableSizedWrapGrid();
                        tableControl.MaximumRowsOrColumns = hintTable.columns;
                        tableControl.Orientation = Orientation.Horizontal;

                        var sometimesHintTable = hintTable as ISometimesHintTable;
                        var itemCount = 0;
                        if (hintTable.showIcon)
                        {
                            itemCount = hintTable.itemCount;
                        }

                        for (int i = 0; i < sometimesHintTable.hintNumber; i++)
                        {
                            var textParams = new TextParams
                            {
                                FontColor = sometimesHintTable.color.ToColor(),
                                FontSize = sometimesHintTable.fontSize ?? 12,
                                BackgroundColor = sometimesHintTable.backgroundColor.ToColor(),
                            };

                            var model = new HintViewModel(
                                CheckListViewModel.GlobalInstance,
                                leftItems: 0,
                                rightItems: itemCount,
                                labelSet: sometimesHintTable.labels,
                                labelsFilter: sometimesHintTable.labelsSet,
                                isEntry: true
                                );
                            var hintControl = new HintControl(
                                model,
                                totalWidth: hintTable.width,
                                itemLayout: new LayoutParams(sometimesHintTable.itemSize[1], sometimesHintTable.itemSize[0], new Thickness(0)),
                                padding: paddingObj,
                                textParams: textParams,
                                placeholderText: sometimesHintTable.placeholderText);
                            hintControl.Width = elementWidth;

                            tableControl.Children.Add(hintControl);
                        }

                        tableControl.Width = (elementWidth + paddingObj.Left + paddingObj.Right) * hintTable.columns + 1;
                        tableControl.SetValue(Canvas.LeftProperty, hintTable.position[1]);
                        tableControl.SetValue(Canvas.TopProperty, hintTable.position[0]);
                        this.Layout.Children.Add(tableControl);
                    }
                }
                else if (component is ScreenshotElement screenshotElem)
                {
                    var vm = new ScreenCaptureViewModel(
                        ScreenCaptureManager,
                        new Rectangle(screenshotElem.clipRegion[0][0], screenshotElem.clipRegion[0][1], screenshotElem.clipRegion[1][0], screenshotElem.clipRegion[1][1]),
                        new LayoutParams(screenshotElem.screenshotSize[1], screenshotElem.screenshotSize[0]),
                        GlobalHooks,
                        DispatcherQueue);
                    var control = new ScreenCaptureControl(
                        vm,
                        new LayoutParams(screenshotElem.size[1], screenshotElem.size[0], new Thickness(0)));

                    control.SetValue(Canvas.LeftProperty, screenshotElem.position[1]);
                    control.SetValue(Canvas.TopProperty, screenshotElem.position[0]);
                    this.Layout.Children.Add(control);
                }
                else if (component is Layout.HashFrog.Elements.Element element)
                {
                    var itemName = ResourceFinder.FindItemById(element.elementId)!;
                    var item = ResourceFinder.FindItem(itemName)!;
                    var control = new ElementControl(new ItemViewModel(item, CheckListViewModel.GlobalInstance), new LayoutParams(element.size[1], element.size[0], new Thickness(0)));

                    control.SetValue(Canvas.LeftProperty, element.position[1]);
                    control.SetValue(Canvas.TopProperty, element.position[0]);
                    this.Layout.Children.Add(control);
                }
            }
        }

        private void MenuOpenLayout(object sender, RoutedEventArgs e)
        {
            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.FileTypeFilter.Add(".json");

            // Open the picker for the user to pick a file
            var pickTask = openPicker.PickSingleFileAsync();
            pickTask.AsTask().ContinueWith(task =>
            {
                var file = task.Result;
                if (file != null)
                {
                    Config.UserConfig.SetLayout(file.Path);
                }
            });
        }

        private void OpenLayout(string layout)
        {
            Config.UserConfig.SetLayout(layout);
        }

        private void MenuReloadLayout(object sender, RoutedEventArgs e)
        {
            ContentDialog reloadDialog = new ContentDialog
            {
                Title = "Reload tracker?",
                Content = "Any content will be lost. Reload current layout?",
                PrimaryButtonText = "Reload",
                CloseButtonText = "Cancel",
                XamlRoot = this.Layout.XamlRoot
            };

            var reloadDialogTask = reloadDialog.ShowAsync();
            reloadDialogTask.AsTask().ContinueWith(task =>
            {
                var result = task.Result;
                if (result == ContentDialogResult.Primary)
                {
                    CheckListViewModel.GlobalInstance.Inventory.Reset();
                    Config.UserConfig.TriggerLayoutReload();
                }
            });
        }

        private void MenuLoadSettings(object sender, RoutedEventArgs e)
        {
            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.SettingsIdentifier = "ChecklistTracker";

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.FileTypeFilter.Add(".json");

            // Open the picker for the user to pick a file
            var pickTask = openPicker.PickSingleFileAsync();
            pickTask.AsTask().ContinueWith(task =>
            {
                var file = task.Result;
                if (file != null)
                {
                    Config.UserConfig.SetSettings(file.Path);
                }
            });
        }

        private void LoadSettings(string settingsFile)
        {
            Logging.WriteLine($"Loading settings preset {settingsFile}");
            Config.SetRandomizerSettings(settingsFile);
        }

        private void ToggleMenuBar(object sender, RoutedEventArgs e)
        {
            this.Config.UserConfig.ShowMenuBar = !this.Config.UserConfig.ShowMenuBar;
        }
    }
}
