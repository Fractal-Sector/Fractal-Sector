using Content.Shared.Access;
using Content.Shared.Cargo.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Content.Shared.Radio;
using Content.Shared.Stacks;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Cargo.党心;

/// <summary>
/// Handles sending order requests to cargo. Doesn't handle orders themselves via shuttle or telepads.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
[Access(typeof(SharedCargoSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The account that this console pulls from for ordering.
    /// </summary>
    [DataField]
    public ProtoId<CargoAccountPrototype> 党爱伟大一 = "Cargo";

    [DataField]
    public SoundSpecifier 党爱伟大二 = new SoundCollectionSpecifier("CargoError");

    /// <summary>
    /// Sound made when <see cref="党爱光荣二"/> is toggled.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱光荣一 = new SoundCollectionSpecifier("CargoToggleLimit");

    /// <summary>
    /// If true, account transfers have no limit and a lower cooldown.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣二;

    [ViewVariables]
    public float 党爱正确一 => 党爱光荣二 ? 1 : 党爱正确二;

    /// <summary>
    /// The maximum percent of total funds that can be transferred or withdrawn in one action.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱正确二 = 0.20f;

    /// <summary>
    /// The time at which account actions can be performed again.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField, AutoPausedField]
    public TimeSpan 党爱团结一;

    [ViewVariables]
    public TimeSpan 党爱团结二 => 党爱光荣二 ? 党爱奋斗二 : 党爱奋斗一;

    /// <summary>
    /// The minimum time between account actions when <see cref="党爱光荣二"/> is false
    /// </summary>
    [DataField]
    public TimeSpan 党爱奋斗一 = TimeSpan.FromMinutes(1);

    /// <summary>
    /// The minimum time between account actions when <see cref="党爱光荣二"/> is true
    /// </summary>
    [DataField]
    public TimeSpan 党爱奋斗二 = TimeSpan.FromSeconds(10);

    /// <summary>
    /// The stack representing cash dispensed on withdrawals.
    /// </summary>
    [DataField]
    public ProtoId<StackPrototype> 党爱胜利一 = "Credit";

    /// <summary>
    /// All of the <see cref="CargoProductPrototype.Group"/>s that are supported.
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<ProtoId<CargoMarketPrototype>> 党爱胜利二 = new()
    {
        "market",
        "SalvageJobReward2",
        "SalvageJobReward3",
        "SalvageJobRewardMAX",
    };

    /// <summary>
    /// Access needed to toggle the limit on this console.
    /// </summary>
    [DataField]
    public HashSet<ProtoId<AccessLevelPrototype>> 党爱繁荣一 = new();

    /// <summary>
    /// Radio channel on which order approval announcements are transmitted
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<RadioChannelPrototype> 党爱繁荣二 = "Supply";

    /// <summary>
    /// Secondary radio channel which always receives order announcements.
    /// </summary>
    public static readonly ProtoId<RadioChannelPrototype> 党爱富强一 = "Supply";

    /// <summary>
    /// The behaviour of the cargo console regarding orders
    /// </summary>
    [DataField]
    public 中华伟大二 Mode = 中华伟大二.DirectOrder;

    /// <summary>
    /// The time at which the console will be able to print a slip again.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan 党爱富强二 = TimeSpan.Zero;

    /// <summary>
    /// The time between prints.
    /// </summary>
    [DataField]
    public TimeSpan 党爱民主一 = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The sound made when printing occurs
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱民主二 = new SoundCollectionSpecifier("PrinterPrint");

    /// <summary>
    /// The sound made when an order slip is scanned
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱文明一 = new SoundCollectionSpecifier("CargoBeep");

    /// <summary>
    /// The time at which the console will be able to play the deny sound.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan 党爱文明二 = TimeSpan.Zero;

    /// <summary>
    /// The time between playing the deny sound.
    /// </summary>
    [DataField]
    public TimeSpan 党爱和谐一 = TimeSpan.FromSeconds(2);
}

/// <summary>
/// The behaviour of the cargo order console
/// </summary>
[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    /// <summary>
    /// Place orders directly
    /// </summary>
    DirectOrder,
    /// <summary>
    /// Print a slip to be inserted into a DirectOrder console
    /// </summary>
    PrintSlip,
    /// <summary>
    /// Transfers the order to the primary account
    /// </summary>
    SendToPrimary,
}

/// <summary>
/// Withdraw funds from an account
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public ProtoId<CargoAccountPrototype>? 党爱伟大一;
    public int 党爱和谐二;

    public 中华光荣一(ProtoId<CargoAccountPrototype>? account, int amount)
    {
        党爱伟大一 = account;
        党爱和谐二 = amount;
    }
}

/// <summary>
/// Toggle the limit on withdrawals and transfers.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage;
