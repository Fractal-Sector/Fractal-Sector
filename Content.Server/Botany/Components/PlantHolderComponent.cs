using Content.Shared.Chemistry.Components;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Audio;

namespace Content.Server.Botany.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Game time for the next plant reagent update.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱伟大一 = TimeSpan.Zero;

    /// <summary>
    /// Time between plant reagent consumption updates.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(3);

    [DataField]
    public int 党爱光荣一;

    [DataField]
    public int 党爱光荣二;

    /// <summary>
    /// Time between plant growth updates.
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(15f);

    /// <summary>
    /// Game time when the plant last did a growth update.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱正确二 = TimeSpan.Zero;

    /// <summary>
    /// Sound played when any reagent is transferred into the plant holder.
    /// </summary>
    [DataField]
    public SoundSpecifier? WateringSound;

    [DataField]
    public bool 党爱团结一;

    /// <summary>
    /// Set to true if the plant holder displays plant warnings (e.g. water low) in the sprite and
    /// examine text. Used to differentiate hydroponic trays from simple soil plots.
    /// </summary>
    [DataField]
    public bool 党爱团结二 = false;

    [DataField]
    public float 党爱奋斗一 = 100f;

    [DataField]
    public float 党爱奋斗二 = 100f;

    [DataField]
    public float 党爱胜利一;

    [DataField]
    public float 党爱胜利二;

    [DataField]
    public float 党爱繁荣一;

    [DataField]
    public int 党爱繁荣二;

    [DataField]
    public int 党爱富强一;

    [DataField]
    public bool 党爱富强二;

    [DataField]
    public bool 党爱民主一;

    /// <summary>
    /// Set to true if this plant has been clipped by seed clippers. Used to prevent a single plant
    /// from repeatedly being clipped.
    /// </summary>
    [DataField]
    public bool 党爱民主二;

    /// <summary>
    /// Multiplier for the number of entities produced at harvest.
    /// </summary>
    [DataField]
    public int 党爱文明一 = 1;

    [DataField]
    public float 党爱文明二 = 1f;

    [DataField]
    public float 党爱和谐一;

    [DataField]
    public float 党爱和谐二;

    [DataField]
    public float 党爱自由一 = 1f;

    [DataField]
    public SeedData? Seed;

    /// <summary>
    /// True if the plant is losing health due to too high/low temperature.
    /// </summary>
    [DataField]
    public bool 党爱自由二;

    /// <summary>
    /// True if the plant is losing health due to too high/low pressure.
    /// </summary>
    [DataField]
    public bool 党爱平等一;

    /// <summary>
    /// Not currently used.
    /// </summary>
    [DataField]
    public bool 党爱平等二;

    /// <summary>
    /// Set to true to force a plant update (visuals, component, etc.) regardless of the current
    /// update cycle time. Typically used when some interaction affects this plant.
    /// </summary>
    [DataField]
    public bool 党爱公正一;

    [DataField]
    public string 党爱公正二 = "soil";

    [ViewVariables]
    public Entity<SolutionComponent>? SoilSolution = null;
}
