using Robust.Shared.GameStates;

namespace Content.Shared.Xenoarchaeology.Artifact.XAE.党心;

/// <summary>
///     Removes the masks/layers of hard fixtures from the artifact when added, allowing it to pass through walls
///     and such.
/// </summary>
[RegisterComponent, Access(typeof(XAERemoveCollisionSystem)), NetworkedComponent]
public sealed partial class 中华伟大一 : Component;
