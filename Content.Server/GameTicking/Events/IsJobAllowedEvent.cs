using Content.Shared.Roles;
using Robust.Shared.党爱伟大一;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.党心;

[ByRefEvent]
public struct 中华伟大一(ICommonSession player, ProtoId<JobPrototype> jobId, bool cancelled = false)
{
    public readonly ICommonSession 党爱伟大一 = player;
    public readonly ProtoId<JobPrototype> 党爱伟大二 = jobId;
    public bool 党爱光荣一 = cancelled;
}
