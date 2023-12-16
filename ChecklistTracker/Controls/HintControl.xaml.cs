using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Windows.UI;

namespace ChecklistTracker.Controls
{
    public sealed partial class HintControl : UserControl
    {
        public double FullHeight { get; set; }
        public double TextWidth { get; set; }
        public double TextHeight { get; set; }
        public double TextFontSize { get; set; }
        public Color TextColorRaw { get; set; }
        public Brush TextColor { get { return new SolidColorBrush(TextColorRaw); } }
        public Color TextBackgroundColorRaw { get; set; }
        public Brush TextBackgroundColor { get { return new SolidColorBrush(TextBackgroundColorRaw); } }

        private List<Config.Label> BaseLabelSet { get; set; }

        private bool IsEntry {  get; set; }

        public string Text { get { return IsEntry ? EntryBox.Text: LabelBox.Text; } }
        public UIElement TextBox { get { return IsEntry ? EntryBox : LabelBox; } }

        private List<HintStoneControl> LeftStones { get; set; }
        public List<ImageSource> LeftImages { get { return LeftStones.Select(stone => stone.CurrentImage).ToList(); } }
        private List<HintStoneControl> RightStones { get; set; }
        public List<ImageSource> RightImages { get { return RightStones.Select(stone => stone.CurrentImage).ToList(); } }

        public HintControl(
            int totalWidth, 
            int leftItems, int rightItems, 
            int itemWidth, int itemHeight,
            Thickness padding,
            Color backgroundColor, Color textColor,
            string? leftIconSet = "bosses", string? rightIconSet = "sometimes",
            string? labelSet = null, string text = "", bool isEntry = false,
            List<ImageSource> leftImages = null, List<ImageSource> rightImages = null,
            string placeholderText = "")
        {
            InitializeComponent();
            LeftStones = new List<HintStoneControl>();
            RightStones = new List<HintStoneControl>();
            for (int i = 0; i < leftItems; i++)
            {
                ImageSource startingImage = null;
                if (leftImages != null && leftImages.Count > i)
                {
                    startingImage = leftImages[i];
                }
                var image = new HintStoneControl(itemWidth, itemHeight, leftIconSet, new Thickness(0), startingImage: startingImage);
                image.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
                LeftStones.Add(image);
                this.Layout.Children.Insert(0, image);
            }
            for (int i = 0; i < rightItems; i++)
            {
                ImageSource startingImage = null;
                if (rightImages != null && rightImages.Count > i)
                {
                    startingImage = rightImages[i];
                }
                var image = new HintStoneControl(itemWidth, itemHeight, rightIconSet, new Thickness(0), startingImage: startingImage);
                RightStones.Add(image);
                this.Layout.Children.Add(image);
            }

            Margin = new Thickness(padding.Left, padding.Top * .4, padding.Right, padding.Bottom * .4);
            TextWidth = totalWidth - (rightItems + leftItems) * itemWidth;
            Height = itemHeight;
            TextHeight = itemHeight;
            FullHeight = itemHeight + padding.Bottom + padding.Top;
            TextFontSize = 12;
            TextBackgroundColorRaw = backgroundColor;
            TextColorRaw = textColor;

            IsEntry = isEntry;
            if (isEntry)
            {
                if (labelSet != null)
                {
                    BaseLabelSet = ResourceFinder.GetLabels(labelSet);
                    EntryBox.ItemsSource = BaseLabelSet;
                    EntryBox.TextMemberPath = "name";
                    EntryBox.DisplayMemberPath = "name";
                }
                EntryBox.Visibility = Visibility.Visible;
                LabelBox.Visibility = Visibility.Collapsed;
                this.EntryBox.PlaceholderText = placeholderText;
            }
            else
            {
                EntryBox.Visibility = Visibility.Collapsed;
                LabelBox.Visibility = Visibility.Visible;
                LabelBox.Text = text;
            }

            this.EntryBox.TextChanged += OnTextChanged;
            this.EntryBox.QuerySubmitted += OnQuerySubmitted;
            this.Width = TextWidth + (leftItems + rightItems) * itemWidth + padding.Left + padding.Right;
        }

        internal AutoSuggestBox GetEntryBox()
        {
            return EntryBox;
        }

        private static double[] MatchScore(string text, Config.Label label)
        {
            var aliasScores = label.alias.Select(alias => {
                if (!alias.StartsWith(text, StringComparison.InvariantCultureIgnoreCase)) { return 0; }
                return 1.0 * text.Length / alias.Length;
            });
            var aliasScore = aliasScores.Any() ?
                aliasScores.Max() :
                0;

            var nameScore = -1;
            if (label.name.Contains(text, StringComparison.InvariantCultureIgnoreCase))
            {
                nameScore = label.name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase);
            }

            if (nameScore == -1 && aliasScore == 0)
            {
                return null;
            }

            return new double[] { aliasScore, (double) 0 - nameScore };
        }

        void OnTextChanged(UIElement sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            if (sender is AutoSuggestBox entry)
            {
                if (entry.Text.Length == 0)
                {
                    entry.IsSuggestionListOpen = false;
                }
                if (e.Reason != AutoSuggestionBoxTextChangeReason.UserInput || BaseLabelSet == null)
                {
                    return;
                }
                if (entry.Text.Length > 0)
                {
                    var items = BaseLabelSet
                        .Select(label => (label, MatchScore(entry.Text, label)))
                        .Where(label => label.Item2 != null)
                        .ToList();

                    items.Sort((label1, label2) =>
                    {
                        if (label1.Item2[0] != label2.Item2[0])
                        {
                            return label2.Item2[0].CompareTo(label1.Item2[0]);
                        }
                        return label2.Item2[1].CompareTo(label1.Item2[1]);
                    });

                    EntryBox.ItemsSource = items.Select(label => label.Item1).ToList();
                }
            }
        }

        void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            if (e.ChosenSuggestion != null)
            {
                this.EntryBox.Text = (e.ChosenSuggestion as Config.Label).name;
            }
            else if (sender.IsSuggestionListOpen && sender.ItemsSource != null && sender.ItemsSource is List<Config.Label> labels && labels.Any())
            {
                this.EntryBox.Text = labels.First().name;
            }
            EntryBox.ItemsSource = null;
            EntryBox.IsFocusEngaged = false;
        }
    }
}
