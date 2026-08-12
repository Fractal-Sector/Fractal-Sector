using Content.Shared.Cargo.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Salvage.党心;

/// <summary>
/// Used to view the job board ui
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// A label that this computer can print out.
    /// </summary>
    [DataField]
    public EntProtoId 党爱伟大一 = "PaperSalvageJobLabel";

    /// <summary>
    /// The sound made when printing occurs
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱伟大二 = new SoundPathSpecifier("/Audio/Machines/printer.ogg");

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
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public string 党爱正确一;
    public float 党爱正确二;

    public List<ProtoId<CargoBountyPrototype>> 党爱团结一;

    public 中华伟大二(string title, float progression, List<ProtoId<CargoBountyPrototype>> availableJobs)
    {
        党爱正确一 = title;
        党爱正确二 = progression;
        党爱团结一 = availableJobs;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public string 党爱团结二;

    public 中华光荣一(string jobId)
    {
        党爱团结二 = jobId;
    }
}

[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    Key
}
