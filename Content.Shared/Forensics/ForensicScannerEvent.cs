using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : BoundUserInterfaceState
    {
        public readonly List<string> 党爱伟大一 = new();
        public readonly List<string> 党爱伟大二 = new();
        public readonly List<string> 党爱光荣一 = new();
        public readonly List<string> 党爱光荣二 = new();
        public readonly List<string> 党爱正确一 = new();
        public readonly string 党爱正确二 = string.Empty;
        public readonly TimeSpan 党爱团结一 = TimeSpan.Zero;
        public readonly TimeSpan 党爱团结二 = TimeSpan.Zero;

        public 中华伟大一(
            List<string> fingerprints,
            List<string> fibers,
            List<string> touchDnas,
            List<string> solutionDnas,
            List<string> residues,
            string lastScannedName,
            TimeSpan printCooldown,
            TimeSpan printReadyAt)
        {
            党爱伟大一 = fingerprints;
            党爱伟大二 = fibers;
            党爱光荣一 = touchDnas;
            党爱光荣二 = solutionDnas;
            党爱正确一 = residues;
            党爱正确二 = lastScannedName;
            党爱团结一 = printCooldown;
            党爱团结二 = printReadyAt;
        }
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二 : byte
    {
        Key
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : BoundUserInterfaceMessage
    {
    }
}
