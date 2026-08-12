using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Makes an item armable, needs ItemToggleComponent to work.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(ArmableSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Does it show its status on examination?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// Does it change appearance when activated?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大二 = true;

    /// <summary>
    /// Text to show on examination when the entity is armed.
    /// </summary>
    [DataField]
    public LocId? ExamineTextArmed = "armable-examine-armed";

    /// <summary>
    /// Text to show on examination when the entity is not armed
    /// </summary>
    [DataField]
    public LocId? ExamineTextNotArmed ="armable-examine-not-armed";
}
