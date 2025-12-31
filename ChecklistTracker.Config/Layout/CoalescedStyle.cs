using System.Collections.Generic;
using System.Linq;

namespace ChecklistTracker.Layout.GossipNotebook
{
    public class CoalescedStyle : IStyle
    {
        readonly IEnumerable<IStyle> Styles;

        public CoalescedStyle(params IStyle[] styles)
        {
            Styles = styles.Where(s => s != null).ToList();
        }

        public string? Title => Styles.Select(s => s.Title).FirstOrDefault(s => s != null);

        public int? Width => Styles.Select(s => s.Width).FirstOrDefault(s => s != null);

        public int? Height => Styles.Select(s => s.Height).FirstOrDefault(s => s != null);

        public string? TextBackgroundColor => Styles.Select(s => s.TextBackgroundColor).FirstOrDefault(s => s != null);

        public string? TextColor => Styles.Select(s => s.TextColor).FirstOrDefault(s => s != null);

        public string? FontFamily => Styles.Select(s => s.FontFamily).FirstOrDefault(s => s != null);
        public string? FontStyle => Styles.Select(s => s.FontStyle).FirstOrDefault(s => s != null);
        public string? FontWeight => Styles.Select(s => s.FontWeight).FirstOrDefault(s => s != null);

        public double? FontSize => Styles.Select(s => s.FontSize).FirstOrDefault(s => s != null);

        public string? Padding => Styles.Select(s => s.Padding).FirstOrDefault(s => s != null);

        public double? TextBackgroundOpacity => Styles.Select(s => s.TextBackgroundOpacity).FirstOrDefault(s => s != null);
    }
}
