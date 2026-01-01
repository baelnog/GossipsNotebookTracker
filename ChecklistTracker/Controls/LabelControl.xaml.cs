using ChecklistTracker.Layout;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace ChecklistTracker.Controls;

public sealed partial class LabelControl : UserControl
{
    internal string Text { get; private set; }
    internal ITextStyle TextStyle { get; private set; }
    internal LayoutParams Layout { get; private set; }

    public LabelControl(string text, ITextStyle textStyle, LayoutParams layoutParams)
    {
        InitializeComponent();

        Text = text;
        TextStyle = textStyle;
        Layout = layoutParams;
    }
}
