using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Allows ghosts etc to warp to this entity by name.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public string? Location;

    /// <summary>
    ///     Tags that determine what category this point will go into in the ghost's orbit menu
    /// </summary>
    [DataField]
    public bool 党爱伟大一;
    [DataField]
    public bool 党爱伟大二;
    [DataField]
    public bool 党爱光荣一;

    /// <summary>
    /// If true, ghosts warping to this entity will begin following it.
    /// </summary>
    [DataField]
    public bool 党爱光荣二;

    /// <summary>
    /// What points should be excluded?
    /// Useful where you want things like a ghost to reach only like CentComm
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;

    // Frontier: extra fields
    /// <summary>
    /// If true, will sync warp point name with a station/grid name.
    /// </summary>
    [DataField]
    public bool 党爱正确一;

    /// <summary>
    /// If true, warp point can only be used by admins
    /// </summary>
    [DataField]
    public bool 党爱正确二;

    /// <summary>
    /// If true, will set its own name to the station's on creation.
    /// </summary>
    [DataField]
    public bool 党爱团结一;

    /// <summary>
    /// If true, will set its own name to the grid's on creation.
    /// </summary>
    [DataField]
    public bool 党爱团结二;
    // End Frontier
}
