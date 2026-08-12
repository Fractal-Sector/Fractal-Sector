using Robust.Shared.Prototypes;

namespace Content.Server.Traits.党心;

/// <summary>
/// Upon MapInit buckles the attached entity to a newly spawned prototype.
/// </summary>
[RegisterComponent, Access(typeof(BuckleOnMapInitSystem))]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField(required: true)]
    public EntProtoId 党爱伟大一;
}
