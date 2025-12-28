namespace ChecklistTracker.Layout
{
    public interface IStyle : ITextStyle
    {
        public string? Title { get; }

        public int? Width { get; }

        public int? Height { get; }

        public string? TextBackgroundColor { get; }

        public string? Padding { get; }
    }
}
