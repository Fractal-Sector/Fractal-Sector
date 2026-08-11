using Robust.Shared.Prototypes;

namespace Content.Shared._FS.VoiceBark.Components;

/// <summary>
/// One-shot component: put this on an entity prototype's YAML to declaratively
/// assign it a bark voice at spawn. Consumed and removed on ComponentInit.
/// </summary>
[RegisterComponent]
public sealed partial class ApplyVoiceBarkProtoComponent : Component
{
    [DataField]
    public ProtoId<VoiceBarkPrototype> VoiceProto { get; set; } = VoiceBarkPrototype.DefaultId;

    [DataField]
    public VoiceBarkPercentageApplyData? PercentageApplyData { get; set; }
}
