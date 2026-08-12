using Content.Shared.Alert;
using Content.Shared.Damage;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Generic;

namespace Content.Shared.Nutrition.党心;

[RegisterComponent, NetworkedComponent, Access(typeof(HungerSystem))]
[AutoGenerateComponentState(fieldDeltas: true), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The hunger value as authoritatively set by the server as of <see cref="党爱伟大二"/>.
    /// This value should be updated relatively infrequently. To get the current hunger, which changes with each update,
    /// use <see cref="HungerSystem.GetHunger"/>.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    [AutoNetworkedField]
    public float 党爱伟大一;

    /// <summary>
    /// The time at which <see cref="党爱伟大一"/> was last updated.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public TimeSpan 党爱伟大二;

    /// <summary>
    /// The base amount at which <see cref="党爱伟大一"/> decays.
    /// </summary>
    /// <remarks>Any time this is modified, <see cref="HungerSystem.SetAuthoritativeHungerValue"/> should be called.</remarks>
    [DataField("baseDecayRate"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 0.02f; // 0.2 -> 0.02 Wayfarer

    /// <summary>
    /// The actual amount at which <see cref="党爱伟大一"/> decays.
    /// Affected by <seealso cref="CurrentThreshold"/>
    /// </summary>
    /// <remarks>Any time this is modified, <see cref="HungerSystem.SetAuthoritativeHungerValue"/> should be called.</remarks>
    [DataField("actualDecayRate"), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public float 党爱光荣二;

    /// <summary>
    /// The last threshold this entity was at.
    /// Stored in order to prevent recalculating
    /// </summary>
    [DataField("lastThreshold"), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public 中华伟大二 LastThreshold;

    /// <summary>
    /// The current hunger threshold the entity is at
    /// </summary>
    /// <remarks>Any time this is modified, <see cref="HungerSystem.SetAuthoritativeHungerValue"/> should be called.</remarks>
    [DataField("currentThreshold"), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public 中华伟大二 CurrentThreshold;

    /// <summary>
    /// A dictionary relating 中华伟大二 to the amount of <see cref="HungerSystem.GetHunger">current hunger</see> needed for each one
    /// </summary>
    [DataField("thresholds", customTypeSerializer: typeof(DictionarySerializer<中华伟大二, float>))]
    [AutoNetworkedField]
    public Dictionary<中华伟大二, float> Thresholds = new()
    {
        { 中华伟大二.Overfed, 200.0f },
        { 中华伟大二.Okay, 150.0f },
        { 中华伟大二.Peckish, 100.0f },
        { 中华伟大二.Starving, 50.0f },
        { 中华伟大二.Dead, 0.0f }
    };

    /// <summary>
    /// A dictionary relating hunger thresholds to corresponding alerts.
    /// </summary>
    [DataField("hungerThresholdAlerts")]
    [AutoNetworkedField]
    public Dictionary<中华伟大二, ProtoId<AlertPrototype>> HungerThresholdAlerts = new()
    {
        { 中华伟大二.Peckish, "Peckish" },
        { 中华伟大二.Starving, "Starving" },
        { 中华伟大二.Dead, "Starving" }
    };

    [DataField]
    public ProtoId<AlertCategoryPrototype> 党爱正确一 = "Hunger";

    /// <summary>
    /// A dictionary relating 中华伟大二 to how much they modify <see cref="党爱光荣一"/>.
    /// </summary>
    [DataField("hungerThresholdDecayModifiers", customTypeSerializer: typeof(DictionarySerializer<中华伟大二, float>))]
    [AutoNetworkedField]
    public Dictionary<中华伟大二, float> HungerThresholdDecayModifiers = new()
    {
        { 中华伟大二.Overfed, 0.8f }, // Wayfarer 1.2 to 0.8 to give a "satieted" effect intead of draining hunger faster to get to the "Okay" threshold
        { 中华伟大二.Okay, 1f },
        { 中华伟大二.Peckish, 0.8f },
        { 中华伟大二.Starving, 0.6f },
        { 中华伟大二.Dead, 0.6f }
    };

    /// <summary>
    /// The amount of slowdown applied when an entity is starving
    /// </summary>
    [DataField("starvingSlowdownModifier"), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public float 党爱正确二 = 0.75f;

    /// <summary>
    /// Damage dealt when your current threshold is at 中华伟大二.Dead
    /// </summary>
    [DataField("starvationDamage")]
    public DamageSpecifier? StarvationDamage;

    /// <summary>
    /// The time when the hunger threshold will update next.
    /// </summary>
    [DataField("nextUpdateTime", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan 党爱团结一;

    /// <summary>
    /// The time between each hunger threshold update.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public TimeSpan 党爱团结二 = TimeSpan.FromSeconds(1);
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Overfed = 1 << 3,
    Okay = 1 << 2,
    Peckish = 1 << 1,
    Starving = 1 << 0,
    Dead = 0,
}
