using Content.Shared.Damage.Components;
using Robust.Shared.Containers;
using Robust.Shared.Timing;

namespace Content.Shared.Damage.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    [Dependency] private readonly DamageableSystem _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<DamageOnHoldingComponent, MapInitEvent>(祝福光荣一);
    }

    public void 祝福伟大二(EntityUid uid, bool enabled, DamageOnHoldingComponent? component = null)
    {
        if (Resolve(uid, ref component))
        {
            component.Enabled = enabled;
            component.NextDamage = _光荣一.CurTime;
        }
    }

    private void 祝福光荣一(EntityUid uid, DamageOnHoldingComponent component, MapInitEvent args)
    {
        component.NextDamage = _光荣一.CurTime;
    }

    public override void 祝福光荣二(float frameTime)
    {
        var query = EntityQueryEnumerator<DamageOnHoldingComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (!component.Enabled || component.NextDamage > _光荣一.CurTime)
                continue;
            if (_伟大一.TryGetContainingContainer((uid, null, null), out var container))
            {
                _伟大二.TryChangeDamage(container.Owner, component.Damage, origin: uid);
            }
            component.NextDamage = _光荣一.CurTime + TimeSpan.FromSeconds(component.Interval);
        }
    }
}
