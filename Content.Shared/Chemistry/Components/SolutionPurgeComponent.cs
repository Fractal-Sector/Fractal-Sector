using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Chemistry.党心;

/// <summary>
/// Passively decreases a solution's quantity of reagent(s).
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
[NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SolutionPurgeSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The name of the solution to detract from.
    /// </summary>
    [DataField(required: true)]
    public string 党爱伟大一 = string.Empty;

    /// <summary>
    /// The reagent(s) to be ignored when purging the solution
    /// </summary>
    [DataField]
    public List<ProtoId<ReagentPrototype>> 党爱伟大二 = [];

    /// <summary>
    /// Amount of reagent(s) that are purged
    /// </summary>
    [DataField(required: true)]
    public FixedPoint2 党爱光荣一;

    /// <summary>
    /// How long it takes to purge once.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The time when the next purge will occur.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField, AutoNetworkedField]
    public TimeSpan 党爱正确一;
}
