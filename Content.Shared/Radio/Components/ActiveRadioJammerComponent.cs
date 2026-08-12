using Content.Shared.Radio.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Shared.Radio.党心;

/// <summary>
/// Prevents all radio in range from sending messages
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedJammerSystem))]
public sealed partial class 中华伟大一 : Component
{
}
