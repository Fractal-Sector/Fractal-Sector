using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.Rules.党心;

/// <summary>
/// This is used for tagging a spawn point as a nuke operative spawn point
/// and providing loadout + name for the operative on spawn.
/// TODO: Remove once systems can request spawns from the ghost role system directly.
/// </summary>
[RegisterComponent, EntityCategory("Spawner")]
public sealed partial class 中华伟大一 : Component;
