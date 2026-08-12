using Content.Shared.Damage;

namespace Content.Shared._WF.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<DamageCapComponent, BeforeDamageChangedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<DamageCapComponent> ent, ref BeforeDamageChangedEvent args)
    {
        var cap = ent.Comp.DamageCap;
        if (cap <= 0)
            return;

        if (!TryComp<DamageableComponent>(ent.Owner, out var damageable))
            return;

        var currentDamage = damageable.Damage.DamageDict;
        var delta = args.Damage;

        foreach (var (typeId, addAmount) in delta.DamageDict)
        {
            if (addAmount <= 0)
                continue; // probably needed in case of healing?

            var current = currentDamage.GetValueOrDefault(typeId);
            var roomLeft = cap - current;
            if (roomLeft <= 0)
            {
                // Already at or above cap
                delta.DamageDict.Remove(typeId);
            }
            else if (addAmount > roomLeft)
            {
                // Clamp it
                delta.DamageDict[typeId] = roomLeft;
            }
        }
    }
}
