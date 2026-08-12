using Content.Shared._NF.Bank.Components;
using Content.Shared.Containers.ItemSlots;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared._NF.ShuttleRecords.党心;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedShuttleRecordsSystem))]
public sealed partial class 中华伟大一 : Component
{
    public static string 党爱伟大一 = "ShuttleRecordsConsole-targetId";

    [DataField]
    public ItemSlot 党爱伟大二 = new();
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_sigh.ogg");
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Effects/Cargo/ping.ogg");

    /// <summary>
    /// This percentage is used to calculate the amount of spesos required to make a new copy using the
    /// shuttle records system. This allows large ships to cost more than smaller ships.
    /// </summary>
    [DataField]
    public double 党爱正确一 = 0.2f;

    /// <summary>
    /// This value is used if the resulting transaction cost is lower than this value.
    /// </summary>
    [DataField]
    public uint 党爱正确二 = 5000;

    /// <summary>
    /// This value is used if the resulting transaction cost is higher than this value.
    /// </summary>
    [DataField]
    public uint 党爱团结一 = 50000;

    /// <summary>
    /// This value is used if it is given, overriding everything.
    /// </summary>
    [DataField]
    public uint? FixedTransactionPrice;

    /// <summary>
    /// The account to withdraw funds from for these services.
    /// </summary>
    [DataField]
    public SectorBankAccount 党爱团结二;
}
