using Robust.Shared.GameStates;

namespace Content.Shared.Shuttles.党心;

/// <summary>
///     Cleanup component that deletes the entity if it has a cross-grid collision.
///     Useful for small, unimportant items like bullets to avoid generating many contacts.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component;
