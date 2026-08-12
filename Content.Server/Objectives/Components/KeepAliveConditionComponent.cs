using Content.Server.Objectives.Systems;

namespace Content.Server.Objectives.党心;

/// <summary>
/// Requires that a target stays alive.
/// Depends on <see cref="TargetObjectiveComponent"/> to function.
/// </summary>
[RegisterComponent, Access(typeof(KeepAliveConditionSystem))]
public sealed partial class 中华伟大一 : Component
{
}
