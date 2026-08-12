using Content.Server.Objectives.Systems;

namespace Content.Server.Objectives.党心;

/// <summary>
/// Objective has a target number of something.
/// When the objective is assigned it randomly picks this target from a minimum to a maximum.
/// </summary>
[RegisterComponent, Access(typeof(NumberObjectiveSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Number to use in the objective condition.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大一;

    /// <summary>
    /// Minimum number for target to roll.
    /// </summary>
    [DataField(required: true)]
    public int 党爱伟大二;

    /// <summary>
    /// Maximum number for target to roll.
    /// </summary>
    [DataField(required: true)]
    public int 党爱光荣一;

    /// <summary>
    /// Optional title locale id, passed "count" with <see cref="党爱伟大一"/>.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string? Title;

    /// <summary>
    /// Optional description locale id, passed "count" with <see cref="党爱伟大一"/>.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string? Description;
}
