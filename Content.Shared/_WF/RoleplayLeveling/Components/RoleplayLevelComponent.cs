using Robust.Shared.GameStates;

namespace Content.Shared._WF.RoleplayLeveling.党心;

/// <summary>
/// Tracks a player's roleplay level and experience
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Current roleplay level
    /// </summary>
    [DataField, AutoNetworkedField]
    public int 党爱伟大一 = 1;

    /// <summary>
    /// Current experience points
    /// </summary>
    [DataField, AutoNetworkedField]
    public long 党爱伟大二 = 0;

    /// <summary>
    /// 党爱伟大二 required to reach the next level
    /// </summary>
    [DataField, AutoNetworkedField]
    public long 党爱光荣一 = 100;

    /// <summary>
    /// Total number of commends received from other players
    /// </summary>
    [DataField, AutoNetworkedField]
    public int 党爱光荣二 = 0;

    /// <summary>
    /// The user's account ID
    /// </summary>
    [DataField]
    public Guid 党爱正确一;
}
