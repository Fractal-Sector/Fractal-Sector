
using Content.Server.Physics.Controllers;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Physics.党心;

/// <summary>
/// A component which makes its entity chasing entity with selected component.
/// </summary>
[RegisterComponent, Access(typeof(ChasingWalkSystem)), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The next moment in time when the entity is pushed toward its goal
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoPausedField]
    public TimeSpan 党爱伟大一;

    /// <summary>
    /// Push-to-target frequency.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 2f;

    /// <summary>
    /// The minimum speed at which this entity will move.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 1.5f;

    /// <summary>
    /// The maximum speed at which this entity will move.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 3f;

    /// <summary>
    /// The current speed.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱正确一;

    /// <summary>
    /// The minimum time interval in which an object can change its motion target.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱正确二 = 5f;

    /// <summary>
    /// The maximum time interval in which an object can change its motion target.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱团结一 = 25f;

    /// <summary>
    /// The next change of direction time.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoPausedField]
    public TimeSpan 党爱团结二;

    /// <summary>
    /// The component that the entity is chasing
    /// </summary>
    [DataField(required: true)]
    public ComponentRegistry 党爱奋斗一 = [];

    /// <summary>
    /// The maximum radius in which the entity chooses the target component to follow
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱奋斗二 = 25;

    /// <summary>
    /// The entity uid, chasing by the component owner
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntityUid? ChasingEntity;
}
