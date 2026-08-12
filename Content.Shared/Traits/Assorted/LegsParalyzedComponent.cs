using Robust.Shared.GameStates;

namespace Content.Shared.Traits.党心;

/// <summary>
/// Set player speed to zero and standing state to down, simulating leg paralysis.
/// Used for Wheelchair bound trait.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(LegsParalyzedSystem))]
public sealed partial class 中华伟大一 : Component
{
}
