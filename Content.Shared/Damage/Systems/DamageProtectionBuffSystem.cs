using Content.Shared.Damage.Components;

namespace Content.Shared.Damage.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DamageProtectionBuffComponent, DamageModifyEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, DamageProtectionBuffComponent component, DamageModifyEvent args)
    {
        foreach (var modifier in component.Modifiers.Values)
            args.Damage = DamageSpecifier.ApplyModifierSet(args.Damage, modifier);
    }
}
