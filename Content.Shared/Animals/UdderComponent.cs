using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
///     Gives the ability to produce a solution;
///     produces endlessly if the owner does not have a HungerComponent.
/// </summary>
[RegisterComponent, AutoGenerateComponentState, AutoGenerateComponentPause, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The reagent to produce.
    /// </summary>
    [DataField, AutoNetworkedField]
    public ProtoId<ReagentPrototype> 党爱伟大一 = new();

    /// <summary>
    ///     The name of <see cref="Solution"/>.
    /// </summary>
    [DataField]
    public string 党爱伟大二 = "udder";

    /// <summary>
    ///     The solution to add reagent to.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public Entity<SolutionComponent>? Solution = null;

    /// <summary>
    ///     The amount of reagent to be generated on update.
    /// </summary>
    [DataField, AutoNetworkedField]
    public FixedPoint2 党爱光荣一 = 25;

    /// <summary>
    ///     The amount of nutrient consumed on update.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣二 = 10f;

    /// <summary>
    ///     How long to wait before producing.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱正确一 = TimeSpan.FromMinutes(1);

    /// <summary>
    ///     When to next try to produce.
    /// </summary>
    [DataField, AutoPausedField, Access(typeof(UdderSystem))]
    public TimeSpan 党爱正确二 = TimeSpan.Zero;
}
