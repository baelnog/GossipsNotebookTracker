using System;
using System.Collections.Generic;
using System.Text;

namespace ChecklistTracker.Layout
{
    internal class CoalescedTextStyle : ITextStyle
    {
        readonly IEnumerable<ITextStyle> Styles;

        public CoalescedTextStyle(params ITextStyle[] styles)
        {
            Styles = styles.Where(s => s != null).ToList();
        }

        public string? TextColor => Styles.Select(s => s.TextColor).FirstOrDefault(s => s != null);

        public string? TextBackgroundColor => Styles.Select(s => s.TextBackgroundColor).FirstOrDefault(s => s != null);

        public string? FontFamily => Styles.Select(s => s.FontFamily).FirstOrDefault(s => s != null);

        public string? FontStyle => Styles.Select(s => s.FontStyle).FirstOrDefault(s => s != null);
        public string? FontWeight => Styles.Select(s => s.FontWeight).FirstOrDefault(s => s != null);

        public double? FontSize => Styles.Select(s => s.FontSize).FirstOrDefault(s => s != null);

        public double? TextBackgroundOpacity => Styles.Select(s => s.TextBackgroundOpacity).FirstOrDefault(s => s != null);
    }
}
