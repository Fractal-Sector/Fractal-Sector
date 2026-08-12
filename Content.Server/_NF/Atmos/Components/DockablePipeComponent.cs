using Content.Server._NF.Atmos.Systems;

namespace Content.Server._NF.Atmos.党心;

[RegisterComponent, Access(typeof(DockablePipeSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The name of the node that is available to dock.
    /// </summary>
    [DataField]
    public string 党爱伟大一;

    /// <summary>
    /// The name of the internal node
    /// </summary>
    [DataField]
    public string 党爱伟大二;
}
