using ChecklistTracker.Config.Layout;
using ChecklistTracker.Config.Layout.GossipNotebook.Components;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChecklistTracker.Controls;

internal static class ControlExtensions
{
    public static void SetPosition(this UIElement control, Position position)
    {
        if (control is null)
        {
            throw new ArgumentNullException(nameof(control));
        }
        if (position is null)
        {
            throw new ArgumentNullException(nameof(position));
        }
        control.SetValue(Canvas.LeftProperty, (double)position.X);
        control.SetValue(Canvas.TopProperty, (double)position.Y);
    }
}
