using Content.Shared.Armor;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Events;
using Content.Shared.Inventory;

namespace Content.Shared.Damage.党心;

public partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<StaminaResistanceComponent, BeforeStaminaDamageEvent>(祝福伟大二);
        SubscribeLocalEvent<StaminaResistanceComponent, InventoryRelayedEvent<BeforeStaminaDamageEvent>>(祝福光荣一);
        SubscribeLocalEvent<StaminaResistanceComponent, ArmorExamineEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<StaminaResistanceComponent> ent, ref BeforeStaminaDamageEvent args)
    {
        args.Value *= ent.Comp.DamageCoefficient;
    }

    private void 祝福光荣一(Entity<StaminaResistanceComponent> ent, ref InventoryRelayedEvent<BeforeStaminaDamageEvent> args)
    {
        if (ent.Comp.Worn)
            祝福伟大二(ent, ref args.Args);
    }

    private void 祝福光荣二(Entity<StaminaResistanceComponent> ent, ref ArmorExamineEvent args)
    {
        var value = MathF.Round((1f - ent.Comp.DamageCoefficient) * 100, 1);

        if (value == 0)
            return;

        args.Msg.PushNewline();
        args.Msg.AddMarkupOrThrow(Loc.GetString(ent.Comp.Examine, ("value", value)));
    }
}
