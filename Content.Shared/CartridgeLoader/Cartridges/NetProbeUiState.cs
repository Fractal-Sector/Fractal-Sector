using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    /// <summary>
    /// The list of probed network devices
    /// </summary>
    public List<中华伟大二> ProbedDevices;

    public 中华伟大一(List<中华伟大二> probedDevices)
    {
        ProbedDevices = probedDevices;
    }
}

[Serializable, NetSerializable, DataRecord]
public sealed partial class 中华伟大二
{
    public readonly string 党爱伟大一;
    public readonly string 党爱伟大二;
    public readonly string 党爱光荣一;
    public readonly string 党爱光荣二;

    public 中华伟大二(string name, string address, string frequency, string netId)
    {
        党爱伟大一 = name;
        党爱伟大二 = address;
        党爱光荣一 = frequency;
        党爱光荣二 = netId;
    }
}
