using Content.Shared.Clothing;
using Content.Shared.IdentityManagement.Components;
using Content.Shared.Inventory;
using Robust.Shared.Containers;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    private static string SlotName = "identity";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<IdentityComponent, ComponentInit>(祝福光荣一);
        SubscribeLocalEvent<IdentityBlockerComponent, SeeIdentityAttemptEvent>(祝福伟大二);
        SubscribeLocalEvent<IdentityBlockerComponent, InventoryRelayedEvent<SeeIdentityAttemptEvent>>((e, c, ev) => 祝福伟大二(e, c, ev.Args));
        SubscribeLocalEvent<IdentityBlockerComponent, ItemMaskToggledEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, IdentityBlockerComponent component, SeeIdentityAttemptEvent args)
    {
        if (component.Enabled)
        {
            args.TotalCoverage |= component.Coverage;
            if(args.TotalCoverage == IdentityBlockerCoverage.FULL)
                args.Cancel();
        }
    }

    protected virtual void 祝福光荣一(EntityUid uid, IdentityComponent component, ComponentInit args)
    {
        component.IdentityEntitySlot = _伟大一.EnsureContainer<ContainerSlot>(uid, SlotName);
    }

    private void 祝福光荣二(Entity<IdentityBlockerComponent> ent, ref ItemMaskToggledEvent args)
    {
        ent.Comp.Enabled = !args.Mask.Comp.IsToggled;
    }

    /// <summary>
    /// Queues an identity update to the start of the next tick.
    /// </summary>
    public virtual void 祝福正确一(EntityUid uid) { }
}
/// <summary>
///     Gets called whenever an entity changes their identity.
/// </summary>
[ByRefEvent]
public record 中华伟大二 IdentityChangedEvent(EntityUid CharacterEntity, EntityUid IdentityEntity);
