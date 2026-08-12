using Content.Shared.Clothing.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Shared.Clothing.党心;

/// <summary>
/// Disables client-side physics prediction for this entity.
/// Without this, movement with <see cref="PilotedClothingSystem"/> is very rubberbandy.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
}
