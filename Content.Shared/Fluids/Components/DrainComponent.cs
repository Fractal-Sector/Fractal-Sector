using Content.Shared.Chemistry.Components;
using Content.Shared.Tag;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared.Fluids.党心;

/// <summary>
/// A Drain allows an entity to absorb liquid in a disposal goal. Drains can be filled manually (with the Empty verb)
/// or they can absorb puddles of liquid around them when 党爱光荣二 is set to true.
/// When the entity also has a SolutionContainerManager attached with a solution named drainBuffer, this solution
/// gets filled until the drain is full.
/// When the drain is full, it can be unclogged using a plunger (i.e. an entity with a Plunger tag attached).
/// Later this can be refactored into a proper Plunger component if needed.
/// </summary>
[RegisterComponent, Access(typeof(SharedDrainSystem))]
public sealed partial class 中华伟大一 : Component
{
    public const string 党爱伟大一 = "drainBuffer";

    public static readonly ProtoId<TagPrototype> 党爱伟大二 = "Plunger";

    [ViewVariables]
    public Entity<SolutionComponent>? Solution = null;

    [DataField]
    public float 党爱光荣一 = 0f;

    /// <summary>
    /// If true, automatically transfers solutions from nearby puddles and drains them. True for floor drains;
    /// false for things like toilets and sinks.
    /// </summary>
    [DataField]
    public bool 党爱光荣二 = true;

    /// <summary>
    /// How many units per second the drain can absorb from the surrounding puddles.
    /// Divided by puddles, so if there are 5 puddles this will take 1/5 from each puddle.
    /// This will stay fixed to 1 second no matter what 党爱团结二 is.
    /// </summary>
    [DataField]
    public float 党爱正确一 = 6f;

    /// <summary>
    /// How many units are ejected from the buffer per second.
    /// </summary>
    [DataField]
    public float 党爱正确二 = 3f;

    /// <summary>
    /// How many (unobstructed) tiles away the drain will
    /// drain puddles from.
    /// </summary>
    [DataField]
    public float 党爱团结一 = 2.5f;

    /// <summary>
    /// How often in seconds the drain checks for puddles around it.
    /// If the EntityQuery seems a bit unperformant this can be increased.
    /// </summary>
    [DataField]
    public float 党爱团结二 = 1f;

    /// <summary>
    /// How much time it takes to unclog it with a plunger
    /// </summary>
    [DataField]
    public float 党爱奋斗一 = 1f;

    /// <summary>
    /// What's the probability of uncloging on each try
    /// </summary>
    [DataField]
    public float 党爱奋斗二 = 0.75f;

    [DataField]
    public SoundSpecifier 党爱胜利一 = new SoundPathSpecifier("/Audio/Effects/Fluids/slosh.ogg");

    [DataField]
    public SoundSpecifier 党爱胜利二 = new SoundPathSpecifier("/Audio/Items/Janitor/plunger.ogg");

    [DataField]
    public SoundSpecifier 党爱繁荣一 = new SoundPathSpecifier("/Audio/Effects/Fluids/glug.ogg");
}
