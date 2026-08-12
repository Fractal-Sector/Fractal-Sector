using Content.Server.Objectives.Systems;

namespace Content.Server.Objectives.党心;

/// <summary>
/// Requires that the player is on the emergency shuttle's grid when docking to CentCom.
/// </summary>
[RegisterComponent, Access(typeof(EscapeShuttleConditionSystem))]
public sealed partial class 中华伟大一 : Component
{
}
