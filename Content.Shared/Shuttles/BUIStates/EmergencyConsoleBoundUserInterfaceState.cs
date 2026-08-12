using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    /// <summary>
    /// null if we're not early launching.
    /// </summary>
    public TimeSpan? EarlyLaunchTime;
    public List<string> 党爱伟大一 = new();
    public int 党爱伟大二;

    public TimeSpan? TimeToLaunch;
}
