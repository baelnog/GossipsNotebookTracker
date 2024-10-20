using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker.Controls
{
    public partial class EntranceControl : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(EntranceViewModel), typeof(EntranceControl), new PropertyMetadata(null));

        public EntranceViewModel ViewModel { get => (EntranceViewModel)GetValue(ViewModelProperty); set => SetValue(ViewModelProperty, value); }

        public EntranceControl()
        {
            this.InitializeComponent();
        }

        void OnTextChanged(UIElement sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            if (sender is AutoSuggestBox entry)
            {
                if (entry.Text.Length == 0)
                {
                    entry.IsSuggestionListOpen = false;
                }
                if (e.Reason != AutoSuggestionBoxTextChangeReason.UserInput || ViewModel.BaseLabelSet == null)
                {
                    return;
                }
                if (entry.Text.Length > 0)
                {
                    var items = ViewModel.BaseLabelSet
                        .Select(label => (Label: label, Score: HintControl.MatchScore(entry.Text, label)))
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
            else if (sender.IsSuggestionListOpen && sender.ItemsSource is List<Config.Label> labels && labels.Any())
            {
                this.EntryBox.Text = labels.First().name;
            }
            EntryBox.ItemsSource = null;
            EntryBox.IsFocusEngaged = false;
            ViewModel.Text = this.EntryBox.Text;
            ViewModel.RaisePropertyChanged(null);
        }
    }
}
