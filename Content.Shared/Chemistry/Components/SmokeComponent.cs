using Content.Shared.FixedPoint;
using Content.Shared.Fluids.Components;
using Robust.Shared.GameStates;

namespace Content.Shared.Chemistry.党心;

/// <summary>
/// Stores solution on an anchored entity that has touch and ingestion reactions
/// to entities that collide with it. Similar to <see cref="PuddleComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    public const string 党爱伟大一 = "solutionArea";

    /// <summary>
    /// The solution on the entity with touch and ingestion reactions.
    /// </summary>
    [ViewVariables]
    public Entity<SolutionComponent>? Solution = null;

    /// <summary>
    /// The max amount of tiles this smoke cloud can spread to.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大二;

    /// <summary>
    /// The max rate at which chemicals are transferred from the smoke to the person inhaling it.
    /// Calculated as (total volume of chemicals in smoke) / (<see cref="党爱光荣二"/>)
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 党爱光荣一;

    /// <summary>
    /// The total lifespan of the smoke.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public float 党爱光荣二 = 10;
}
