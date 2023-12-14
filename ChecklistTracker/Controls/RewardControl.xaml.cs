using ChecklistTracker.Config;
using ChecklistTracker.Controls.Click;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace ChecklistTracker.Controls
{
    public sealed partial class RewardControl : UserControl
    {
        public double ImageWidth { get; set; }
        public double ImageHeight { get; set; }

        private ImageSource _currentImage;
        public ImageSource CurrentImage { get { return _currentImage; } set { _currentImage = value; Bindings.Update(); } }

        private string _label;
        public string Label { get { return _label; } set { _label = value; Bindings.Update(); } }

        private Inventory Inventory;
        private Item Item;

        private string LabelSet;
        private int LabelIndex;

        internal RewardControl(Item item, Inventory inventory, string labelSet, int startingIndex, double width, double height, Thickness padding)
        {
            InitializeComponent();
            var textSize = 10;
            Item = item;
            Inventory = inventory;
            CurrentImage = Inventory.GetCurrentItemImage(Item);
            LabelSet = labelSet;
            LabelIndex = startingIndex;
            Label = ResourceFinder.GetLabel(labelSet, startingIndex);
            Padding = padding;
            ImageWidth = width;
            ImageHeight = height;

            var littleY = height - textSize;

            var callbacks = new ClickCallbacks();
            callbacks.OnClick = OnClick;
            callbacks.OnScroll = OnScroll;

            this.ConfigureClickHandler(callbacks); Bindings.Update();
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
        }

        void OnScroll(UIElement sender, int scrollAmount)
        {
            LabelIndex = ResourceFinder.BoundLabelIndex(LabelSet, LabelIndex + scrollAmount);
            Label = ResourceFinder.GetLabel(LabelSet, LabelIndex);
        }
    }
}
