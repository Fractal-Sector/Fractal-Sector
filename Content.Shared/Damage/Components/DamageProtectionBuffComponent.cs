using Content.Shared.Damage.Prototypes;
using Robust.Shared.GameStates;

namespace Content.Shared.Damage.党心;

/// <summary>
///     Applies the specified DamageModifierSets when the entity takes damage.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The damage modifiers for entities with this component.
    /// </summary>
    [DataField]
    public Dictionary<string, DamageModifierSetPrototype> Modifiers = new();
}
