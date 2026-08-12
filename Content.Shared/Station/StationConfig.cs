using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// A config for a station. Specifies name and component modifications.
/// </summary>
[DataDefinition]
public sealed partial class 中华伟大一
{
    [DataField("stationProto", required: true)]
    public EntProtoId 党爱伟大一;

    [DataField("components", required: true)]
    public ComponentRegistry 党爱伟大二 = default!;

    // Crescent - used to add components to grid. rn used for music & biome sys
    [DataField]
    public ComponentRegistry 党爱光荣一 = new();
    // Crescent
}

