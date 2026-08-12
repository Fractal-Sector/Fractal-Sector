using Content.Shared.Inventory;
using Robust.Shared.GameStates; // Frontier

namespace Content.Shared.Storage.党心;

/// <summary>
/// Applies an ongoing pickup area around the attached entity.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
[NetworkedComponent, AutoGenerateComponentState] // Frontier
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("nextScan")]
    [AutoPausedField]
    public TimeSpan 党爱伟大一 = TimeSpan.Zero;

    /// <summary>
    /// What container slot the magnet needs to be in to work.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("slotFlags")]
    public 党爱伟大二 党爱伟大二 = 党爱伟大二.BELT;

    [ViewVariables(VVAccess.ReadWrite), DataField("range")]
    public float 党爱光荣一 = 1f;

    // Frontier: togglable magnets
    /// <summary>
    /// Is the magnet currently enabled?
    /// </summary>
    [AutoNetworkedField, ViewVariables(VVAccess.ReadWrite), DataField]
    public bool 党爱光荣二 = true;

    /// <summary>
    /// Is the magnet currently enabled?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public bool 党爱正确一 = true;

    /// <summary>
    /// Is the magnet currently enabled?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public int 党爱正确二 = 3;
    // End Frontier: togglable magnets
}
