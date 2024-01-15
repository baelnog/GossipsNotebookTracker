using ChecklistTracker.Config;
using ChecklistTracker.Controls.Click;
using ChecklistTracker.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;

namespace ChecklistTracker.Controls
{
    internal partial class HintStoneControl : UserControl
    {
        internal LayoutParams _LayoutParams;
        internal LayoutParams LayoutParams 
        {
            get => _LayoutParams;
            set
            {
                if (_LayoutParams != value)
                {
                    _LayoutParams = value;
                    Margin = _LayoutParams.Padding;
                }
            }
        }

        private HintStoneViewModel _ViewModel;
        internal HintStoneViewModel ViewModel 
        {
            get => _ViewModel;
            set
            {
                if (_ViewModel != value)
                {
                    _ViewModel = value;
                    InitClickCallbacks();
                }
            }
        }

        internal HintStoneControl()
        {
            InitializeComponent();
        }

        internal HintStoneControl(HintStoneViewModel viewModel, LayoutParams layout) : this()
        {
            ViewModel = viewModel;
            LayoutParams = layout;
        }

        private void InitClickCallbacks()
        {
            var callbacks = new ClickCallbacks();
            callbacks.OnClick = ViewModel.OnClick;
            callbacks.OnScroll = ViewModel.OnScroll;
            callbacks.OnDragImageCompleted = ViewModel.OnDrag;
            callbacks.OnDropImageCompleted = ViewModel.OnDrop;

            this.ConfigureClickHandler(callbacks);
        }
    }
}
