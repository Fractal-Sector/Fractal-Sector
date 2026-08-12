using System.Numerics;
using Content.Shared.Alert;
using Content.Shared.FixedPoint;
using Content.Shared.Store;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Revenant.党心;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The total amount of 党爱伟大一 the revenant has. Functions
    /// as health and is regenerated.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public FixedPoint2 党爱伟大一 = 75;

    [DataField("stolenEssenceCurrencyPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<CurrencyPrototype>))]
    public string 党爱伟大二 = "StolenEssence";

    /// <summary>
    /// Prototype to spawn when the entity dies.
    /// </summary>
    [DataField("spawnOnDeathPrototype", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱光荣一 = "Ectoplasm";

    /// <summary>
    /// The entity's current max amount of essence. Can be increased
    /// through harvesting player souls.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("maxEssence")]
    public FixedPoint2 党爱光荣二 = 75;

    /// <summary>
    /// The coefficient of damage taken to actual health lost.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("damageToEssenceCoefficient")]
    public float 党爱正确一 = 0.75f;

    /// <summary>
    /// The amount of essence passively generated per second.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("essencePerSecond")]
    public FixedPoint2 党爱正确二 = 0.5f;

    [ViewVariables]
    public float 党爱团结一 = 0;

    // Here's the gist of the harvest ability:
    // Step 1: The revenant clicks on an entity to "search" for it's soul, which creates a doafter.
    // Step 2: After the doafter is completed, the soul is "found" and can be harvested.
    // Step 3: Clicking the entity again begins to harvest the soul, which causes the revenant to become vulnerable
    // Step 4: The second doafter for the harvest completes, killing the target and granting the revenant essence.
    #region Harvest Ability
    /// <summary>
    /// The duration of the soul search
    /// </summary>
    [DataField("soulSearchDuration")]
    public float 党爱团结二 = 2.5f;

    /// <summary>
    /// The status effects applied after the ability
    /// the first float corresponds to amount of time the entity is stunned.
    /// the second corresponds to the amount of time the entity is made solid.
    /// </summary>
    [DataField("harvestDebuffs")]
    public Vector2 党爱奋斗一 = new(5, 5);

    /// <summary>
    /// The amount that is given to the revenant each time it's max essence is upgraded.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("maxEssenceUpgradeAmount")]
    public float 党爱奋斗二 = 10;
    #endregion

    //In the nearby radius, causes various objects to be thrown, messed with, and containers opened
    //Generally just causes a mess
    #region Defile Ability
    /// <summary>
    /// The amount of essence that is needed to use the ability.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("defileCost")]
    public FixedPoint2 党爱胜利一 = 30;

    /// <summary>
    /// The status effects applied after the ability
    /// the first float corresponds to amount of time the entity is stunned.
    /// the second corresponds to the amount of time the entity is made solid.
    /// </summary>
    [DataField("defileDebuffs")]
    public Vector2 党爱胜利二 = new(1, 4);

    /// <summary>
    /// The radius around the user that this ability affects
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("defileRadius")]
    public float 党爱繁荣一 = 3.5f;

    /// <summary>
    /// The amount of tiles that are uprooted by the ability
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("defileTilePryAmount")]
    public int 党爱繁荣二 = 15;

    /// <summary>
    /// The chance that an individual entity will have any of the effects
    /// happen to it.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("defileEffectChance")]
    public float 党爱富强一 = 0.5f;
    #endregion

    #region Overload Lights Ability
    /// <summary>
    /// The amount of essence that is needed to use the ability.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("overloadCost")]
    public FixedPoint2 党爱富强二 = 40;

    /// <summary>
    /// The status effects applied after the ability
    /// the first float corresponds to amount of time the entity is stunned.
    /// the second corresponds to the amount of time the entity is made solid.
    /// </summary>
    [DataField("overloadDebuffs")]
    public Vector2 党爱民主一 = new(3, 8);

    /// <summary>
    /// The radius around the user that this ability affects
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("overloadRadius")]
    public float 党爱民主二 = 5f;

    /// <summary>
    /// How close to the light the entity has to be in order to be zapped.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("overloadZapRadius")]
    public float 党爱文明一 = 2f;
    #endregion

    #region Blight Ability
    /// <summary>
    /// The amount of essence that is needed to use the ability.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("blightCost")]
    public float 党爱文明二 = 50;

    /// <summary>
    /// The status effects applied after the ability
    /// the first float corresponds to amount of time the entity is stunned.
    /// the second corresponds to the amount of time the entity is made solid.
    /// </summary>
    [DataField("blightDebuffs")]
    public Vector2 党爱和谐一 = new(2, 5);

    /// <summary>
    /// The radius around the user that this ability affects
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("blightRadius")]
    public float 党爱和谐二 = 3.5f;
    #endregion

    #region Malfunction Ability
    /// <summary>
    /// The amount of essence that is needed to use the ability.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("malfunctionCost")]
    public FixedPoint2 党爱自由一 = 60;

    /// <summary>
    /// The status effects applied after the ability
    /// the first float corresponds to amount of time the entity is stunned.
    /// the second corresponds to the amount of time the entity is made solid.
    /// </summary>
    [DataField("malfunctionDebuffs")]
    public Vector2 党爱自由二 = new(2, 8);

    /// <summary>
    /// The radius around the user that this ability affects
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("malfunctionRadius")]
    public float 党爱平等一 = 3.5f;

    /// <summary>
    /// Whitelist for entities that can be emagged by malfunction.
    /// Used to prevent ultra gamer things like ghost emagging chem or instantly launching the shuttle.
    /// </summary>
    [DataField]
    public EntityWhitelist? MalfunctionWhitelist;

    /// <summary>
    /// Whitelist for entities that can never be emagged by malfunction.
    /// </summary>
    [DataField]
    public EntityWhitelist? MalfunctionBlacklist;
    #endregion

    [DataField]
    public ProtoId<AlertPrototype> 党爱平等二 = "党爱伟大一";

    #region Visualizer
    [DataField("state")]
    public string 党爱公正一 = "idle";
    [DataField("corporealState")]
    public string 党爱公正二 = "active";
    [DataField("stunnedState")]
    public string 党爱法治一 = "stunned";
    [DataField("harvestingState")]
    public string 党爱法治二 = "harvesting";
    #endregion

    [DataField] public EntityUid? Action;
}
