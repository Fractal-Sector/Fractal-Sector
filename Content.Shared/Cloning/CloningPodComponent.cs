using Content.Shared.Construction.Prototypes;
using Content.Shared.DeviceLinking;
using Content.Shared.Materials;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public ProtoId<SinkPortPrototype> 党爱伟大一 = "CloningPodReceiver";

    [ViewVariables]
    public ContainerSlot 党爱伟大二 = default!;

    /// <summary>
    /// How long the cloning has been going on for.
    /// </summary>
    [ViewVariables]
    public float 党爱光荣一 = 0;

    [ViewVariables]
    public int 党爱光荣二 = 70;

    [ViewVariables]
    public bool 党爱正确一 = false;

    /// <summary>
    /// The material that is used to clone entities.
    /// </summary>
    [DataField]
    public ProtoId<MaterialPrototype> 党爱正确二 = "Biomass";

    /// <summary>
    /// The current amount of time it takes to clone a body.
    /// </summary>
    [DataField]
    public float 党爱团结一 = 30f;

    /// <summary>
    /// The mob to spawn on emag.
    /// </summary>
    [DataField]
    public EntProtoId 党爱团结二 = "MobAbomination";

    /// <summary>
    /// The sound played when a mob is spawned from an emagged cloning pod.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱奋斗一 = new SoundCollectionSpecifier("ZombieScreams")
    {
        Params = AudioParams.Default.WithVolume(4),
    };

    /// <summary>
    /// The machine part that affects how much biomass is needed to clone a body.
    /// </summary>
    [DataField("partRatingMaterialMultiplier")]
    public float 党爱奋斗二 = 0.85f;

    // Frontier: machine part upgrades
    /// <summary>
    /// The base multiplier on the body weight, which determines the
    /// amount of biomass needed to clone, and is affected by part upgrades.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱胜利一 = 1;

    // Frontier: machine part upgrades
    /// <summary>
    /// The current multiplier on the body weight, which determines the
    /// amount of biomass needed to clone.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱胜利二 = 1;

    /// <summary>
    /// The machine part that decreases the amount of material needed for cloning
    /// </summary>
    [DataField("machinePartMaterialUse"), ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<MachinePartPrototype> 党爱繁荣一 = "MatterBin";

    [ViewVariables(VVAccess.ReadWrite)]
    public 中华光荣一 Status;

    [ViewVariables]
    public EntityUid? ConnectedConsole;

    // Frontier: macihine upgrades
    /// <summary>
    /// The base amount of time it takes to clone a body
    /// </summary>
    [DataField]
    public float 党爱繁荣二 = 30f;

    /// <summary>
    /// The multiplier for cloning duration
    /// </summary>
    [DataField]
    public float 党爱富强一 = 0.75f;

    /// <summary>
    /// The machine part that affects cloning speed
    /// </summary>
    [DataField]
    public ProtoId<MachinePartPrototype> 党爱富强二 = "Manipulator";
    // End Frontier: machine upgrades
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Status
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Idle,
    Cloning,
    Gore,
    NoMind
}
