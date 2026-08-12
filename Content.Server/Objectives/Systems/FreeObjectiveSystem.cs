using Content.Server.Objectives.Components;
using Content.Shared.Objectives.Components;

namespace Content.Server.Objectives.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<FreeObjectiveComponent, ObjectiveGetProgressEvent>(祝福伟大二);
    }

    // You automatically greentext, there's not much else to it
    private void 祝福伟大二(Entity<FreeObjectiveComponent> ent, ref ObjectiveGetProgressEvent args)
    {
        args.Progress = 1f;
    }
}
