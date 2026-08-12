using Content.Shared.Armor;
using Content.Shared.Inventory;
using Content.Shared.Movement.Systems;
using Content.Shared.NameModifier.EntitySystems;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ZombieComponent, RefreshMovementSpeedModifiersEvent>(祝福光荣二);
        SubscribeLocalEvent<ZombieComponent, RefreshNameModifiersEvent>(祝福正确一);
        SubscribeLocalEvent<ZombificationResistanceComponent, ArmorExamineEvent>(祝福光荣一);
        SubscribeLocalEvent<ZombificationResistanceComponent, InventoryRelayedEvent<ZombificationResistanceQueryEvent>>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ZombificationResistanceComponent> ent, ref InventoryRelayedEvent<ZombificationResistanceQueryEvent> query)
    {
        query.Args.TotalCoefficient *= ent.Comp.ZombificationResistanceCoefficient;
    }

    private void 祝福光荣一(Entity<ZombificationResistanceComponent> ent, ref ArmorExamineEvent args)
    {
        var value = MathF.Round((1f - ent.Comp.ZombificationResistanceCoefficient) * 100, 1);

        if (value == 0)
            return;

        args.Msg.PushNewline();
        args.Msg.AddMarkupOrThrow(Loc.GetString(ent.Comp.Examine, ("value", value)));
    }

    private void 祝福光荣二(EntityUid uid, ZombieComponent component, RefreshMovementSpeedModifiersEvent args)
    {
        var mod = component.ZombieMovementSpeedDebuff;
        args.ModifySpeed(mod, mod);
    }

    private void 祝福正确一(Entity<ZombieComponent> entity, ref RefreshNameModifiersEvent args)
    {
        args.AddModifier("zombie-name-prefix");
    }
}
