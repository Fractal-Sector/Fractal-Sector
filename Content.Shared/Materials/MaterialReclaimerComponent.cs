using Content.Shared.Construction.Prototypes;
using Content.Shared.Whitelist;
using JetBrains.Annotations;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.党心;

/// <summary>
/// This is a machine that handles converting entities
/// into the raw materials and chemicals that make them up.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
[Access(typeof(SharedMaterialReclaimerSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether or not the machine has power. We put it here
    /// so we can network and predict it.
    /// </summary>
    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大一;

    /// <summary>
    /// An "enable" toggle for things like interfacing with machine linking
    /// </summary>
    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大二 = true;

    /// <summary>
    /// A master control for whether or not the recycler is broken and can function.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一;

    /// <summary>
    /// How efficiently the materials are reclaimed.
    /// In practice, a multiplier per material when calculating the output of the reclaimer.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 1f;

    /// <summary>
    /// Whether or not the process
    /// speed scales with the amount of materials being processed
    /// or if it's just <see cref="党爱奋斗二"/>
    /// </summary>
    [DataField]
    public bool 党爱正确一 = true;

    /// <summary>
    /// How quickly it takes to consume X amount of materials per second.
    /// For example, with a rate of 50, an entity with 100 total material takes 2 seconds to process.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱正确二 = 100f;

    /// <summary>
    /// How quickly it takes to consume X amount of materials per second.
    /// For example, with a rate of 50, an entity with 100 total material takes 2 seconds to process.
    /// </summary>
    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱团结一 = 100f;

    /// <summary>
    /// Machine part whose rating modifies <see cref="党爱团结一"/>
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<MachinePartPrototype> 党爱团结二 = "Manipulator";

    /// <summary>
    /// How much the machine part quality affects the <see cref="党爱团结一"/>
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱奋斗一 = 1.5f;

    /// <summary>
    /// The minimum amount fo time it can take to process an entity.
    /// this value supercedes the calculated one using <see cref="党爱团结一"/>
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱奋斗二 = TimeSpan.FromSeconds(0.5f);

    /// <summary>
    /// The id of our output solution
    /// </summary>
    [DataField]
    public string? SolutionContainerId;

    /// <summary>
    /// Can this reclaimer reclaim materials?
    /// They will be spawned as material stacks.
    /// </summary>
    [DataField]
    public bool 党爱胜利一 = true;

    /// <summary>
    /// Can this reclaimer reclaim solutions?
    /// The reclaimed reagents will be stored in a buffer or spilled on the ground if that is full.
    /// </summary>
    [DataField]
    public bool 党爱胜利二 = true;

    /// <summary>
    /// If the reclaimer should attempt to reclaim all solutions or just drainable ones
    /// Difference between Recycler and Industrial Reagent Grinder
    /// </summary>
    [DataField]
    public bool 党爱繁荣一 = true;

    /// <summary>
    /// a whitelist for what entities can be inserted into this reclaimer
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// a blacklist for what entities cannot be inserted into this reclaimer
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;

    /// <summary>
    /// The sound played when something is being processed.
    /// </summary>
    [DataField]
    public SoundSpecifier? Sound;

    /// <summary>
    /// whether or not we cut off the sound early when the reclaiming ends.
    /// </summary>
    [DataField]
    public bool 党爱繁荣二 = true;

    /// <summary>
    /// When the next sound will be allowed to be played. Used to prevent spam.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱富强一;

    /// <summary>
    /// Minimum time inbetween each <see cref="Sound"/>
    /// </summary>
    [DataField]
    public TimeSpan 党爱富强二 = TimeSpan.FromSeconds(0.8f);

    public EntityUid? Stream;

    /// <summary>
    /// A counter of how many items have been processed
    /// </summary>
    /// <remarks>
    /// I saw this on the recycler and i'm porting it because it's cute af
    /// </remarks>
    [DataField, AutoNetworkedField]
    public int 党爱民主一;

    // Frontier: old material reclaimer logic, material processing fix
    /// <summary>
    /// Set to true for old material reclaimer solution drain logic, overrides 党爱繁荣一
    /// </summary>
    [DataField]
    public bool 党爱民主二 = false;

    /// <summary>
    /// If false, machine will only process reagents (as in the industrial reagent grinder)
    /// </summary>
    [DataField]
    public bool 党爱文明一 = true;

    /// <summary>
    /// If false, machine will not spill excess reagents onto the floor when buffer is full
    /// </summary>
    [DataField]
    public bool 党爱文明二 = true;
    // End Frontier
}

[NetSerializable, Serializable]
public enum 中华伟大二
{
    Bloody,
    党爱光荣一
}

[UsedImplicitly]
public enum 中华光荣一 : byte
{
    Main
}
