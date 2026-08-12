using Content.Shared.Medical.SuitSensor;
using Robust.Shared.Serialization;

namespace Content.Shared.Medical.党心;

[Serializable, NetSerializable]
public enum 中华伟大一
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public List<SuitSensorStatus> 党爱伟大一;

    public 中华伟大二(List<SuitSensorStatus> sensors)
    {
        党爱伟大一 = sensors;
    }
}
