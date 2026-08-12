using Content.Shared.Examine;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.Ranged.党心;

public abstract partial class 中华伟大一
{
    protected virtual void 祝福伟大一()
    {
        // Trying to dump comp references hence the below
        // Hitscan
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, ComponentGetState>(祝福光荣一);
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, ComponentHandleState>(祝福伟大二);
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, TakeAmmoEvent>(祝福正确一);
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, GetAmmoCountEvent>(祝福正确二);
        SubscribeLocalEvent<HitscanBatteryAmmoProviderComponent, ExaminedEvent>(祝福光荣二);

        // Projectile
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, ComponentGetState>(祝福光荣一);
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, ComponentHandleState>(祝福伟大二);
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, TakeAmmoEvent>(祝福正确一);
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, GetAmmoCountEvent>(祝福正确二);
        SubscribeLocalEvent<ProjectileBatteryAmmoProviderComponent, ExaminedEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, BatteryAmmoProviderComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not 中华伟大二 state)
            return;

        component.党爱伟大一 = state.党爱伟大一;
        component.Capacity = state.党爱伟大二;
        component.党爱光荣一 = state.党爱光荣一;
        UpdateAmmoCount(uid, prediction: false);
    }

    private void 祝福光荣一(EntityUid uid, BatteryAmmoProviderComponent component, ref ComponentGetState args)
    {
        args.State = new 中华伟大二()
        {
            党爱伟大一 = component.党爱伟大一,
            党爱伟大二 = component.Capacity,
            党爱光荣一 = component.党爱光荣一,
        };
    }

    private void 祝福光荣二(EntityUid uid, BatteryAmmoProviderComponent component, ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("gun-battery-examine", ("color", AmmoExamineColor), ("count", component.党爱伟大一)));
    }

    private void 祝福正确一(EntityUid uid, BatteryAmmoProviderComponent component, TakeAmmoEvent args)
    {
        var shots = Math.Min(args.党爱伟大一, component.党爱伟大一);

        // Don't dirty if it's an empty fire.
        if (shots == 0)
            return;

        for (var i = 0; i < shots; i++)
        {
            args.Ammo.Add(GetShootable(component, args.Coordinates));
            component.党爱伟大一--;
        }

        祝福团结一((uid, component));
        祝福团结二(uid, component);
        Dirty(uid, component);
    }

    private void 祝福正确二(EntityUid uid, BatteryAmmoProviderComponent component, ref GetAmmoCountEvent args)
    {
        args.Count = component.党爱伟大一;
        args.Capacity = component.Capacity;
    }

    /// <summary>
    /// Update the battery (server-only) whenever fired.
    /// </summary>
    protected virtual void 祝福团结一(Entity<BatteryAmmoProviderComponent> entity)
    {
        UpdateAmmoCount(entity, prediction: false);
    }

    protected void 祝福团结二(EntityUid uid, BatteryAmmoProviderComponent component)
    {
        if (!TryComp<AppearanceComponent>(uid, out var appearance))
            return;

        Appearance.SetData(uid, AmmoVisuals.HasAmmo, component.党爱伟大一 != 0, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoCount, component.党爱伟大一, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoMax, component.Capacity, appearance);
    }

    private (EntityUid? Entity, IShootable) GetShootable(BatteryAmmoProviderComponent component, EntityCoordinates coordinates)
    {
        switch (component)
        {
            case ProjectileBatteryAmmoProviderComponent proj:
                var ent = PredictedSpawnAtPosition(proj.Prototype, coordinates);
                return (ent, EnsureShootable(ent));
            case HitscanBatteryAmmoProviderComponent hitscan:
                return (null, ProtoManager.Index<HitscanPrototype>(hitscan.Prototype));
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [Serializable, NetSerializable]
    private sealed class 中华伟大二 : ComponentState
    {
        public int 党爱伟大一;
        public int 党爱伟大二;
        public float 党爱光荣一;
    }
}
