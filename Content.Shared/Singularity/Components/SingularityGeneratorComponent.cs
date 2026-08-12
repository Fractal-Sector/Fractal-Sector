using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

using Content.Shared.Physics;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.GameStates;

namespace Content.Shared.Singularity.党心;

[RegisterComponent, AutoGenerateComponentPause, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The amount of power this generator has accumulated.
    /// If you want to set this use <see  cref="SingularityGeneratorSystem.SetPower"/>
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 0;

    /// <summary>
    /// The power threshold at which this generator will spawn a singularity.
    /// If you want to set this use <see  cref="SingularityGeneratorSystem.SetThreshold"/>
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 16;

    /// <summary>
    /// Allows the generator to ignore all the failsafe stuff, e.g. when emagged
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一 = false;

    /// <summary>
    /// Maximum distance at which the generator will check for a field at
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 16;

    /// <summary>
    ///     The prototype ID used to spawn a singularity.
    /// </summary>
    [DataField("spawnId", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string? SpawnPrototype = "Singularity";

    /// <summary>
    /// The masks the raycast should not go through
    /// </summary>
    [DataField]
    public int 党爱正确一 = (int)CollisionGroup.FullTileMask;

    /// <summary>
    /// Message to use when there's no containment field on cardinal directions
    /// </summary>
    [DataField]
    public LocId 党爱正确二 = "comp-generator-failsafe";

    /// <summary>
    /// For how long the failsafe will cause the generator to stop working and not issue a failsafe warning
    /// </summary>
    [DataField]
    public TimeSpan 党爱团结一 = TimeSpan.FromSeconds(10);

    /// <summary>
    /// How long until the generator can issue a failsafe warning again
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱团结二 = TimeSpan.Zero;

    // Frontier: tether dangerous entities
    /// <summary>
    /// If true, generator must be in range of a tether to interact with particles.
    /// </summary>
    [DataField]
    public bool 党爱奋斗一 = true;
    // End Frontier: tether dangerous entities
}
