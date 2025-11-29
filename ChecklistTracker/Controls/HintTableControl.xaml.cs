using ChecklistTracker.Controls.Click;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;

namespace ChecklistTracker.Controls
{
    public sealed partial class HintTableControl : UserControl, IDropProvider<HintControl>
    {
        private double HintHeight { get; set; }

        public int MaxColumns { get; set; }
        public double TableHeight { get; set; }
        public double TableWidth { get; set; }

        public ScrollingScrollMode HintScrollMode { get; set; }

        private int MaxHints;

        private int TotalWidth;
        private int LeftItems;
        private int RightItems;
        private int ItemWidth;
        private int ItemHeight;

        private TextParams TextParams { get; set; }

        public Brush HintBackgroundColor { get; set; }
        private Color HintBackgroundColorRaw;
        public Brush TextColor { get; set; }
        private Color TextColorRaw;

        private HintControl Entry;

        private string LeftIconSet { get; set; }
        private string RightIconSet { get; set; }

        internal HintTableControl(int hintCount, int hintColumns, int totalWidth, int leftItems, int rightItems, int itemWidth, int itemHeight, Thickness padding, TextParams textParams, string rightIconSet, string leftIconSet, string? labelSet, string[]? labelsFilter, bool allowOverflow = false, string placeholderText = "")
        {
            InitializeComponent();

            HintScrollMode = allowOverflow ? ScrollingScrollMode.Auto : ScrollingScrollMode.Disabled;

            MaxColumns = hintColumns;

            Padding = padding;

            HintHeight = itemHeight + (padding.Top + padding.Bottom) * .5;

            TableWidth = MaxColumns * (totalWidth + padding.Left + padding.Right);
            TableHeight = Math.Ceiling((double)hintCount / MaxColumns) * (HintHeight);

            ScrollLayout.Height = TableHeight;

            MaxHints = hintCount;

            TotalWidth = totalWidth;
            LeftItems = leftItems;
            LeftIconSet = leftIconSet;
            RightItems = rightItems;
            RightIconSet = rightIconSet;
            ItemWidth = itemWidth;
            ItemHeight = itemHeight;

            TextParams = textParams;
            HintBackgroundColorRaw = textParams.BackgroundColor;
            HintBackgroundColor = textParams.BackgroundColorBrush;
            TextColorRaw = textParams.FontColor;
            TextColor = textParams.FontColorBrush;
            FontSize = textParams.FontSize;

            Entry = new HintControl(
                    new HintViewModel(
                        CheckListViewModel.GlobalInstance!,
                        labelSet: labelSet,
                        labelsFilter: labelsFilter,
                        isEntry: true),
                    totalWidth: totalWidth,
                    itemLayout: new LayoutParams(itemWidth, itemHeight, new Thickness(0)),
                    padding: Padding,
                    textParams: textParams,
                    placeholderText: placeholderText);

            Entry.Padding = new Thickness(0);

            Entry.GetEntryBox().QuerySubmitted += OnEnterLocation;

            var clickCallbacks = new ClickCallbacks();
            clickCallbacks.DropHintControlProvider = this;
            //clickCallbacks.OnDropHintControlCompleted = OnDropHint;
            this.ConfigureClickHandler(clickCallbacks);

            this.Layout.Children.Add(Entry);
        }

        HintControl OnDragHint(HintControl control, UIElement sender, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                RemoveHintControl(control);
            }
            return control;
        }

        void OnDropHint(UIElement sender, MouseButton button, HintControl dropped)
        {
            CopyHintControl(dropped);
        }

        void OnEnterLocation(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(sender.Text))
            {
                var text = sender.Text;
                AddHintControl(text);
                sender.Text = "";
            }
        }

        void OnClick(UIElement sender, MouseButton button)
        {
            if (button == MouseButton.Middle && sender is HintControl view)
            {
                RemoveHintControl(view);
            }
        }

        void CopyHintControl(HintControl source)
        {
            var viewModel = new HintViewModel(
                CheckListViewModel.GlobalInstance!,
                leftItems: LeftItems,
                rightItems: RightItems,
                leftIconSet: LeftIconSet,
                rightIconSet: RightIconSet);

            viewModel.AdoptFrom(source.ViewModel);

            var hintControl = new HintControl(
                viewModel,
                totalWidth: TotalWidth,
                itemLayout: new LayoutParams(ItemWidth, ItemHeight, new Thickness(0)),
                padding: Padding,
                textParams: TextParams);
            AddHintControl(hintControl);
        }

        void AddHintControl(string text)
        {
            var hintControl = new HintControl(
                new HintViewModel(
                    CheckListViewModel.GlobalInstance!,
                    leftItems: LeftItems,
                    rightItems: RightItems,
                    leftIconSet: LeftIconSet,
                    rightIconSet: RightIconSet,
                    text: text
                ),
                totalWidth: TotalWidth,
                itemLayout: new LayoutParams(ItemWidth, ItemHeight, new Thickness(0)),
                padding: Padding,
                textParams: TextParams);
            AddHintControl(hintControl);
        }

        void AddHintControl(HintControl hintControl)
        {
            hintControl.Padding = new Thickness(0);
            hintControl.Width = TotalWidth;

            var callbacks = new ClickCallbacks();
            callbacks.OnClick = (s, e) => OnClick(hintControl, e);
            callbacks.DragHintControlProvider = new HintControlDragProvider
            {
                Control = hintControl,
                OnRemove = RemoveHintControl
            };

            hintControl.ConfigureClickHandler(callbacks);

            this.Layout.Children.Insert(this.Layout.Children.Count - 1, hintControl);

            if (HintScrollMode == ScrollingScrollMode.Disabled && this.Layout.Children.Count > MaxHints)
            {
                Entry.Visibility = Visibility.Collapsed;
            }
        }

        void RemoveHintControl(HintControl hintControl)
        {
            this.Layout.Children.Remove(hintControl);

            if (this.Layout.Children.Count <= MaxHints)
            {
                Entry.Visibility = Visibility.Visible;
            }
        }

        public void OnDataDroppedTo(HintControl data)
        {
            CopyHintControl(data);
        }

        private class HintControlDragProvider : IDragProvider<HintControl>
        {
            internal HintControl Control;
            internal Action<HintControl> OnRemove;

            public HintControl GetDragData(MouseButton dragType)
            {
                return Control;
            }

            public void OnDataDraggedFrom(MouseButton dragType)
            {
                if (dragType == MouseButton.Left)
                {
                    OnRemove(Control);
                }
            }
        }
    }
}
