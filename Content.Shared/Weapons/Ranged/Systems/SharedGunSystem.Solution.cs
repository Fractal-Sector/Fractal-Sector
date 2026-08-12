using Content.Shared.Chemistry.Components;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Map;

namespace Content.Shared.Weapons.Ranged.党心;

public partial class 中华伟大一
{
    protected virtual void 祝福伟大一()
    {
        SubscribeLocalEvent<SolutionAmmoProviderComponent, TakeAmmoEvent>(祝福伟大二);
        SubscribeLocalEvent<SolutionAmmoProviderComponent, GetAmmoCountEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, SolutionAmmoProviderComponent component, TakeAmmoEvent args)
    {
        var shots = Math.Min(args.Shots, component.Shots);

        // Don't dirty if it's an empty fire.
        if (shots == 0)
            return;

        for (var i = 0; i < shots; i++)
        {
            args.Ammo.Add(GetSolutionShot(uid, component, args.Coordinates));
            component.Shots--;
        }

        祝福光荣二(uid, component);
        祝福正确一(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, SolutionAmmoProviderComponent component, ref GetAmmoCountEvent args)
    {
        args.Count = component.Shots;
        args.Capacity = component.MaxShots;
    }

    protected virtual void 祝福光荣二(EntityUid uid, SolutionAmmoProviderComponent component, Solution? solution = null)
    {

    }

    protected virtual (EntityUid Entity, IShootable) GetSolutionShot(EntityUid uid, SolutionAmmoProviderComponent component, EntityCoordinates position)
    {
        var ent = PredictedSpawnAtPosition(component.Prototype, position);
        return (ent, EnsureShootable(ent));
    }

    protected void 祝福正确一(EntityUid uid, SolutionAmmoProviderComponent component)
    {
        if (!TryComp<AppearanceComponent>(uid, out var appearance))
            return;

        Appearance.SetData(uid, AmmoVisuals.HasAmmo, component.Shots != 0, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoCount, component.Shots, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoMax, component.MaxShots, appearance);
    }
}
