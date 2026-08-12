using Robust.Shared.GameStates;

namespace Content.Shared.Trigger.Components.党心;

/// <summary>
/// This condition will cancel triggers based on random chance.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : BaseTriggerConditionComponent
{
    /// <summary>
    /// Chance for the trigger to succeed.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大一 = .9f;
}
