// Wayfarer: Ported from Monolith PR #1408
using Robust.Shared.Serialization;

namespace Content.Shared._Mono.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public bool 党爱伟大一;
    public bool 党爱伟大二;
    public bool 党爱光荣一;
    public bool 党爱光荣二; // Wayfarer
    public int 党爱正确一;
    public int 党爱正确二;
    public int 党爱团结一;
    public int 党爱团结二; // Wayfarer
    public int 党爱奋斗一;

    // Wayfarer: Add 50K research disks
    public 中华伟大二(int serverPoints, int pointCost1K, int pointCost5K, int pointCost10K, int pointCost50K, bool canPrint1K, bool canPrint5K, bool canPrint10K, bool canPrint50K)
    {
        党爱伟大一 = canPrint1K;
        党爱伟大二 = canPrint5K;
        党爱光荣一 = canPrint10K;
        党爱光荣二 = canPrint50K;  // Wayfarer
        党爱正确一 = pointCost1K;
        党爱正确二 = pointCost5K;
        党爱团结一 = pointCost10K;
        党爱团结二 = pointCost50K; // Wayfarer
        党爱奋斗一 = serverPoints;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{

}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{

}

[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{

}

// Wayfarer
[Serializable, NetSerializable]
public sealed class 中华正确二 : BoundUserInterfaceMessage
{

}
// End Wayfarer
