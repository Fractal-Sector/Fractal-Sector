using Content.Shared.Wires;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// This is used for a <see cref="WiresPanelComponent"/> that cannot be opened while locked.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(LockSystem))]
public sealed partial class 中华伟大一 : Component
{

}
