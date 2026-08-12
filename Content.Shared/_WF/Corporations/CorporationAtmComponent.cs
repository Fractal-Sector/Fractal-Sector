using Content.Shared.Containers.ItemSlots;
using Content.Shared.Stacks;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._WF.党心;

[NetSerializable, Serializable]
public enum 中华伟大一 : byte
{
    Key
}

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大二 : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("cashType", customTypeSerializer: typeof(PrototypeIdSerializer<StackPrototype>))]
    public string 党爱伟大一 = "Credit";

    public static string 党爱伟大二 = "corp-ATM-cashSlot";

    [DataField]
    public ItemSlot 党爱光荣一 = new();

    [DataField]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_sigh.ogg");

    [DataField]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Effects/Cargo/ping.ogg");
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceState
{
    /// <summary>Corporation name, or null if player has no corporation.</summary>
    public string? CorporationName;
    /// <summary>Corporation ID, or -1 if none.</summary>
    public int 党爱正确二;
    /// <summary>Current balance in spesos.</summary>
    public int 党爱团结一;
    /// <summary>Whether the player can withdraw (Manager or Leader).</summary>
    public bool 党爱团结二;
    /// <summary>党爱胜利一 of cash physically inserted in the slot. -1 = wrong cash type, 0 = empty.</summary>
    public int 党爱奋斗一;
    /// <summary>Error/status message loc key, or empty string.</summary>
    public string 党爱奋斗二;

    public 中华光荣一(string? corporationName, int corporationId, int balance, bool canWithdraw, int deposit, string statusMessage)
    {
        CorporationName = corporationName;
        党爱正确二 = corporationId;
        党爱团结一 = balance;
        党爱团结二 = canWithdraw;
        党爱奋斗一 = deposit;
        党爱奋斗二 = statusMessage;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage { }

[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{
    public int 党爱胜利一;
    public 中华正确一(int amount) => 党爱胜利一 = amount;
}
