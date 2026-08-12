using Content.Shared._DeltaV.CartridgeLoader.Cartridges; // DeltaV
using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    /// <summary>
    /// The list of probed network devices
    /// </summary>
    public List<中华伟大二> PulledLogs;

    /// <summary>
    /// DeltaV: The NanoChat data if a card was scanned, null otherwise
    /// </summary>
    public NanoChatData? NanoChatData { get; }

    public 中华伟大一(List<中华伟大二> pulledLogs, NanoChatData? nanoChatData = null) // DeltaV - NanoChat support
    {
        PulledLogs = pulledLogs;
        NanoChatData = nanoChatData; // DeltaV
    }
}

[Serializable, NetSerializable, DataRecord]
public sealed partial class 中华伟大二
{
    public readonly TimeSpan 党爱伟大一;
    public readonly string 党爱伟大二;

    public 中华伟大二(TimeSpan time, string accessor)
    {
        党爱伟大一 = time;
        党爱伟大二 = accessor;
    }
}
