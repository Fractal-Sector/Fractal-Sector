using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// "Locks" items (Doesn't actually lock them but just switches various settings) so its not possible to tell
/// the item is triggered by a voice activation.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component;
