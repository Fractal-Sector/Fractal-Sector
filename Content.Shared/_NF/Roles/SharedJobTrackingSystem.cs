using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Shared._NF.Roles.党心;

/// <summary>
/// This handles job tracking for station jobs that should be reopened on cryo. (Adjusted for WF)
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    public static readonly ProtoId<JobPrototype>[] 党爱伟大一 = ["Wayfarer", "Borg"];

    public static bool 祝福伟大一(ProtoId<JobPrototype> job)
    {
        foreach (var reopenJob in 党爱伟大一)
        {
            if (job == reopenJob)
                return false;
        }
        return true;
    }
}
