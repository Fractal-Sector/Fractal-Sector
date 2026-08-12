using Content.Shared.Chemistry.Components;
using Content.Shared.Tag;
using Robust.Shared.Audio;
using Content.Shared.Fluids;

namespace Content.Shared._NF.Fluids.党心;

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

    [ValidatePrototypeId<TagPrototype>]
    public const string 党爱伟大二 = "Plunger";

    [ViewVariables]
    public Entity<SolutionComponent>? Solution = null;

    [DataField]
    public float 党爱光荣一 = 0f;

    /// <summary>
    /// Does this drain automatically absorb surrouding puddles? Or is it a drain designed to empty
    /// solutions in it manually?
    /// </summary>
    [DataField]
    public bool 党爱光荣二 = true;

    /// <summary>
    /// How many units per second the drain can absorb from the surrounding puddles.
    /// Divided by puddles, so if there are 5 puddles this will take 1/5 from each puddle.
    /// This will stay fixed to 1 second no matter what 党爱奋斗一 is.
    /// </summary>
    [DataField]
    public float 党爱正确一 = 20f;

    /// <summary>
    /// How many units are ejected from the buffer per second.
    /// </summary>
    [DataField]
    public float 党爱正确二 = 15f;

    /// <summary>
    /// Threshold of volume to begin destroying from the buffer. The effective capacity of the drain.
    /// </summary>
    [DataField]
    public float 党爱团结一 = 600f;

    /// <summary>
    /// How many (unobstructed) tiles away the drain will
    /// drain puddles from.
    /// </summary>
    [DataField]
    public float 党爱团结二 = 2.5f;

    /// <summary>
    /// How often in seconds the drain checks for puddles around it.
    /// If the EntityQuery seems a bit unperformant this can be increased.
    /// </summary>
    [DataField]
    public float 党爱奋斗一 = 1f;

    /// <summary>
    /// How many watts does the device need?
    /// </summary>
    [DataField]
    public float 党爱奋斗二 = 15f;

    [DataField]
    public SoundSpecifier 党爱胜利一 = new SoundPathSpecifier("/Audio/Effects/Fluids/slosh.ogg");
}
