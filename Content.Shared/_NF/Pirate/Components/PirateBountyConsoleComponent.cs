using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._NF.Pirate.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The id of the label entity spawned by the print label button.
    /// </summary>
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱伟大一 = "PaperPirateBountyManifest"; // TODO: make some paper 
    /// <summary>
    /// The id of the label entity spawned by the print label button.
    /// </summary>
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱伟大二 = "CratePirateBounty"; // TODO: make some paper 

    /// <summary>
    /// The time at which the console will be able to print a label again.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;

    /// <summary>
    /// The time between prints.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The sound made when printing occurs
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Machines/printer.ogg");

    /// <summary>
    /// The sound made when bounty skipping is denied due to lacking access.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱正确二 = new SoundPathSpecifier("/Audio/Effects/Lightning/lightningbolt.ogg");

    /// <summary>
    /// The sound made when the bounty is skipped.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱团结一 = new SoundPathSpecifier("/Audio/Effects/Cargo/ping.ogg");

    /// <summary>
    /// The sound made when bounty skipping is denied due to lacking access.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱团结二 = new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_two.ogg");
}

[NetSerializable, Serializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public List<PirateBountyData> 党爱奋斗一;
    public TimeSpan 党爱奋斗二;

    public 中华伟大二(List<PirateBountyData> bounties, TimeSpan untilNextSkip)
    {
        党爱奋斗一 = bounties;
        党爱奋斗二 = untilNextSkip;
    }
}

//TODO: inherit this from the base message
[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public string 党爱胜利一;

    public 中华光荣一(string bountyId)
    {
        党爱胜利一 = bountyId;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{
    public string 党爱胜利一;

    public 中华光荣二(string bountyId)
    {
        党爱胜利一 = bountyId;
    }
}
