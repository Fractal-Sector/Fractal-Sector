using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// Applies container changes whenever an entity is inserted into the specified container on this entity.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField(required: true)]
    public EntProtoId 党爱伟大一;

    [DataField(required: true)]
    public string 党爱伟大二 = string.Empty;
}
