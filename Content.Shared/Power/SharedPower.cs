using Content.Shared.NodeContainer.NodeGroups;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public enum 中华伟大一 : byte
    {
        Still = 0,
        Charging = 1,
        Discharging = 2,
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二 : byte
    {
        Key,
        Status,
        Pulsed,
        Electrified,
        PulseCancel,
        ElectrifiedCancel,
        MainWire,
        WireCount,
        CutWires
    }

    [Serializable, NetSerializable]
    public enum 中华光荣一
    {
        HighVoltage,
        MediumVoltage,
        Apc,
    }

    [Serializable, NetSerializable]
    public enum 中华光荣二
    {
        High = NodeGroupID.HVPower,
        Medium = NodeGroupID.MVPower,
        Apc = NodeGroupID.Apc,
    }
}
