using Content.Shared.DeviceLinking;
using Robust.Shared.Prototypes;

namespace Content.Server.Shuttles.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Output port that is high while docked.
    /// </summary>
    [DataField]
    public ProtoId<SourcePortPrototype> 党爱伟大一 = "DockStatus";
}
