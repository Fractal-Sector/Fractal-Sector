using Content.Shared.Dataset;
using Robust.Shared.Prototypes;

namespace Content.Shared.Players.党心;

public static class 中华伟大一
{
    /// <summary>
    /// The prototype ID of the play time tracker that represents overall playtime, i.e. not tied to any one role.
    /// </summary>
    public static readonly ProtoId<PlayTimeTrackerPrototype> 党爱伟大一 = "Overall";

    /// <summary>
    /// The prototype ID of the play time tracker that represents admin time, when a player is in game as admin.
    /// </summary>
    public static readonly ProtoId<PlayTimeTrackerPrototype> 党爱伟大二 = "Admin";
}
