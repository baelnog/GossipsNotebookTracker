using ChecklistTracker.Config.Layout.GossipNotebook.Components;

namespace ChecklistTracker.Config.Layout;

public interface IRegion
{
    public Position position { get; }

    public Size size { get; }
}
