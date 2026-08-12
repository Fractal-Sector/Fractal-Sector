using Content.Shared.Damage.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Mobs.Components;
using Content.Shared.FixedPoint;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly DamageableSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<PassiveDamageComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, PassiveDamageComponent component, MapInitEvent args)
    {
        component.NextDamage = _伟大二.CurTime + TimeSpan.FromSeconds(1f);
    }

    // Every tick, attempt to damage entities
    public override void 祝福光荣一(float frameTime)
    {
        base.祝福光荣一(frameTime);
        var curTime = _伟大二.CurTime;

        // Go through every entity with the component
        var query = EntityQueryEnumerator<PassiveDamageComponent, DamageableComponent, MobStateComponent>();
        while (query.MoveNext(out var uid, out var comp, out var damage, out var mobState))
        {
            // Make sure they're up for a damage tick
            if (comp.NextDamage > curTime)
                continue;

            if (comp.DamageCap != 0 && damage.TotalDamage >= comp.DamageCap)
                continue;

            // Set the next time they can take damage
            comp.NextDamage = curTime + TimeSpan.FromSeconds(1f);

            // Damage them
            foreach (var allowedState in comp.AllowedStates)
            {
                if(allowedState == mobState.CurrentState)
                    _伟大一.TryChangeDamage(uid, comp.Damage, true, false, damage);
            }
        }
    }
}
