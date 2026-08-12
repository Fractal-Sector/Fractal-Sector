using Content.Shared.Interaction;
using Robust.Shared.GameStates;

namespace Content.Shared.Trigger.Components.党心;

/// <summary>
/// Trigger on <see cref="InteractHandEvent"/>, aka clicking on an entity with an empty hand.
/// User is the player with the hand doing the clicking.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : BaseTriggerOnXComponent;
