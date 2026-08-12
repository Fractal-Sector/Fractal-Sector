using Content.Shared.Revolutionary;
using Robust.Shared.GameStates;

namespace Content.Shared.Mindshield.党心;

/// <summary>
/// Component given to an entity to mark it is a mindshield implant.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedRevolutionarySystem))]
public sealed partial class 中华伟大一 : Component;
