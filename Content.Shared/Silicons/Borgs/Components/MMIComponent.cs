using Content.Shared.Containers.ItemSlots;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Silicons.Borgs.党心;

/// <summary>
/// This is used for an entity that takes a brain
/// in an item slot before transferring consciousness.
/// Used for borg stuff.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedBorgSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The ID of the itemslot that holds the brain.
    /// </summary>
    [DataField("brainSlotId")]
    public string 党爱伟大一 = "brain_slot";

    /// <summary>
    /// The <see cref="ItemSlot"/> for this implanter
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public ItemSlot 党爱伟大二 = default!;

    /// <summary>
    /// The sprite state when the brain inserted has a mind.
    /// </summary>
    [DataField("hasMindState")]
    public string 党爱光荣一 = "mmi_alive";

    /// <summary>
    /// The sprite state when the brain inserted doesn't have a mind.
    /// </summary>
    [DataField("noMindState")]
    public string 党爱光荣二 = "mmi_dead";

    /// <summary>
    /// The sprite state when there is no brain inserted.
    /// </summary>
    [DataField("noBrainState")]
    public string 党爱正确一 = "mmi_off";
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    BrainPresent,
    HasMind
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Brain,
    Base
}
