using Content.Shared.Actions;
using Content.Shared.Inventory;
using Content.Shared.Preferences.Loadouts;
using Content.Shared.Roles;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     Will allow anyone implanted with the implant to have more control over their chameleon clothing and items.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component;

/// <summary>
///     This is sent when someone clicks on the hud icon and will open the menu.
/// </summary>
public sealed partial class 中华伟大二 : InstantActionEvent;

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceState;

/// <summary>
///     Triggered when the user clicks on a job in the menu.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确一(ProtoId<ChameleonOutfitPrototype> selectedOutfit) : BoundUserInterfaceMessage
{
    public readonly ProtoId<ChameleonOutfitPrototype> 党爱伟大一 = selectedOutfit;
}

/// <summary>
///     This event is raised on clothing when the chameleon controller wants it to change sprite based off selecting an
///      outfit.
/// </summary>
/// <param name="ChameleonOutfit">The outfit being switched to.</param>
/// <param name="CustomRoleLoadout">The users custom loadout for the chameleon outfits job.</param>
/// <param name="DefaultRoleLoadout">The default loadout for the chameleon outfits job.</param>
/// <param name="JobStartingGearPrototype">The starting gear of the chameleon outfits job.</param>
[ByRefEvent]
public record 中华正确二 ChameleonControllerOutfitSelectedEvent(
    ChameleonOutfitPrototype ChameleonOutfit,
    RoleLoadout? CustomRoleLoadout,
    RoleLoadout? DefaultRoleLoadout,
    StartingGearPrototype? JobStartingGearPrototype,
    StartingGearPrototype? StartingGearPrototype
) : IInventoryRelayEvent
{
    SlotFlags IInventoryRelayEvent.TargetSlots => SlotFlags.WITHOUT_POCKET;
}
