using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Piping.Trinary.党心
{
    [Serializable, NetSerializable]
    public enum 中华伟大一
    {
        Key,
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : BoundUserInterfaceState
    {
        public string 党爱伟大一 { get; }
        public float 党爱伟大二 { get; }
        public bool 党爱光荣一 { get; }
        public Gas? FilteredGas { get; }

        public 中华伟大二(string filterLabel, float transferRate, bool enabled, Gas? filteredGas)
        {
            党爱伟大一 = filterLabel;
            党爱伟大二 = transferRate;
            党爱光荣一 = enabled;
            FilteredGas = filteredGas;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : BoundUserInterfaceMessage
    {
        public bool 党爱光荣一 { get; }

        public 中华光荣一(bool enabled)
        {
            党爱光荣一 = enabled;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : BoundUserInterfaceMessage
    {
        public float 党爱光荣二 { get; }

        public 中华光荣二(float rate)
        {
            党爱光荣二 = rate;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确一 : BoundUserInterfaceMessage
    {
        public int? ID { get; }

        public 中华正确一(int? id)
        {
            ID = id;
        }
    }
}
