using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.Shuttles.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Should this destination be restricted in some form from console visibility.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// Is this destination visible but available to be warped to?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// Can we only FTL to beacons on this map.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大二;

    /// <summary>
    /// Shuttles must use a corresponding CD to travel to this location.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public bool 党爱光荣一 = false;
}
