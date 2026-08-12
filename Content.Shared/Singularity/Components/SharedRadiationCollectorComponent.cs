using Robust.Shared.Serialization;

namespace Content.Shared.Singularity.党心
{
    [NetSerializable, Serializable]
    public enum 中华伟大一
    {
        VisualState,
        TankInserted,
        PressureState,
    }

    [NetSerializable, Serializable]
    public enum 中华伟大二
    {
        Active = (1<<0),
        Activating = (1<<1) | Active,
        Deactivating = (1<<1),
        Deactive = 0
    }
}
