using System.Numerics;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Stacks;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._NF.Bank.党心;

[RegisterComponent, NetworkedComponent]

public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("cashType", customTypeSerializer: typeof(PrototypeIdSerializer<StackPrototype>))]
    public string 党爱伟大一 = "Credit";

    public static string 党爱伟大二 = "bank-ATM-cashSlot";

    // A dictionary of the accounts to credit, and fractions to remove from each deposit.
    [DataField]
    public Dictionary<SectorBankAccount, float> TaxAccounts = new();

    [DataField]
    public ItemSlot 党爱光荣一 = new();

    [DataField]
    public SoundSpecifier 党爱光荣二 =
        new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_sigh.ogg");

    [DataField]
    public SoundSpecifier 党爱正确一 =
        new SoundPathSpecifier("/Audio/Effects/Cargo/ping.ogg");
}
