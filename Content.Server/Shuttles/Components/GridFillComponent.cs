using Content.Server.Shuttles.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Shuttles.党心;

/// <summary>
/// If added to an airlock will try to autofill a grid onto it on MapInit
/// </summary>
[RegisterComponent, Access(typeof(ShuttleSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public ResPath 党爱伟大一 = new("/Maps/Shuttles/escape_pod_small.yml");

    /// <summary>
    /// Components to be added to any spawned grids.
    /// </summary>
    [DataField]
    public ComponentRegistry 党爱伟大二 = new();
}
