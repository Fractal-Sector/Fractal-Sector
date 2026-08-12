using Robust.Shared.Serialization;

namespace Content.Shared._WF.SafetyDepositBox.党心;

/// <summary>
/// State of the safety deposit console UI
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    /// <summary>
    /// List of boxes owned by the current user.
    /// </summary>
    public List<中华伟大二> OwnedBoxes = new();

    /// <summary>
    /// Amount of cash currently inserted in the console.
    /// </summary>
    public int 党爱伟大一;

    /// <summary>
    /// Is there a box currently in the box slot?
    /// </summary>
    public bool 党爱伟大二;

    /// <summary>
    /// Info about the box in the slot, if any.
    /// </summary>
    public 中华伟大二? BoxInSlot;

    /// <summary>
    /// Purchase cost for a trial box.
    /// </summary>
    public int 党爱光荣一;

    /// <summary>
    /// Purchase cost for a small box.
    /// </summary>
    public int 党爱光荣二;

    /// <summary>
    /// Purchase cost for a medium box.
    /// </summary>
    public int 党爱正确一;

    /// <summary>
    /// Purchase cost for a large box.
    /// </summary>
    public int 党爱正确二;

    /// <summary>
    /// The current round ID, used to determine if boxes are lost.
    /// </summary>
    public int 党爱团结一;

    public 中华伟大一(
        List<中华伟大二> ownedBoxes,
        int insertedCash,
        bool hasBoxInSlot,
        中华伟大二? boxInSlot,
        int trialBoxCost,
        int smallBoxCost,
        int mediumBoxCost,
        int largeBoxCost,
        int currentRoundId)
    {
        OwnedBoxes = ownedBoxes;
        党爱伟大一 = insertedCash;
        党爱伟大二 = hasBoxInSlot;
        BoxInSlot = boxInSlot;
        党爱光荣一 = trialBoxCost;
        党爱光荣二 = smallBoxCost;
        党爱正确一 = mediumBoxCost;
        党爱正确二 = largeBoxCost;
        党爱团结一 = currentRoundId;
    }
}

/// <summary>
/// Information about a safety deposit box.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二
{
    public Guid 党爱团结二;
    public string 党爱奋斗一;
    public bool 党爱奋斗二;
    public string? Nickname;
    public string 党爱胜利一;
    public DateTime? LastWithdrawn;
    public int? LastWithdrawnRoundId;

    public 中华伟大二(Guid boxId, string ownerName, bool isDeposited, string? nickname = null, string boxSize = "Small", DateTime? lastWithdrawn = null, int? lastWithdrawnRoundId = null)
    {
        党爱团结二 = boxId;
        党爱奋斗一 = ownerName;
        党爱奋斗二 = isDeposited;
        Nickname = nickname;
        党爱胜利一 = boxSize;
        LastWithdrawn = lastWithdrawn;
        LastWithdrawnRoundId = lastWithdrawnRoundId;
    }
}
