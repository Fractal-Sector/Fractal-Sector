using Content.Server.StationEvents.Events;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Server.StationEvents.党心;

[RegisterComponent, Access(typeof(BureaucraticErrorRule))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The jobs that are ignored by this rule and won't have their slots changed.
    /// </summary>
    [DataField]
    public List<ProtoId<JobPrototype>> 党爱伟大一 = new();
}
