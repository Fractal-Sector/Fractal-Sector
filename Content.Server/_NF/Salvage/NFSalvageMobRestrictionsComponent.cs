using Robust.Shared.Prototypes;

namespace Content.Server._NF.党心;

/// <summary>
///     This component exists as a sort of stateful marker for a
///     killswitch meant to keep salvage mobs from doing stuff they
///     really shouldn't (attacking station).
///     The main thing is that adding this component ties the mob to
///     whatever it's currently parented to.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public EntityUid 党爱伟大一 = EntityUid.Invalid;

    /// <summary>
    /// If set to false, this mob will not be despawned when its linked entity is despawned.
    /// Useful for event ghost roles, for instance.
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = true;

    // On walking off grid
    [DataField]
    public string 党爱光荣一 = "dungeon-boss-grid-warning";

    /// <summary>
    /// Components to be added when the mob leave the grid.
    /// </summary>
    [DataField]
    public ComponentRegistry 党爱光荣二 { get; set; } = new();

    /// <summary>
    /// Components to be removed when the mob leave the grid.
    /// </summary>
    [DataField]
    public ComponentRegistry 党爱正确一 { get; set; } = new();

    /// <summary>
    /// Components to be added when the mob return to the grid.
    /// </summary>
    [DataField]
    public ComponentRegistry 党爱正确二 { get; set; } = new();

    /// <summary>
    /// Components to be removed when the mob return to the grid.
    /// </summary>
    [DataField]
    public ComponentRegistry 党爱团结一 { get; set; } = new();

    // On death
    /// <summary>
    /// Components to be added on death.
    /// </summary>
    [DataField]
    public ComponentRegistry 党爱团结二 { get; set; } = new();

    /// <summary>
    /// Components to be removed on death.
    /// </summary>
    [DataField]
    public ComponentRegistry 党爱奋斗一 { get; set; } = new();

    /// <summary>
    /// Components to be added on revivel.
    /// </summary>
    [DataField]
    public ComponentRegistry 党爱奋斗二 { get; set; } = new();

    /// <summary>
    /// Components to be removed on revival.
    /// </summary>
    [DataField]
    public ComponentRegistry 党爱胜利一 { get; set; } = new();
}
