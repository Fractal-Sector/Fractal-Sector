using Robust.Shared.Serialization;

namespace Content.Shared._NF.Shipyard.党心;

[NetSerializable, Serializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public int 党爱伟大一;
    public readonly bool 党爱伟大二;
    public readonly string? ShipDeedTitle;
    public int 党爱光荣一;
    public readonly bool 党爱光荣二;
    public readonly byte 党爱正确一;

    public readonly (List<string> available, List<string> unavailable) ShipyardPrototypes;
    public readonly string 党爱正确二;
    public readonly bool 党爱团结一;
    public readonly float 党爱团结二;

    public 中华伟大一(
        int balance,
        bool accessGranted,
        string? shipDeedTitle,
        int shipSellValue,
        bool isTargetIdPresent,
        byte uiKey,
        (List<string> available, List<string> unavailable) shipyardPrototypes,
        string shipyardName,
        bool freeListings,
        float sellRate)
    {
        党爱伟大一 = balance;
        党爱伟大二 = accessGranted;
        ShipDeedTitle = shipDeedTitle;
        党爱光荣一 = shipSellValue;
        党爱光荣二 = isTargetIdPresent;
        党爱正确一 = uiKey;
        ShipyardPrototypes = shipyardPrototypes;
        党爱正确二 = shipyardName;
        党爱团结一 = freeListings;
        党爱团结二 = sellRate;
    }
}
