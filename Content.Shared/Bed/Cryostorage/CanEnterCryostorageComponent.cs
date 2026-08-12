using Robust.Shared.GameStates;

namespace Content.Shared.Bed.党心;

/// <summary>
/// Serves as a whitelist that allows an entity with this component to enter cryostorage.
/// It will also require MindContainerComponent.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component { }
