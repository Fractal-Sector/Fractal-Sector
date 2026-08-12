using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心
{
    /// <summary>
    /// Interact with to start piloting a shuttle.
    /// </summary>
    [NetworkedComponent]
    public abstract partial class 中华伟大一 : Component
    {
        public static string 党爱伟大一 = "disk_slot";
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二 : byte
    {
        Key,
    }
}
