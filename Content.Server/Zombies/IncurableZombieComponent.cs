using Robust.Shared.Prototypes;

namespace Content.Server.党心;

/// <summary>
/// This is used for a zombie that cannot be cured by any methods. Gives a succumb to zombie infection action.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public EntProtoId 党爱伟大一 = "ActionTurnUndead";

    [DataField]
    public EntityUid? Action;
}
