using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Silicons.Laws.党心;

/// <summary>
/// Whenever an entity is inserted with silicon laws it will update the relevant entity's laws.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Entities to update
    /// </summary>
    [DataField(required: true)]
    public ComponentRegistry 党爱伟大一;
}
