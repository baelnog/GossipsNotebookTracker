using ChecklistTracker.Controls.Click;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json.Serialization;
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ChecklistTracker.Controls.Click;
using Microsoft.UI;
using Microsoft.UI.Xaml.Markup;
using Windows.UI;
using ChecklistTracker.Layout.HashFrog;
using ChecklistTracker.Layout.HashFrog.Elements;
using ChecklistTracker.Controls;
using ChecklistTracker.Config;
using Windows.Graphics;
using CommunityToolkit.WinUI.Helpers;
using System.Runtime.InteropServices;
using WinRT;
using Windows.ApplicationModel;
using System;
using Microsoft.Graphics.Display;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            LayoutDesign();
        }

        private void SetWindowSize(int width, int height)
        {
            var setSize = () =>
            {
                var titleBarHeight = 32;
                if (this.AppWindow.TitleBar.Height != 0)
                {
                    titleBarHeight = this.AppWindow.TitleBar.Height;
                }
                var size = new SizeInt32
                {
                    Height = (int)((height + titleBarHeight) * this.Layout.XamlRoot.RasterizationScale),
                    Width = (int)((width + 12) * this.Layout.XamlRoot.RasterizationScale)
                };

                this.AppWindow.Resize(size);
            };
            if (this.Layout.XamlRoot != null)
            {
                setSize();
            }
            RoutedEventHandler handler = (object s, RoutedEventArgs e) =>
            {
                setSize();
            };
            this.Layout.Loaded += handler;
            this.SizeChanged += (object s, Microsoft.UI.Xaml.WindowSizeChangedEventArgs e) => { setSize(); };
        }

        private void LayoutDesign()
        {
            //this.Layout.ConfigureClickHandler(new ClickCallbacks());
            this.Layout.CanDrag = false;
            //this.Layout.ZIndex--;
            var layoutDoc = ResourceFinder.ReadResourceFile("layouts/season7.json").Result;
            //var layoutDoc = ResourceFinder.ReadResourceFile("layouts/season7.json").Result;
            var layout = JsonSerializer.Deserialize<HashFrogLayout>(
                layoutDoc,
                new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    PropertyNameCaseInsensitive = true,
                    Converters = {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                        new ElementConverter(),
                    }

                });
            this.Content.SetValue(FrameworkElement.WidthProperty, layout.layoutConfig.width);
            this.Content.SetValue(FrameworkElement.HeightProperty, layout.layoutConfig.height);

            SetWindowSize(layout.layoutConfig.width, layout.layoutConfig.height);
            //this.AppWindow.Resize(new SizeInt32 { Width = layout.layoutConfig.width, Height = layout.layoutConfig.height });

            this.Layout.Width = layout.layoutConfig.width;
            this.Layout.Height = layout.layoutConfig.height;
            this.Layout.Background = new SolidColorBrush(layout.layoutConfig.backgroundColor.ToColor());
            this.Title = layout.layoutConfig.name;

            Inventory inventory = new Inventory();

            foreach (var component in layout.components)
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
                    //this.Layout.SetLayoutBounds(label, new Rect(0, 0, Microsoft.Maui.Controls.AbsoluteLayout.AutoSize, Microsoft.Maui.Controls.AbsoluteLayout.AutoSize));
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
                        UIElement elementControl = null;
                        switch (type)
                        {
                            case ItemType.Song:
                                elementControl = new SongControl(item, inventory, compTable.elementsSize[1], compTable.elementsSize[0], paddingObj);
                                break;
                            case ItemType.Reward:
                                elementControl = new RewardControl(item, inventory, "dungeons", 3, compTable.elementsSize[1], compTable.elementsSize[0], paddingObj);
                                break;
                            case ItemType.Hint:
                                elementControl = new HintStoneControl(compTable.elementsSize[1], compTable.elementsSize[0], element, paddingObj);
                                break;
                            default:
                                elementControl = new ElementControl(item, inventory, compTable.elementsSize[1], compTable.elementsSize[0], paddingObj);
                                break;

                        }
                        if (elementControl != null)
                        {
                            elementControl.SetValue(FrameworkElement.MarginProperty, paddingObj);
                            grid.Children.Add(elementControl);
                        }
                    }
                    grid.Width = compTable.columns * (paddingObj.Left + paddingObj.Right + compTable.elementsSize[0]) + 10;
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

                        var tableControl = new HintTableControl(
                                hintCount: locationHintTable.hintNumber,
                                hintColumns: locationHintTable.columns,
                                totalWidth: locationHintTable.width,
                                leftItems: locationHintTable.showBoss ? locationHintTable.bossCount : 0,
                                rightItems: locationHintTable.showItems ? locationHintTable.itemCount : 0,
                                itemWidth: locationHintTable.itemSize[1],
                                itemHeight: locationHintTable.itemSize[0],
                                padding: paddingObj,
                                backgroundColor: locationHintTable.backgroundColor.ToColor(),
                                textColor: locationHintTable.color.ToColor(),
                                leftIconSet: locationHintTable.bossIconSet,
                                rightIconSet: locationHintTable.itemIconSet,
                                labelSet: locationHintTable.labels,
                                allowOverflow: locationHintTable.allowScroll,
                                placeholderText: locationHintTable.placeholderText);

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
                            itemCount++;
                            if (hintTable.dual)
                            {
                                itemCount++;
                            }
                        }
                        //elementWidth += itemCount * (hintTable.itemSize[1] + paddingObj.HorizontalThickness);

                        for (int i = 0; i < sometimesHintTable.hintNumber; i++)
                        {
                            var hintControl = new HintControl(
                                totalWidth: hintTable.width,
                                leftItems: 0,
                                rightItems: itemCount,
                                itemWidth: sometimesHintTable.itemSize[1],
                                itemHeight: sometimesHintTable.itemSize[0],
                                padding: paddingObj,
                                backgroundColor: sometimesHintTable.backgroundColor.ToColor(),
                                textColor: sometimesHintTable.color.ToColor(),
                                isEntry: true,
                                labelSet: sometimesHintTable.labels,
                                placeholderText: sometimesHintTable.placeholderText);
                            hintControl.Width = elementWidth;

                            tableControl.Children.Add(hintControl);
                        }

                        //tableControl.Wrap = FlexWrap.Wrap;
                        //tableControl.Padding = paddingObj;
                        tableControl.Width = (elementWidth + paddingObj.Left + paddingObj.Right) * hintTable.columns;
                        tableControl.SetValue(Canvas.LeftProperty, hintTable.position[1]);
                        tableControl.SetValue(Canvas.TopProperty, hintTable.position[0]);
                        this.Layout.Children.Add(tableControl);
                    }
                }
                else if (component is Layout.HashFrog.Elements.Element element)
                {
                    var itemName = ResourceFinder.FindItemById(element.elementId);
                    var item = ResourceFinder.FindItem(itemName);
                    var control = new ElementControl(ResourceFinder.FindItem(itemName), inventory, element.Size[1], element.Size[0], new Thickness(0));

                    control.SetValue(Canvas.LeftProperty, element.position[1]);
                    control.SetValue(Canvas.TopProperty, element.position[0]);
                    this.Layout.Children.Add(control);
                }
            }

            //this.Content = this.Layout;
        }
    }
}
