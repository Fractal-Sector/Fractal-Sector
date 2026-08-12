using Content.Shared.Armor;
using Content.Shared.Atmos;
using Content.Shared.Clothing.Components;
using Content.Shared.Inventory;

namespace Content.Shared.Clothing.党心;

/// <summary>
/// Handles reducing fire damage when wearing clothing with <see cref="FireProtectionComponent"/>.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<FireProtectionComponent, InventoryRelayedEvent<GetFireProtectionEvent>>(祝福伟大二);
        SubscribeLocalEvent<FireProtectionComponent, ArmorExamineEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<FireProtectionComponent> ent, ref InventoryRelayedEvent<GetFireProtectionEvent> args)
    {
        args.Args.Reduce(ent.Comp.Reduction);
    }

    private void 祝福光荣一(Entity<FireProtectionComponent> ent, ref ArmorExamineEvent args)
    {
        var value = MathF.Round(ent.Comp.Reduction * 100, 1);

        if (value == 0)
            return;

        args.Msg.PushNewline();
        args.Msg.AddMarkupOrThrow(Loc.GetString(ent.Comp.ExamineMessage, ("value", value)));
    }
}
