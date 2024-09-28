using System.Collections.Generic;
using System.Linq;

namespace ChecklistTracker.Layout.GossipNotebook
{
    class CoalescedStyle : IStyle
    {
        readonly IEnumerable<IStyle> Styles;

        public CoalescedStyle(params IStyle[] styles)
        {
            Styles = styles.Where(s => s != null).ToList();
        }

        public string? Title => Styles.Select(s => s.Title).FirstOrDefault(s => s != null);

        public int? Width => Styles.Select(s => s.Width).FirstOrDefault(s => s != null);

        public int? Height => Styles.Select(s => s.Height).FirstOrDefault(s => s != null);

        public string? BackgroundColor => Styles.Select(s => s.BackgroundColor).FirstOrDefault(s => s != null);

        public string? FontColor => Styles.Select(s => s.FontColor).FirstOrDefault(s => s != null);

        public string? FontFamilty => Styles.Select(s => s.FontFamilty).FirstOrDefault(s => s != null);

        public int? FontSize => Styles.Select(s => s.FontSize).FirstOrDefault(s => s != null);

        public string? Padding => Styles.Select(s => s.Padding).FirstOrDefault(s => s != null);
    }
}
