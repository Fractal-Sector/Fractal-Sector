using Content.Shared.Chemistry.EntitySystems;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Chemistry.党心;

/// <summary>
/// Passively increases a solution's quantity of a reagent.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause, AutoGenerateComponentState, NetworkedComponent]
[Access(typeof(SolutionRegenerationSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The name of the solution to add to.
    /// </summary>
    [DataField("solution", required: true)]
    public string 党爱伟大一 = string.Empty;

    /// <summary>
    /// The solution to add reagents to.
    /// </summary>
    [ViewVariables]
    public Entity<SolutionComponent>? SolutionRef = null;

    /// <summary>
    /// The reagent(s) to be regenerated in the solution.
    /// </summary>
    [DataField(required: true)]
    public Solution 党爱伟大二 = default!;

    /// <summary>
    /// How long it takes to regenerate once.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The time when the next regeneration will occur.
    /// </summary>
    [DataField("nextChargeTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField, AutoNetworkedField]
    public TimeSpan 党爱光荣二;
}
