using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace ChecklistTracker.Controls;

public sealed partial class ManyValueToggleButton : FlipView
{
    public static readonly DependencyProperty LabelMemberPathProperty = DependencyProperty
        .Register(nameof(LabelMemberPath), typeof(string), typeof(ManyValueToggleButton), new PropertyMetadata(null));

    public static readonly DependencyProperty ImageMemberPathProperty = DependencyProperty
        .Register(nameof(ImageMemberPath), typeof(ImageSource), typeof(ManyValueToggleButton), new PropertyMetadata(null));

    internal string? LabelMemberPath
    {
        get => (string?)GetValue(LabelMemberPathProperty);
        set
        {
            SetValue(LabelMemberPathProperty, value);
        }
    }

    internal ImageSource? ImageMemberPath
    {
        get => (ImageSource?)GetValue(ImageMemberPathProperty);
        set
        {
            SetValue(ImageMemberPathProperty, value);
        }
    }

    public ManyValueToggleButton() : base()
    {
    }
}

internal class ManyValueToggleButtonTemplateType
{
    private ManyValueToggleButton Parent;
    private object Item;

    internal ManyValueToggleButtonTemplateType(ManyValueToggleButton parent, object item)
    {
        Parent = parent;
        Item = item;
    }

    public ImageSource? ImageProperty
    {
        get
        {
            if (Parent.ImageMemberPath == null)
            {
                return null;
            }
            return new BitmapImage
            {
                UriSource = new Uri("file:///images/kokiri-sword_32x32.png")
            };
        }
    }

    public string? TextProperty
    {
        get
        {
            if (Parent.LabelMemberPath == null)
            {
                return null;
            }
            return Item.ToString();
        }
    }
}
