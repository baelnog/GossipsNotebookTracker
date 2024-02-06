using ChecklistTracker.Config;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace ChecklistTracker.Controls;

public sealed partial class ManyValueToggleButton : FlipView
{
    //public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty
    //    .Register(nameof(ItemsSource), typeof(ObservableCollection<object>), typeof(ManyValueToggleButton), new PropertyMetadata(null));

    //public static readonly DependencyProperty SelectedItemProperty = DependencyProperty
    //    .Register(nameof(SelectedItem), typeof(object), typeof(ManyValueToggleButton), new PropertyMetadata(null));

    //public static readonly DependencyProperty HeaderProperty = DependencyProperty
    //    .Register(nameof(Header), typeof(string), typeof(ManyValueToggleButton), new PropertyMetadata(null));

    public static readonly DependencyProperty LabelMemberPathProperty = DependencyProperty
        .Register(nameof(LabelMemberPath), typeof(string), typeof(ManyValueToggleButton), new PropertyMetadata(null));

    public static readonly DependencyProperty ImageMemberPathProperty = DependencyProperty
        .Register(nameof(ImageMemberPath), typeof(ImageSource), typeof(ManyValueToggleButton), new PropertyMetadata(null));

    //ObservableCollection<object> ItemsSource
    //{
    //    get => (ObservableCollection<object>) GetValue(ItemsSourceProperty);
    //    set
    //    {
    //        SetValue(ItemsSourceProperty, value);
    //    }
    //}

    //object? SelectedItem
    //{
    //    get => (object?)GetValue(SelectedItemProperty);
    //    set
    //    {
    //        SetValue(SelectedItemProperty, value);
    //    }
    //}

    //string? Header
    //{
    //    get => (string?)GetValue(HeaderProperty);
    //    set
    //    {
    //        SetValue(HeaderProperty, value);
    //    }
    //}

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
