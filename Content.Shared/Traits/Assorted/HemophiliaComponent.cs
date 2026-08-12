using Robust.Shared.GameStates;

namespace Content.Shared.Traits.党心;

/// <summary>
/// This component is used for the Hemophilia Trait, it reduces the passive bleed stack reduction amount so entities with it bleed for longer.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// What multiplier should be applied to the BleedReduction when an entity bleeds?
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大一 = 0.33f;
}
