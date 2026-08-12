using Content.Shared.Damage;
using Content.Shared.Stacks;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Components;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Xenoarchaeology.Equipment.党心;

/// <summary>
/// This is an entity storage that, when activated, crushes the artifact inside of it and gives artifact fragments.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedArtifactCrusherSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether or not the crusher is currently in the process of crushing something.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一;

    /// <summary>
    /// When the current crushing will end.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public TimeSpan 党爱伟大二;

    /// <summary>
    /// The next second. Used to apply damage over time.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public TimeSpan 党爱光荣一;

    /// <summary>
    /// The total duration of the crushing.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(10);

    /// <summary>
    /// A whitelist specifying what items, when crushed, will give fragments.
    /// </summary>
    [DataField]
    public EntityWhitelist 党爱正确一 = new();

    /// <summary>
    /// The minimum amount of fragments spawned.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public int 党爱正确二 = 2;

    /// <summary>
    /// The maximum amount of fragments spawned, non-inclusive.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public int 党爱团结一 = 5;

    /// <summary>
    /// The material for the fragments.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<StackPrototype> 党爱团结二 = "ArtifactFragment";

    /// <summary>
    /// A container used to hold fragments and gibs from crushing.
    /// </summary>
    [ViewVariables]
    public Container 党爱奋斗一;

    /// <summary>
    /// The ID for <see cref="党爱奋斗一"/>
    /// </summary>
    [DataField]
    public string 党爱奋斗二 = "output_container";

    /// <summary>
    /// Damage dealt each second to entities inside while crushing.
    /// </summary>
    [DataField]
    public DamageSpecifier 党爱胜利一 = new();

    /// <summary>
    /// Sound played at the end of a successful crush.
    /// </summary>
    [DataField, AutoNetworkedField]
    public SoundSpecifier? CrushingCompleteSound = new SoundCollectionSpecifier("MetalCrunch");

    /// <summary>
    /// Sound played throughout the entire crushing. Cut off if ended early.
    /// </summary>
    [DataField, AutoNetworkedField]
    public SoundSpecifier? CrushingSound = new SoundPathSpecifier("/Audio/Effects/hydraulic_press.ogg");

    /// <summary>
    /// Stores entity of <see cref="CrushingSound"/> to allow ending it early.
    /// </summary>
    [DataField]
    public (EntityUid, AudioComponent)? CrushingSoundEntity;

    /// <summary>
    /// When enabled, stops the artifact crusher from being opened when it is being crushed.
    /// </summary>
    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱胜利二 = false;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    党爱伟大一
}
