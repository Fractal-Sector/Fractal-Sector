using Content.Shared.Clothing.Components;
using Content.Shared.Inventory;
using Content.Shared.Movement.Events;

namespace Content.Shared.Clothing.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SpeedModifierContactCapClothingComponent, InventoryRelayedEvent<GetSpeedModifierContactCapEvent>>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<SpeedModifierContactCapClothingComponent> ent, ref InventoryRelayedEvent<GetSpeedModifierContactCapEvent> args)
    {
        args.Args.SetIfMax(ent.Comp.MaxContactSprintSlowdown, ent.Comp.MaxContactWalkSlowdown);
    }
}
