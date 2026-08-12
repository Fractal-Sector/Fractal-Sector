using Content.Server.Objectives.Systems;

namespace Content.Server.Objectives.党心;

/// <summary>
/// Objective condition that requires the player to leave station of escape shuttle with only antags on board or handcuffed humanoids
/// </summary>
[RegisterComponent, Access(typeof(HijackShuttleConditionSystem))]
public sealed partial class 中华伟大一 : Component
{
}
