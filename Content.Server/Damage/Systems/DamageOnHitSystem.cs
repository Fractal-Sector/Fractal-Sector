using Content.Server.Damage.Components;
using Content.Shared.Damage;
using Robust.Shared.Player;
using Content.Shared.Weapons.Melee.Events;
using System.Linq;

namespace Content.Server.Damage.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly DamageableSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<DamageOnHitComponent, MeleeHitEvent>(祝福伟大二);
    }
    // Looks for a hit, then damages the held item an appropriate amount.
    private void 祝福伟大二(EntityUid uid, DamageOnHitComponent component, MeleeHitEvent args)
    {
        if (args.HitEntities.Any()) {
            _伟大一.TryChangeDamage(uid, component.Damage, component.IgnoreResistances);
        }
    }
}
