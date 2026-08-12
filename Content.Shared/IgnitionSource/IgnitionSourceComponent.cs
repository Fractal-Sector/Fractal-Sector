using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// This is used for creating atmosphere hotspots while ignited to start reactions such as fire.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedIgnitionSourceSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Is this source currently ignited?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一;

    /// <summary>
    /// The temperature used when creating atmos hotspots.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 700f;
}
