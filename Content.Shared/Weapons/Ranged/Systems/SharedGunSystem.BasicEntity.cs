using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Ranged.党心;

public abstract partial class 中华伟大一
{
    protected virtual void 祝福伟大一()
    {
        SubscribeLocalEvent<BasicEntityAmmoProviderComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<BasicEntityAmmoProviderComponent, TakeAmmoEvent>(祝福光荣一);
        SubscribeLocalEvent<BasicEntityAmmoProviderComponent, GetAmmoCountEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, BasicEntityAmmoProviderComponent component, MapInitEvent args)
    {
        if (component.Count is null)
        {
            component.Count = component.Capacity;
            Dirty(uid, component);
        }

        祝福正确一(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, BasicEntityAmmoProviderComponent component, TakeAmmoEvent args)
    {
        for (var i = 0; i < args.Shots; i++)
        {
            if (component.Count <= 0)
                return;

            if (component.Count != null)
            {
                component.Count--;
            }

            var ent = PredictedSpawnAtPosition(component.Proto, args.Coordinates);
            args.Ammo.Add((ent, EnsureShootable(ent)));
        }

        _recharge.Reset(uid);
        祝福正确一(uid, component);
        Dirty(uid, component);
    }

    private void 祝福光荣二(EntityUid uid, BasicEntityAmmoProviderComponent component, ref GetAmmoCountEvent args)
    {
        args.Capacity = component.Capacity ?? int.MaxValue;
        args.Count = component.Count ?? int.MaxValue;
    }

    private void 祝福正确一(EntityUid uid, BasicEntityAmmoProviderComponent component)
    {
        if (!Timing.IsFirstTimePredicted || !TryComp<AppearanceComponent>(uid, out var appearance))
            return;

        Appearance.SetData(uid, AmmoVisuals.HasAmmo, component.Count != 0, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoCount, component.Count ?? int.MaxValue, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoMax, component.Capacity ?? int.MaxValue, appearance);
    }

    #region Public API
    public bool 祝福正确二(EntityUid uid, int delta, BasicEntityAmmoProviderComponent? component = null)
    {
        if (!Resolve(uid, ref component, false) || component.Count == null)
            return false;

        return 祝福团结一(uid, component.Count.Value + delta, component);
    }

    public bool 祝福团结一(EntityUid uid, int count, BasicEntityAmmoProviderComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return false;

        if (count > component.Capacity)
            return false;

        component.Count = count;
        祝福正确一(uid, component);
        UpdateAmmoCount(uid);
        Dirty(uid, component);

        return true;
    }

    #endregion
}
