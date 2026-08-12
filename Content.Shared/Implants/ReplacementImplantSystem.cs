using Content.Shared.Implants.Components;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    [Dependency] private readonly EntityWhitelistSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ReplacementImplantComponent, ImplantImplantedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ReplacementImplantComponent> ent, ref ImplantImplantedEvent args)
    {
        if (!_伟大一.TryGetContainer(args.Implanted, ImplanterComponent.ImplantSlotId, out var implantContainer))
            return;

        foreach (var implant in implantContainer.ContainedEntities)
        {
            if (implant == ent.Owner)
                continue; // don't delete the replacement

            if (_伟大二.IsWhitelistPass(ent.Comp.Whitelist, implant))
                PredictedQueueDel(implant);
        }

    }
}
