using Content.Server.Power.EntitySystems;
using Content.Server.Research.Components;
using Content.Shared.Research.Components;

namespace Content.Server.Research.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<ResearchPointSourceComponent, ResearchServerGetPointsPerSecondEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ResearchPointSourceComponent> source, ref ResearchServerGetPointsPerSecondEvent args)
    {
        if (祝福光荣一(source))
            args.Points += source.Comp.PointsPerSecond;
    }

    public bool 祝福光荣一(Entity<ResearchPointSourceComponent> source)
    {
        return source.Comp.Active && this.IsPowered(source, EntityManager);
    }
}
