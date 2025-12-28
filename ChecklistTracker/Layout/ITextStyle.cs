using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;

namespace ChecklistTracker.Layout
{
    public interface ITextStyle
    {
        public double? FontSize { get; }
        public string? TextColor { get; }

        public string? TextBackgroundColor { get; }

        public string? FontFamily { get; }

        public string? FontStyle { get; }
        public string? FontWeight { get; }

        public double? TextBackgroundOpacity { get; }
    }
}
