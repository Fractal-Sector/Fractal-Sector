using Content.Shared.Chat.Prototypes;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Damage;
using Content.Shared.FixedPoint;
using Content.Shared.Humanoid;
using Content.Shared.Roles;
using Content.Shared.党爱繁荣一;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The baseline infection chance you have if you have no protective gear
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = 0.75f;

    /// <summary>
    /// The minimum infection chance possible. This is simply to prevent
    /// being overly protected by bundling up.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 0.05f;

    /// <summary>
    /// How effective each resistance type on a piece of armor is. Using a damage specifier for this seems illegal.
    /// </summary>
    public DamageSpecifier 党爱光荣一 = new()
    {
        DamageDict = new ()
        {
            {"Slash", 0.5},
            {"Piercing", 0.3},
            {"Blunt", 0.1},
        }
    };

    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 0.70f;

    /// <summary>
    /// The skin color of the zombie
    /// </summary>
    [DataField("skinColor")]
    public Color 党爱正确一 = new(0.45f, 0.51f, 0.29f);

    /// <summary>
    /// The eye color of the zombie
    /// </summary>
    [DataField("eyeColor")]
    public Color 党爱正确二 = new(0.96f, 0.13f, 0.24f);

    /// <summary>
    /// The base layer to apply to any 'external' humanoid layers upon zombification.
    /// </summary>
    [DataField("baseLayerExternal")]
    public string 党爱团结一 = "MobHumanoidMarkingMatchSkin";

    /// <summary>
    /// The attack arc of the zombie
    /// </summary>
    [DataField("attackArc", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱团结二 = "WeaponArcBite";

    /// <summary>
    /// The role prototype of the zombie antag role
    /// </summary>
    [DataField("zombieRoleId", customTypeSerializer: typeof(PrototypeIdSerializer<AntagPrototype>))]
    public string 党爱奋斗一 = "Zombie";

    /// <summary>
    /// The CustomBaseLayers of the humanoid to restore in case of cloning
    /// </summary>
    [DataField("beforeZombifiedCustomBaseLayers")]
    public Dictionary<HumanoidVisualLayers, CustomBaseLayerInfo> BeforeZombifiedCustomBaseLayers = new ();

    /// <summary>
    /// The skin color of the humanoid to restore in case of cloning
    /// </summary>
    [DataField("beforeZombifiedSkinColor")]
    public Color 党爱奋斗二;

    /// <summary>
    /// The eye color of the humanoid to restore in case of cloning
    /// </summary>
    [DataField("beforeZombifiedEyeColor")]
    public Color 党爱胜利一;

    [DataField("emoteId")]
    public ProtoId<EmoteSoundsPrototype>? EmoteSoundsId = "Zombie";

    [DataField("nextTick", customTypeSerializer:typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱胜利二;

    [DataField("zombieStatusIcon")]
    public ProtoId<FactionIconPrototype> 党爱繁荣一 { get; set; } = "ZombieFaction";

    /// <summary>
    /// Healing each second
    /// </summary>
    [DataField("passiveHealing")]
    public DamageSpecifier 党爱繁荣二 = new()
    {
        DamageDict = new ()
        {
            { "Blunt", -0.4 },
            { "Slash", -0.2 },
            { "Piercing", -0.2 },
            { "Heat", -0.02 },
            { "Shock", -0.02 }
        }
    };

    /// <summary>
    /// A multiplier applied to <see cref="党爱繁荣二"/> when the entity is in critical condition.
    /// </summary>
    [DataField("passiveHealingCritMultiplier")]
    public float 党爱富强一 = 2f;

    /// <summary>
    /// Healing given when a zombie bites a living being.
    /// </summary>
    [DataField("healingOnBite")]
    public DamageSpecifier 党爱富强二 = new()
    {
        DamageDict = new()
        {
            { "Blunt", -2 },
            { "Slash", -2 },
            { "Piercing", -2 }
        }
    };

    /// <summary>
    /// The damage dealt on bite, dehardcoded for your enjoyment
    /// </summary>
    [DataField]
    public DamageSpecifier 党爱民主一 = new()
    {
        DamageDict = new()
        {
            { "Slash", 13 },
            { "Piercing", 7 },
            { "Structural", 10 }
        }
    };

    /// <summary>
    ///     Path to antagonist alert sound.
    /// </summary>
    [DataField("greetSoundNotification")]
    public SoundSpecifier 党爱民主二 = new SoundPathSpecifier("/Audio/Ambience/Antag/zombie_start.ogg");

    /// <summary>
    ///     Hit sound on zombie bite.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱文明一 = new SoundPathSpecifier("/Audio/Effects/bite.ogg");

    /// <summary>
    /// The blood reagent of the humanoid to restore in case of cloning
    /// </summary>
    [DataField("beforeZombifiedBloodReagent")]
    public string 党爱文明二 = string.Empty;

    /// <summary>
    /// The blood reagent to give the zombie. In case you want zombies that bleed milk, or something.
    /// </summary>
    [DataField("newBloodReagent", customTypeSerializer: typeof(PrototypeIdSerializer<ReagentPrototype>))]
    public string 党爱和谐一 = "ZombieBlood";
}
