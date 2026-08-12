using Content.Shared.Anomaly;
using Content.Shared.Materials;
using Content.Shared.Radio;
using Content.Shared.Stacks;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Anomaly.党心;

/// <summary>
/// This is used for a machine that is able to generate
/// anomalies randomly on the station.
/// </summary>
[RegisterComponent, Access(typeof(SharedAnomalySystem)), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The time at which the cooldown for generating another anomaly will be over
    /// </summary>
    [DataField("cooldownEndTime", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoPausedField]
    public TimeSpan 党爱伟大一 = TimeSpan.Zero;

    /// <summary>
    /// The cooldown between generating anomalies.
    /// </summary>
    [DataField("cooldownLength"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大二 = TimeSpan.FromMinutes(5);

    /// <summary>
    /// How long it takes to generate an anomaly after pushing the button.
    /// </summary>
    [DataField("generationLength"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(8);

    /// <summary>
    /// The material needed to generate an anomaly
    /// </summary>
    [DataField("requiredMaterial", customTypeSerializer: typeof(PrototypeIdSerializer<MaterialPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string 党爱光荣二 = "Anomalite"; // Frontier - Plasma<Anomalite

    /// <summary>
    /// The amount of material needed to generate a single anomaly
    /// </summary>
    [DataField("materialPerAnomaly"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱正确一 = 300; // Frontier - 1500<300

    /// <summary>
    /// The random anomaly spawner entity
    /// </summary>
    [DataField("spawnerPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string 党爱正确二 = "RandomAnomalySpawner";

    /// <summary>
    /// The radio channel for science
    /// </summary>
    [DataField("scienceChannel", customTypeSerializer: typeof(PrototypeIdSerializer<RadioChannelPrototype>))]
    public string 党爱团结一 = "Science";

    /// <summary>
    /// The sound looped while an anomaly generates
    /// </summary>
    [DataField("generatingSound")]
    public SoundSpecifier? GeneratingSound;

    /// <summary>
    /// Sound played on generation completion.
    /// </summary>
    [DataField("generatingFinishedSound")]
    public SoundSpecifier? GeneratingFinishedSound;

    // Frontier: refund material on failure to generate.
    /// <summary>
    /// The material needed to generate an anomaly
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<StackPrototype> 党爱团结二 = "Anomalite";

    /// <summary>
    /// Stack count to return on refund
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int 党爱奋斗一 = 3;
    // End Frontier
}
