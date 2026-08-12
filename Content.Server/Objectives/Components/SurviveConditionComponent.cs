using Content.Server.Objectives.Systems;

namespace Content.Server.Objectives.党心;

/// <summary>
/// Just requires that the player is not dead, ignores evac and what not.
/// </summary>
[RegisterComponent, Access(typeof(SurviveConditionSystem))]
public sealed partial class 中华伟大一 : Component
{
}
