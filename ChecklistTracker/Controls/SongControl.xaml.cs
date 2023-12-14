using ChecklistTracker.Config;
using ChecklistTracker.Controls.Click;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace ChecklistTracker.Controls;

public partial class SongControl : UserControl
{
    public double ImageWidth { get; set; }
    public double ImageHeight { get; set; }

    public double BottomImageWidth { get; set; }
    public double BottomImageHeight { get; set; }
    public Thickness BottomImageMargin { get; set; }

    public ImageSource CurrentImage { get; set; }
    public ImageSource BottomImage { get; set; }

    private Inventory Inventory;
    internal Item Item;

    internal SongControl(Item trackedItem, Inventory inventory, double width, double height, Thickness padding)
    {
        InitializeComponent();
        Inventory = inventory;
        Item = trackedItem;
        CurrentImage = Inventory.GetCurrentItemImage(trackedItem);
        BottomImage = ResourceFinder.FindItem("song", 0);
        ImageWidth = width;
        ImageHeight = height;

        var bottomScale = 0.6;
        BottomImageWidth = bottomScale * width;
        BottomImageHeight = bottomScale * height;

        Padding = new Thickness(padding.Left, padding.Top, padding.Right, padding.Bottom);
        BottomImageMargin = new Thickness(0, 0, 0, -0.6 * BottomImageWidth);

        var callbacks = new ClickCallbacks();
        callbacks.OnClick = OnClick;
        callbacks.OnDragImageCompleted = OnDragImage;
        callbacks.OnDropImageCompleted = OnDropImage;

        Image.ConfigureClickHandler(callbacks);

        var smallImageCallbacks = new ClickCallbacks();
        smallImageCallbacks.OnClick = OnSmallClick;
        smallImageCallbacks.OnDropImageCompleted += OnDropImage;
        Image2.ConfigureClickHandler(smallImageCallbacks);
    }

    void OnClick(UIElement sender, MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                Inventory.CollectItem(Item);
                break;
            case MouseButton.Right:
                Inventory.UncollectItem(Item);
                break;
            default:
                return;
        }
        CurrentImage = Inventory.GetCurrentItemImage(Item);
        this.Bindings.Update();
    }

    void OnSmallClick(UIElement sender, MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                BottomImage = ResourceFinder.FindItem("song", 1);
                break;
            case MouseButton.Right:
                BottomImage = ResourceFinder.FindItem("song", 0);
                break;
            default:
                return;
        }
        this.Bindings.Update();
    }

    ImageSource OnDragImage(UIElement sender, MouseButton button)
    {
        if (button == MouseButton.Left)
        {
            Inventory.CollectItem(Item);
            CurrentImage = Inventory.GetCurrentItemImage(Item);
            this.Bindings.Update();
            return CurrentImage;
        }
        else
        {
            return ResourceFinder.FindItemImage(Item, 1);
        }
    }

    void OnDropImage(UIElement sender, MouseButton button, ImageSource image)
    {
        BottomImage = image;
        this.Bindings.Update();
    }
}