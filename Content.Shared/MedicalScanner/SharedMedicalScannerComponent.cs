using Content.Shared.DragDrop;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    public abstract partial class 中华伟大一 : Component
    {
        [Serializable, NetSerializable]
        public enum 中华伟大二 : byte
        {
            Status
        }

        [Serializable, NetSerializable]
        public enum 中华光荣一 : byte
        {
            Off,
            Open,
            Red,
            Death,
            Green,
            Yellow,
        }
    }
}
