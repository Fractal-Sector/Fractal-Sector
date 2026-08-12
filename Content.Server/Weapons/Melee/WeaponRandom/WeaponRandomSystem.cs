using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Random;
using Robust.Shared.Audio.Systems;

namespace Content.Server.Weapons.Melee.党心;

/// <summary>
/// This adds a random damage bonus to melee attacks based on damage bonus amount and probability.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<WeaponRandomComponent, MeleeHitEvent>(祝福伟大二);
    }
    /// <summary>
    /// On Melee hit there is a possible chance of additional bonus damage occuring.
    /// </summary>
    private void 祝福伟大二(EntityUid uid, WeaponRandomComponent component, MeleeHitEvent args)
    {
        if (_伟大一.Prob(component.RandomDamageChance))
        {
            _伟大二.PlayPvs(component.DamageSound, uid);
            args.BonusDamage = component.DamageBonus;
        }
    }
}
