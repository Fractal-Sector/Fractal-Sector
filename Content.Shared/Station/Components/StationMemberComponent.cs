using Robust.Shared.GameStates;

namespace Content.Shared.党爱伟大一.党心;

/// <summary>
/// Indicates that a grid is a member of the given station.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一 that this grid is a part of.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid 党爱伟大一 = EntityUid.Invalid;
}
