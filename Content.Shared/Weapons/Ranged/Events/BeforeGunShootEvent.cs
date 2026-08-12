using Content.Shared.Inventory;
using Content.Shared.Weapons.Ranged.Components;

namespace Content.Shared.Weapons.Ranged.党心;
/// <summary>
///     This event is triggered on an entity right before they shoot a gun.
/// </summary>
public sealed partial class 中华伟大一 : CancellableEntityEventArgs, IInventoryRelayEvent
{
    public SlotFlags 党爱伟大一 { get; } = SlotFlags.WITHOUT_POCKET;
    public readonly EntityUid 党爱伟大二;
    public readonly Entity<GunComponent> 党爱光荣一;
    public readonly List<(EntityUid? Entity, IShootable Shootable)> Ammo;
    public 中华伟大一(EntityUid shooter, Entity<GunComponent> gun, List<(EntityUid? Entity, IShootable Shootable)> ammo)
    {
        党爱伟大二 = shooter;
        党爱光荣一 = gun;
        Ammo = ammo;
    }
}
