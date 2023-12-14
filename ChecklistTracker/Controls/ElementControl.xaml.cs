using ChecklistTracker.Config;
using ChecklistTracker.Controls.Click;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace ChecklistTracker.Controls
{
    public sealed partial class ElementControl : UserControl
    {
        public ImageSource CurrentImage { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int Count { get; set; }
        public bool HasCount { get; set; }
        public Visibility CountVisibility { get { return HasCount ? Visibility.Visible: Visibility.Collapsed; } }

        private Inventory Inventory;
        internal Item Item;

        internal ElementControl(Item item, Inventory inventory, int width, int height, Thickness padding)
        {
            InitializeComponent();
            Inventory = inventory;
            Item = item;
            CurrentImage = inventory.GetCurrentItemImage(item);
            ImageWidth = width; ImageHeight = height;
            Padding = padding;

            HasCount = Item.collection == CollectionType.Count;

            var callbacks = new ClickCallbacks();
            callbacks.OnClick = OnClick;
            callbacks.OnDragImageCompleted = OnDragImage;
            callbacks.OnScroll += OnScroll;

            this.Image.ConfigureClickHandler(callbacks);
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
            Count = Inventory.GetCurrentItemCount(Item);
            this.Bindings.Update();
        }

        ImageSource OnDragImage(UIElement sender, MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                Inventory.CollectItem(Item);
                CurrentImage = Inventory.GetCurrentItemImage(Item);
                Count = Inventory.GetCurrentItemCount(Item);
                return CurrentImage;
            }
            else
            {
                return ResourceFinder.FindItemImage(Item, 1);
            }
        }

        void OnScroll(UIElement sender, int scrollAmount)
        {
            Inventory.CollectAmount(Item, scrollAmount);
            CurrentImage = Inventory.GetCurrentItemImage(Item);
            Count = Inventory.GetCurrentItemCount(Item);
            this.Bindings.Update();
        }
    }
}
