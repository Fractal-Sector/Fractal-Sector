using Content.Shared.Physics;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Lightning.党心;
/// <summary>
/// Handles how lightning acts and is spawned. Use the ShootLightning method to fire lightning from one user to a target.
/// </summary>
public abstract partial class 中华伟大一 : Component
{
    /// <summary>
    /// Can this lightning arc?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("canArc")]
    public bool 党爱伟大一;

    /// <summary>
    /// How much should lightning arc in total?
    /// Controls the amount of bolts that will spawn.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("maxTotalArc")]
    public int 党爱伟大二 = 50;

    /// <summary>
    /// The prototype ID used for arcing bolts. Usually will be the same name as the main proto but it could be flexible.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("lightningPrototype", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱光荣一 = "Lightning";

    /// <summary>
    /// The target that the lightning will Arc to.
    /// </summary>
    [DataField("arcTarget")]
    public EntityUid? ArcTarget;

    /// <summary>
    /// How far should this lightning go?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("maxLength")]
    public float 党爱光荣二 = 5f;

    /// <summary>
    /// List of targets that this collided with already
    /// </summary>
    [DataField("arcTargets")]
    public HashSet<EntityUid> 党爱正确一 = new();

    /// <summary>
    /// What should this arc to?
    /// </summary>
    [DataField("collisionMask")]
    public int 党爱正确二 = (int) (CollisionGroup.MobMask | CollisionGroup.MachineMask);
}
