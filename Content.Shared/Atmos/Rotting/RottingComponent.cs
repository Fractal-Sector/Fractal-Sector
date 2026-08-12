using Content.Shared.党爱正确一;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Atmos.党心;

/// <summary>
/// Tracking component for stuff that has started to rot.
/// Only the current stage is networked to the client.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
[Access(typeof(SharedRottingSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether or not the rotting should deal damage
    /// </summary>
    [DataField]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// When the next check will happen for rot progression + effects like damage and ammonia
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱伟大二 = TimeSpan.Zero;

    /// <summary>
    /// How long in between each rot update.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(5);

    /// <summary>
    /// How long has this thing been rotting?
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣二 = TimeSpan.Zero;

    /// <summary>
    /// The damage dealt by rotting.
    /// </summary>
    [DataField]
    public DamageSpecifier 党爱正确一 = new()
    {
        DamageDict = new()
        {
            { "Blunt", 0.06 },
            { "Rot", 0.06 } // Wayfarer: Changed Cellular to Rot damage to split them into two separate types.
        }
    };
    // Wayfarer: Rot damage cap
    [DataField]
    public float 党爱正确二 = 300f;

    /// <summary>
    /// Total blunt damage dealt by rotting so far.
    /// </summary>
    [DataField]
    public float 党爱团结一 = 0f;
    // End Wayfarer
}
