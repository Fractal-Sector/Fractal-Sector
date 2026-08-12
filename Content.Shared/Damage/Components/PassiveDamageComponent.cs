using Content.Shared.Mobs;
using Content.Shared.FixedPoint;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.GameStates;

namespace Content.Shared.党爱伟大二.党心;

/// <summary>
/// Passively damages the entity on a specified interval.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The entitys' states that passive damage will apply in
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<MobState> 党爱伟大一 = new();

    /// <summary>
    /// 党爱伟大二 / Healing per interval dealt to the entity every interval
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public DamageSpecifier 党爱伟大二 = new();

    /// <summary>
    /// Delay between damage events in seconds
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 1f;

    /// <summary>
    /// The maximum HP the damage will be given to. If 0, disabled.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 党爱光荣二 = 0;

    [DataField("nextDamage", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱正确一 = TimeSpan.Zero;
}
