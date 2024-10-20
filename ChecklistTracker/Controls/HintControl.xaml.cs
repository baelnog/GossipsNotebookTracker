using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<Config.Label>? BaseLabelSet { get; set; }

        private bool IsEntry { get; set; }

        //public string Text { get { return IsEntry ? EntryBox.Text: LabelBox.Text; } }
        public UIElement TextBox { get { return IsEntry ? EntryBox : LabelBox; } }

        private List<HintStoneControl> LeftStones { get; set; }
        //public List<ImageSource> LeftImages { get { return LeftStones.Select(stone => stone.ViewModel.CurrentImage).ToList(); } }
        private List<HintStoneControl> RightStones { get; set; }
        //public List<ImageSource> RightImages { get { return RightStones.Select(stone => stone.ViewModel.CurrentImage).ToList(); } }

        internal HintViewModel ViewModel;

        internal HintControl(
            HintViewModel viewModel,
            int totalWidth,
            LayoutParams itemLayout,
            Thickness padding,
            TextParams textParams,
            string placeholderText = "")
        {
            InitializeComponent();
            ViewModel = viewModel;

            LeftStones = new List<HintStoneControl>();
            RightStones = new List<HintStoneControl>();
            foreach (HintStoneViewModel stone in ViewModel.LeftStones)
            {
                var control = new HintStoneControl(stone, itemLayout);
                LeftStones.Add(control);
                this.LeftItemsLayout.Children.Add(control);
            }
            foreach (HintStoneViewModel stone in ViewModel.RightStones)
            {
                var control = new HintStoneControl(stone, itemLayout);
                RightStones.Add(control);
                this.RightItemsLayout.Children.Add(control);
            }

            Margin = new Thickness(padding.Left, padding.Top * .4, padding.Right, padding.Bottom * .4);
            TextWidth = totalWidth - (ViewModel.RightStones.Count + ViewModel.LeftStones.Count) * itemLayout.Width;
            Height = itemLayout.Height;
            TextHeight = itemLayout.Height;
            FullHeight = itemLayout.Height + padding.Bottom + padding.Top;
            TextFontSize = textParams.FontSize;
            TextBackgroundColorRaw = textParams.BackgroundColor;
            TextColorRaw = textParams.FontColor;

            IsEntry = viewModel.IsEntry;
            if (IsEntry)
            {
                if (ViewModel.LabelSet != null)
                {
                    BaseLabelSet = ViewModel.BaseLabelSet;
                    EntryBox.ItemsSource = ViewModel.BaseLabelSet;
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
                LabelBox.Text = viewModel.Text;
            }

            this.EntryBox.TextChanged += OnTextChanged;
            this.EntryBox.QuerySubmitted += OnQuerySubmitted;
            this.Width = totalWidth;// TextWidth + (ViewModel.RightStones.Count + ViewModel.LeftStones.Count) * itemLayout.Width + itemLayout.Padding.Left + itemLayout.Padding.Right;
        }

        internal AutoSuggestBox GetEntryBox()
        {
            return EntryBox;
        }

        internal static double[]? MatchScore(string text, Config.Label label)
        {
            var aliasScores = label.alias.Select(alias =>
            {
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

            return [aliasScore, (double)0 - nameScore];
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
                        .Select(label => (Label: label, Score: MatchScore(entry.Text, label)))
                        .Where(label => label.Score != null)
                        .ToList();

                    items.Sort((label1, label2) =>
                    {
                        if (label1.Score![0] != label2.Score![0])
                        {
                            return label2.Score[0].CompareTo(label1.Score[0]);
                        }
                        return label2.Score[1].CompareTo(label1.Score[1]);
                    });

                    EntryBox.ItemsSource = items.Select(label => label.Label).ToList();
                }
            }
        }

        void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            if (e.ChosenSuggestion is Config.Label chosenLabel)
            {
                this.EntryBox.Text = chosenLabel.name;
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
