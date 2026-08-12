using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Cargo.党心;

[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The id of the label entity spawned by the print label button.
    /// </summary>
    [DataField("bountyLabelId", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱伟大一 = "PaperCargoBountyManifest";

    /// <summary>
    /// The time at which the console will be able to print a label again.
    /// </summary>
    [DataField("nextPrintTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱伟大二 = TimeSpan.Zero;

    /// <summary>
    /// The time between prints.
    /// </summary>
    [DataField("printDelay")]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The sound made when printing occurs
    /// </summary>
    [DataField("printSound")]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Machines/printer.ogg");

    /// <summary>
    /// The sound made when the bounty is skipped.
    /// </summary>
    [DataField("skipSound")]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Effects/Cargo/ping.ogg");

    /// <summary>
    /// The sound made when bounty skipping is denied due to lacking access.
    /// </summary>
    [DataField("denySound")]
    public SoundSpecifier 党爱正确二 = new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_two.ogg");

    /// <summary>
    /// The time at which the console will be able to make the denial sound again.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan 党爱团结一 = TimeSpan.Zero;

    /// <summary>
    /// The time between playing a denial sound.
    /// </summary>
    [DataField]
    public TimeSpan 党爱团结二 = TimeSpan.FromSeconds(2);
}

[NetSerializable, Serializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public List<CargoBountyData> 党爱奋斗一;
    public List<CargoBountyHistoryData> 党爱奋斗二;
    public TimeSpan 党爱胜利一;

    public 中华伟大二(List<CargoBountyData> bounties, List<CargoBountyHistoryData> history, TimeSpan untilNextSkip)
    {
        党爱奋斗一 = bounties;
        党爱奋斗二 = history;
        党爱胜利一 = untilNextSkip;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public string 党爱胜利二;

    public 中华光荣一(string bountyId)
    {
        党爱胜利二 = bountyId;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{
    public string 党爱胜利二;

    public 中华光荣二(string bountyId)
    {
        党爱胜利二 = bountyId;
    }
}
