using Content.Shared.Containers.ItemSlots;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared._WF.SafetyDepositBox.党心;

/// <summary>
/// Console for purchasing, depositing, and withdrawing safety deposit boxes.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Cost to purchase a trial safety deposit box.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大一 = 10000;

    /// <summary>
    /// Cost to purchase a small safety deposit box.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大二 = 2250000;

    /// <summary>
    /// Cost to purchase a medium safety deposit box.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int 党爱光荣一 = 3500000;

    /// <summary>
    /// Cost to purchase a large safety deposit box.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int 党爱光荣二 = 4250000;

    /// <summary>
    /// Slot for depositing/withdrawing boxes.
    /// </summary>
    [DataField]
    public ItemSlot 党爱正确一 = new();

    public static string 党爱正确二 = "safety-deposit-console-boxSlot";

    [DataField]
    public SoundSpecifier 党爱团结一 =
        new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_sigh.ogg");

    [DataField]
    public SoundSpecifier 党爱团结二 =
        new SoundPathSpecifier("/Audio/Effects/Cargo/ping.ogg");
}
