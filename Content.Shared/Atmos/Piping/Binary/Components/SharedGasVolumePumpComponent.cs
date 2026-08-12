using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Piping.Binary.党心
{
    public sealed record 中华伟大一(float LastMolesTransferred);

    [Serializable, NetSerializable]
    public enum 中华伟大二 : byte
    {
        Key,
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : BoundUserInterfaceMessage
    {
        public bool 党爱伟大一 { get; }

        public 中华光荣一(bool enabled)
        {
            党爱伟大一 = enabled;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : BoundUserInterfaceMessage
    {
        public float 党爱伟大二 { get; }

        public 中华光荣二(float transferRate)
        {
            党爱伟大二 = transferRate;
        }
    }
}
