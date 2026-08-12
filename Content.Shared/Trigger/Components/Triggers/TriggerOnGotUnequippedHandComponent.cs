using Robust.Shared.GameStates;

namespace Content.Shared.Trigger.Components.党心;

/// <summary>
/// Triggers an item when it is dropped from a hand slot.
/// The user is the entity that dropped the item.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : BaseTriggerOnXComponent;
