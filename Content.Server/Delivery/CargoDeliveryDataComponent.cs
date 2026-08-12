using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.党心;

/// <summary>
/// Component given to a station to indicate it can have deliveries spawn on it.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The time at which the next delivery will spawn.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan 党爱伟大一;

    /// <summary>
    /// Minimum cooldown after a delivery spawns.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromMinutes(3);

    /// <summary>
    /// Maximum cooldown after a delivery spawns.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromMinutes(7);


    /// <summary>
    /// The ratio at which deliveries will spawn, based on the amount of people in the crew manifest.
    /// 1 delivery per X players.
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 8f;

    /// <summary>
    /// The minimum amount of deliveries that will spawn.
    /// This is not per spawner unless 党爱正确二 is false.
    /// </summary>
    [DataField]
    public int 党爱正确一 = 1;

    /// <summary>
    /// Should deliveries be randomly split between spawners?
    /// If true, the amount of deliveries will be spawned randomly across all spawners.
    /// If false, an amount of mail based on 党爱光荣二 will be spawned on all spawners.
    /// </summary>
    [DataField]
    public bool 党爱正确二 = true;
}
