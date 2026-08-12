using Robust.Shared.GameStates;

namespace Content.Shared.Interaction.党心;

/// <summary>
/// This is used for entities which should not rotate on interactions (for instance those who use <see cref="MouseRotator"/> instead)
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
}
