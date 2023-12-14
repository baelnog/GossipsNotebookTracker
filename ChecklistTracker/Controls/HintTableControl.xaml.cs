using ChecklistTracker.Controls.Click;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using Windows.UI;

namespace ChecklistTracker.Controls
{
    public sealed partial class HintTableControl : UserControl
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
        public Brush HintBackgroundColor { get; set; }
        private Color HintBackgroundColorRaw;
        public Brush TextColor { get; set; }
        private Color TextColorRaw;

        private HintControl Entry;

        private string LeftIconSet { get; set; }
        private string RightIconSet { get; set; }

        public HintTableControl(int hintCount, int hintColumns, int totalWidth, int leftItems, int rightItems, int itemWidth, int itemHeight, Thickness padding, Color backgroundColor, Color textColor, string rightIconSet, string leftIconSet, string? labelSet, bool allowOverflow = false, string placeholderText = "")
        {
            InitializeComponent();

            HintScrollMode = allowOverflow ? ScrollingScrollMode.Auto : ScrollingScrollMode.Disabled;

            MaxColumns = hintColumns;

            Padding = padding;

            HintHeight = itemHeight + (padding.Top + padding.Bottom) * .5;

            TableWidth = MaxColumns * (totalWidth + padding.Left + padding.Right);
            TableHeight = Math.Ceiling((double) hintCount / MaxColumns) * (HintHeight);

            ScrollLayout.Height = TableHeight;

            MaxHints = hintCount;

            TotalWidth = totalWidth;
            LeftItems = leftItems;
            LeftIconSet = leftIconSet;
            RightItems = rightItems;
            RightIconSet = rightIconSet;
            ItemWidth = itemWidth;
            ItemHeight = itemHeight;
            HintBackgroundColorRaw = backgroundColor;
            HintBackgroundColor = new SolidColorBrush(backgroundColor);
            TextColorRaw = textColor;
            TextColor = new SolidColorBrush(textColor);

            Entry = new HintControl(
                    totalWidth: totalWidth,
                    leftItems: 0,
                    rightItems: 0,
                    itemWidth: itemWidth,
                    itemHeight: itemHeight,
                    padding: padding,
                    backgroundColor: backgroundColor,
                    textColor: textColor,
                    isEntry: true,
                    labelSet: labelSet,
                    placeholderText: placeholderText);

            Entry.Padding = new Thickness(0);

            Entry.GetEntryBox().QuerySubmitted += OnEnterLocation;

            var clickCallbacks = new ClickCallbacks();
            clickCallbacks.OnDropHintControlCompleted = OnDropHint;
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
            var hintControl = new HintControl(
                totalWidth: TotalWidth,
                leftItems: LeftItems,
                rightItems: RightItems,
                itemWidth: ItemWidth,
                itemHeight: ItemHeight,
                padding: Padding,
                backgroundColor: HintBackgroundColorRaw,
                textColor: TextColorRaw,
                text: source.Text,
                leftImages: source.LeftImages,
                rightImages: source.RightImages,
                leftIconSet: LeftIconSet,
                rightIconSet: RightIconSet);
            AddHintControl(hintControl);
        }

        void AddHintControl(string text)
        {
            var hintControl = new HintControl(
                totalWidth: TotalWidth,
                leftItems: LeftItems,
                rightItems: RightItems,
                itemWidth: ItemWidth,
                itemHeight: ItemHeight,
                padding: Padding,
                backgroundColor: HintBackgroundColorRaw,
                textColor: TextColorRaw,
                text: text,
                leftIconSet: LeftIconSet,
                rightIconSet: RightIconSet);
            AddHintControl(hintControl);
        }

        void AddHintControl(HintControl hintControl)
        {
            hintControl.Padding = new Thickness(0);
            hintControl.Width = TotalWidth;

            var callbacks = new ClickCallbacks();
            callbacks.OnClick = (s, e) => OnClick(hintControl, e);
            callbacks.OnDragHintControlCompleted = (s, e) => OnDragHint(hintControl, s, e);

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
    }
}
