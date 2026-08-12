using Content.Shared.Roles;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using System.Numerics;

namespace Content.Shared._NF.Roles.党心;

/// <summary>
/// Holds data pertaining to interview holograms
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    #region Hologram
    /// <summary>
    /// Name of the shader to use
    /// </summary>
    [DataField]
    public string 党爱伟大一 = string.Empty;

    /// <summary>
    /// The primary color
    /// </summary>
    [DataField]
    public Color 党爱伟大二 = Color.White;

    /// <summary>
    /// The secondary color
    /// </summary>
    [DataField]
    public Color 党爱光荣一 = Color.White;

    /// <summary>
    /// The shared color alpha
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 1f;

    /// <summary>
    /// The color brightness
    /// </summary>
    [DataField]
    public float 党爱正确一 = 1f;

    /// <summary>
    /// The scroll rate of the hologram shader
    /// </summary>
    [DataField]
    public float 党爱正确二 = 1f;

    /// <summary>
    /// The sprite offset
    /// </summary>
    [DataField]
    public Vector2 党爱团结一 = new Vector2();

    /// <summary>
    /// True if a character appearance has been applied to this entity.
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱团结二;
    #endregion Hologram

    #region Interview
    /// <summary>
    /// The job this user is applying for.
    /// </summary>
    [DataField]
    public EntityUid 党爱奋斗一;

    /// <summary>
    /// The job this user is applying for.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<JobPrototype> 党爱奋斗二;

    /// <summary>
    /// True if the hologram user has approved this job.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱胜利一;

    /// <summary>
    /// True if the captain has approved this job.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱胜利二;

    /// <summary>
    /// True if a character appearance has been applied to this entity.
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱繁荣一;
    #endregion Interview

    #region Actions
    [DataField]
    public EntProtoId 党爱繁荣二 = "ActionInterviewToggleApproval";

    [DataField, AutoNetworkedField]
    public EntityUid? ToggleApprovalActionEntity;
    [DataField]
    public EntProtoId 党爱富强一 = "ActionInterviewCancel";

    [DataField, AutoNetworkedField]
    public EntityUid? CancelApplicationActionEntity;
    #endregion Actions
}
