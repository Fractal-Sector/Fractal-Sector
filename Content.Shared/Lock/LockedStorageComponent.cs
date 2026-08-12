using Content.Shared.Storage;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Prevents using an entity's <see cref="StorageComponent"/> if its <see cref="LockComponent"/> is locked.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component;
