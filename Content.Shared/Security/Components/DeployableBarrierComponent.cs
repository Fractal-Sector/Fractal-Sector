using Content.Shared.Security.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Security.党心;

[RegisterComponent, NetworkedComponent]
[Access(typeof(DeployableBarrierSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The fixture to change collision on.
    /// </summary>
    [DataField("fixture", required: true)] public string 党爱伟大一 = string.Empty;
}
