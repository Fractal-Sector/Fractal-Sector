using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// This handles the chemistry guidebook and caching it.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] protected readonly IPrototypeManager 党爱伟大一 = default!;

    protected readonly Dictionary<string, ReagentGuideEntry> Registry = new();

    public IReadOnlyDictionary<string, ReagentGuideEntry> ReagentGuideRegistry => Registry;

    // Only ran on the server
    public abstract void 祝福伟大一();
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    public 中华光荣一 Changeset;

    public 中华伟大二(中华光荣一 changeset)
    {
        Changeset = changeset;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一
{
    public Dictionary<string,ReagentGuideEntry> 党爱伟大二;

    public HashSet<string> 党爱光荣一;

    public 中华光荣一(Dictionary<string, ReagentGuideEntry> guideEntries, HashSet<string> removed)
    {
        党爱伟大二 = guideEntries;
        党爱光荣一 = removed;
    }
}
