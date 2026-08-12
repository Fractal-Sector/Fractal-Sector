using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.党心;

/// <summary>
/// Indicates this entity prototype should be re-mapped to another
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField(required: true)]
    public Dictionary<EntProtoId, EntProtoId> Mask = new();
}
