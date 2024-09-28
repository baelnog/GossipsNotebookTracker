namespace ChecklistTracker.Layout
{
    public interface IStyle
    {
        public string? Title { get; }

        public int? Width { get; }

        public int? Height { get; }

        public string? BackgroundColor { get; }

        public string? FontColor { get; }

        public string? FontFamilty { get; }

        public int? FontSize { get; }

        public string? Padding { get; }
    }
}
