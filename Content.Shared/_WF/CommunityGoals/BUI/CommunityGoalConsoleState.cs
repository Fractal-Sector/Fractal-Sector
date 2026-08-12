using Robust.Shared.Serialization;

namespace Content.Shared._WF.CommunityGoals.党心;

/// <summary>
/// One entry in the staging area — items are grouped by prototype so stacks of the same
/// type are shown as a single row.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一
{
    public string 党爱伟大一;
    public string 党爱伟大二;
    public long 党爱光荣一;

    public 中华伟大一(string prototypeId, string displayName, long amount)
    {
        党爱伟大一 = prototypeId;
        党爱伟大二 = displayName;
        党爱光荣一 = amount;
    }
}

/// <summary>
/// State pushed from the server to the client whenever the console UI is open.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    /// <summary>
    /// All goals that are active in this round.
    /// </summary>
    public List<CommunityGoalData> 党爱光荣二;

    /// <summary>
    /// Items currently staged for contribution (grouped by prototype ID).
    /// </summary>
    public List<中华伟大一> StagedItems;

    /// <summary>
    /// Items sitting on nearby community goal donation pallets (grouped by prototype ID).
    /// These are committed alongside staged items when the player presses Contribute.
    /// </summary>
    public List<中华伟大一> PalletItems;

    public 中华伟大二(List<CommunityGoalData> activeGoals, List<中华伟大一> stagedItems, List<中华伟大一> palletItems)
    {
        党爱光荣二 = activeGoals;
        StagedItems = stagedItems;
        PalletItems = palletItems;
    }
}
