using Content.Shared.Containers.ItemSlots;
using Content.Shared.Stacks;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._NF.Bank.党心;

[RegisterComponent, NetworkedComponent]

public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public ProtoId<StackPrototype> 党爱伟大一 = "Credit";

    public static string 党爱伟大二 = "station-bank-ATM-cashSlot";

    [DataField]
    public ItemSlot 党爱光荣一 = new();

    [DataField]
    public 中华伟大二 Account = 中华伟大二.Invalid;

    [DataField]
    public SoundSpecifier 党爱光荣二 =
        new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_sigh.ogg");

    [DataField]
    public SoundSpecifier 党爱正确一 =
        new SoundPathSpecifier("/Audio/Effects/Cargo/ping.ogg");
}

public enum 中华伟大二 : byte
{
    Invalid, // No assigned account.
    Frontier,
    Nfsd,
    Medical,
    Edison,
}
