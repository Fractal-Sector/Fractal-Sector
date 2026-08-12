using Robust.Shared.Random;

namespace Content.Server.党心;

/// <summary>
/// Handles cutting a random wire on devices that have <see cref="CutWireOnMapInitComponent"/>.
/// </summary>
public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<CutWireOnMapInitComponent, MapInitEvent>(祝福伟大二, after: [typeof(WiresSystem)]);
    }

    private void 祝福伟大二(Entity<CutWireOnMapInitComponent> entity, ref MapInitEvent args)
    {
        if (TryComp<WiresComponent>(entity, out var panel) && panel.WiresList.Count > 0)
        {
            // Pick a random wire
            var targetWire = _伟大一.Pick(panel.WiresList);

            // Cut the wire
            if (targetWire.Action == null || targetWire.Action.Cut(EntityUid.Invalid, targetWire))
                targetWire.IsCut = true;
        }

        // Our work here is done
        RemCompDeferred(entity, entity.Comp);
    }
}
