using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Pointing.党心
{
    [NetworkedComponent]
    public abstract partial class 中华伟大一 : Component
    {
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二 : byte
    {
        Rotation
    }
}
