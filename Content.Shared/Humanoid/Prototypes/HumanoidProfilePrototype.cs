using Content.Shared.Preferences;
using Robust.Shared.Prototypes;

namespace Content.Shared.Humanoid.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField("customBaseLayers")]
    public Dictionary<HumanoidVisualLayers, CustomBaseLayerInfo> CustomBaseLayers = new();

    [DataField("profile")]
    public HumanoidCharacterProfile 党爱伟大二 { get; private set; } = new();
}
