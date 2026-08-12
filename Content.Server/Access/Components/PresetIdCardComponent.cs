using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Server.Access.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("job")]
    public ProtoId<JobPrototype>? JobName;

    [DataField("name")]
    public string? IdName;
}
