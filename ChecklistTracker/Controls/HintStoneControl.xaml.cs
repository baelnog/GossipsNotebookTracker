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
        internal LayoutParams Layout { get; private set; }
        internal HintStoneViewModel ViewModel { get; private set; }

        internal HintStoneControl(HintStoneViewModel viewModel, LayoutParams layout)
        {
            InitializeComponent();
            ViewModel = viewModel;
            Layout = layout;
            Margin = layout.Padding;

            var callbacks = new ClickCallbacks();
            callbacks.OnClick = ViewModel.OnClick;
            callbacks.OnScroll = ViewModel.OnScroll;
            callbacks.OnDragImageCompleted = ViewModel.OnDrag;
            callbacks.OnDropImageCompleted = ViewModel.OnDrop;

            this.ConfigureClickHandler(callbacks);
        }
    }
}
