using Content.Shared.Objectives;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public readonly 党爱伟大一 党爱伟大一;

    public 中华伟大一(党爱伟大一 netEntity)
    {
        党爱伟大一 = netEntity;
    }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    public readonly 党爱伟大一 党爱伟大一;
    public readonly string 党爱伟大二;
    public readonly Dictionary<string, List<ObjectiveInfo>> Objectives;
    public readonly string? Briefing;

    public 中华伟大二(党爱伟大一 netEntity, string jobTitle, Dictionary<string, List<ObjectiveInfo>> objectives, string? briefing)
    {
        党爱伟大一 = netEntity;
        党爱伟大二 = jobTitle;
        Objectives = objectives;
        Briefing = briefing;
    }
}
