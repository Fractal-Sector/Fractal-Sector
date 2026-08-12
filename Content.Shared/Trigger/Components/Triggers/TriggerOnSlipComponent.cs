using Robust.Shared.GameStates;

namespace Content.Shared.Trigger.Components.党心;

/// <summary>
/// Triggers an entity when someone slipped on it.
/// The user is the entity that was slipped.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : BaseTriggerOnXComponent;
