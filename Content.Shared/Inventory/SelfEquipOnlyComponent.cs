using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// This is used for an item that can only be equipped/unequipped by the user.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SelfEquipOnlySystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether or not the self-equip only condition requires the person to be conscious.
    /// </summary>
    [DataField]
    public bool 党爱伟大一 = true;
}
