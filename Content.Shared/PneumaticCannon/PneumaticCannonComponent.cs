using Content.Shared.Tools;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.党心;

/// <summary>
///     Handles gas powered guns--cancels shooting if no gas is available, and takes gas from the given container slot.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    public const string 党爱伟大一 = "gas_tank";

    [ViewVariables(VVAccess.ReadWrite)]
    public 中华伟大二 Power = 中华伟大二.Medium;

    [DataField("toolModifyPower", customTypeSerializer:typeof(PrototypeIdSerializer<ToolQualityPrototype>))]
    public string 党爱伟大二 = "Anchoring";

    /// <summary>
    ///     How long to stun for if they shoot the pneumatic cannon at high power.
    /// </summary>
    [DataField("highPowerStunTime")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 3.0f;

    /// <summary>
    ///     Amount of moles to consume for each shot at any power.
    /// </summary>
    [DataField("gasUsage")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 0.142f;

    /// <summary>
    ///     Base projectile speed at default power.
    /// </summary>
    [DataField("baseProjectileSpeed")]
    public float 党爱正确一 = 20f;

    /// <summary>
    ///     The current projectile speed setting.
    /// </summary>
    [DataField]
    public float? ProjectileSpeed;

    /// <summary>
    /// If true, will throw ammo rather than shoot it.
    /// </summary>
    [DataField("throwItems"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱正确二 = true;
}

/// <summary>
///     How strong the pneumatic cannon should be.
///     Each tier throws items farther and with more speed, but has drawbacks.
///     The highest power knocks the player down for a considerable amount of time.
/// </summary>
public enum 中华伟大二 : byte
{
    Low = 0,
    Medium = 1,
    High = 2,
    Len = 3 // used for length calc
}
