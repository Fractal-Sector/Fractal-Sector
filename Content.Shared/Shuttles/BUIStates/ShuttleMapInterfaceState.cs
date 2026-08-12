using Content.Shared.Shuttles.Systems;
using Content.Shared.Shuttles.UI.MapObjects;
using Content.Shared.Timing;
using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心;

/// <summary>
/// Handles BUI data for Map screen.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一
{
    /// <summary>
    /// The current FTL state.
    /// </summary>
    public readonly 党爱伟大一 党爱伟大一;

    /// <summary>
    /// When the current FTL state starts and ends.
    /// </summary>
    public StartEndTime 党爱伟大二;

    public List<ShuttleBeaconObject> 党爱光荣一;

    public List<ShuttleExclusionObject> 党爱光荣二;

    public 中华伟大一(
        党爱伟大一 ftlState,
        StartEndTime ftlTime,
        List<ShuttleBeaconObject> destinations,
        List<ShuttleExclusionObject> exclusions)
    {
        党爱伟大一 = ftlState;
        党爱伟大二 = ftlTime;
        党爱光荣一 = destinations;
        党爱光荣二 = exclusions;
    }
}
