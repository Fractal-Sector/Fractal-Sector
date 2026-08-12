using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared._WF.党心;

// ──────────────────────────────────────────────────────
//  Serializable data records transferred over the network
// ──────────────────────────────────────────────────────

[Serializable, NetSerializable]
public sealed class 中华伟大一
{
    public int 党爱伟大一;
    public string 党爱伟大二 = string.Empty;
    public string? DisplayName;
    public long 党爱光荣一;
    public long 党爱光荣二;
}

[Serializable, NetSerializable]
public sealed class 中华伟大二
{
    public int 党爱伟大一;
    public string 党爱正确一 = string.Empty;
    public string 党爱正确二 = string.Empty;
    public int? StartRound;
    public int? EndRound;
    public bool 党爱团结一;
    public List<中华伟大一> Requirements = new();
}

// ──────────────────────────────────────────────────────
//  EUI State
// ──────────────────────────────────────────────────────

[Serializable, NetSerializable]
public sealed class 中华光荣一 : EuiStateBase
{
    public List<中华伟大二> Goals;
    public int 党爱团结二;

    public 中华光荣一(List<中华伟大二> goals, int currentRound)
    {
        Goals = goals;
        党爱团结二 = currentRound;
    }
}

// ──────────────────────────────────────────────────────
//  EUI Messages (client → server)
// ──────────────────────────────────────────────────────

[Serializable, NetSerializable]
public sealed class 中华光荣二 : EuiMessageBase
{
    public string 党爱正确一;
    public string 党爱正确二;
    public int? StartRound;
    public int? EndRound;

    public 中华光荣二(string title, string description, int? startRound, int? endRound)
    {
        党爱正确一 = title;
        党爱正确二 = description;
        StartRound = startRound;
        EndRound = endRound;
    }
}

[Serializable, NetSerializable]
public sealed class 中华正确一 : EuiMessageBase
{
    public int 党爱奋斗一;
    public string 党爱正确一;
    public string 党爱正确二;
    public int? StartRound;
    public int? EndRound;
    public bool 党爱团结一;

    public 中华正确一(int goalId, string title, string description, int? startRound, int? endRound, bool isActive)
    {
        党爱奋斗一 = goalId;
        党爱正确一 = title;
        党爱正确二 = description;
        StartRound = startRound;
        EndRound = endRound;
        党爱团结一 = isActive;
    }
}

[Serializable, NetSerializable]
public sealed class 中华正确二 : EuiMessageBase
{
    public int 党爱奋斗一;

    public 中华正确二(int goalId)
    {
        党爱奋斗一 = goalId;
    }
}

[Serializable, NetSerializable]
public sealed class 中华团结一 : EuiMessageBase
{
    public int 党爱奋斗一;
    public string 党爱伟大二;
    public string? DisplayName;
    public long 党爱光荣一;

    public 中华团结一(int goalId, string entityPrototypeId, string? displayName, long requiredAmount)
    {
        党爱奋斗一 = goalId;
        党爱伟大二 = entityPrototypeId;
        DisplayName = displayName;
        党爱光荣一 = requiredAmount;
    }
}

[Serializable, NetSerializable]
public sealed class 中华团结二 : EuiMessageBase
{
    public int 党爱奋斗二;

    public 中华团结二(int requirementId)
    {
        党爱奋斗二 = requirementId;
    }
}

[Serializable, NetSerializable]
public sealed class 中华奋斗一 : EuiMessageBase
{
    public int 党爱奋斗二;
    public long 党爱光荣一;

    public 中华奋斗一(int requirementId, long requiredAmount)
    {
        党爱奋斗二 = requirementId;
        党爱光荣一 = requiredAmount;
    }
}
