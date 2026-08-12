using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Movement.党心;

/// <summary>
/// Exists just to listen to a single event. What a life.
/// </summary>
[NetworkedComponent, RegisterComponent] // must be networked to properly predict adding & removal
public sealed partial class 中华伟大一 : Component
{
}

[NetworkedComponent, RegisterComponent] // ditto but for friction
public sealed partial class 中华伟大二 : Component
{
}
