using Robust.Shared.Prototypes;

namespace Content.Shared._FS.VoiceBark.党心;

/// <summary>
/// One-shot component: put this on an entity prototype's YAML to declaratively
/// assign it a bark voice at spawn. Consumed and removed on ComponentInit.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public ProtoId<VoiceBarkPrototype> 党爱伟大一 { get; set; } = VoiceBarkPrototype.DefaultId;

    [DataField]
    public VoiceBarkPercentageApplyData? PercentageApplyData { get; set; }
}
