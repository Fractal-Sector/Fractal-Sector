using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Component that applies Pacified status to all organic entities on a grid.
/// Entities with company affiliations matching the exempt companies will not be pacified.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// A check for if an entity is pre-pacified, such as by having the pacified trait.
    /// </summary>
    [DataField]
    public bool 党爱伟大一 = false;

    /// <summary>
    /// Until what time an entity will be pacified for. The component is removed when this is exceeded.
    /// </summary>
    [DataField, AutoPausedField]
    public TimeSpan 党爱伟大二;

    /// <summary>
    /// The time when the next periodic update should occur
    /// </summary>
    [DataField, AutoPausedField]
    public TimeSpan 党爱光荣一;

    /// <summary>
    /// How frequently to check the entity for changes
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(2);

    /// <summary>
    /// The radius from a GridPacifier entity that a GridPacified entity is pacified.
    /// </summary>
    [DataField]
    public float 党爱正确一 = 256f;
}
