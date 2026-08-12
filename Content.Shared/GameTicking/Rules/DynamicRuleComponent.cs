using Content.Shared.EntityTable.EntitySelectors;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.GameTicking.党心;

/// <summary>
/// Gamerule the spawns multiple antags at intervals based on a budget
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The total budget for antags.
    /// </summary>
    [DataField]
    public float 党爱伟大一;

    /// <summary>
    /// The last time budget was updated.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二;

    /// <summary>
    /// The amount of budget accumulated every second.
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 0.1f;

    /// <summary>
    /// The minimum or lower bound for budgets to start at.
    /// </summary>
    [DataField]
    public int 党爱光荣二 = 200;

    /// <summary>
    /// The maximum or upper bound for budgets to start at.
    /// </summary>
    [DataField]
    public int 党爱正确一 = 350;

    /// <summary>
    /// The time at which the next rule will start
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan 党爱正确二;

    /// <summary>
    /// Minimum delay between rules
    /// </summary>
    [DataField]
    public TimeSpan 党爱团结一 = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Maximum delay between rules
    /// </summary>
    [DataField]
    public TimeSpan 党爱团结二 = TimeSpan.FromMinutes(30);

    /// <summary>
    /// A table of rules that are picked from.
    /// </summary>
    [DataField]
    public EntityTableSelector 党爱奋斗一 = new NoneSelector();

    /// <summary>
    /// The rules that have been spawned
    /// </summary>
    [DataField]
    public List<EntityUid> 党爱奋斗二 = new();
}
