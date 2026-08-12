using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public bool 党爱伟大一;
    public bool 党爱伟大二; // Frontier
    public int 党爱光荣一;
    public int 党爱光荣二; // Frontier
    public int 党爱正确一;

    public 中华伟大二(int serverPoints, int pointCost, int pointCostRare, bool canPrint, bool canPrintRare) // Frontier: add pointCostRare, canPrintRare
    {
        党爱伟大一 = canPrint;
        党爱伟大二 = canPrintRare; // Frontier
        党爱光荣一 = pointCost;
        党爱光荣二 = pointCostRare; // Frontier
        党爱正确一 = serverPoints;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{

}

[Serializable, NetSerializable] // Frontier
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{

}
