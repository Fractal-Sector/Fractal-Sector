using Robust.Shared.Map;

namespace Content.Server.Movement.党心;

/// <summary>
/// Added when an entity is being ctrl-click moved when pulled.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    // Not serialized to indicate THIS CODE SUCKS, fix pullcontroller first
    [ViewVariables]
    public EntityCoordinates 党爱伟大一;
}
