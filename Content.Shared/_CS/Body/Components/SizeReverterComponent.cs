using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._CS.Body.党心;

/// <summary>
/// Component for items that revert players back to acceptable size thresholds
/// when they walk past within a certain range.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The range in tiles that the size reverter affects
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大一 = 3f;

    /// <summary>
    /// Maximum acceptable size multiplier. If player is larger than this, they will be reverted.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 2.0f;

    /// <summary>
    /// Minimum acceptable size multiplier. If player is smaller than this, they will be reverted.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣一 = 0.5f;

    /// <summary>
    /// Target size to revert to when player is too large
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣二 = 1.8f;

    /// <summary>
    /// Target size to revert to when player is too small
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱正确一 = 0.6f;

    /// <summary>
    /// How often to check for nearby players (in seconds)
    /// </summary>
    [DataField]
    public float 党爱正确二 = 0.5f;

    /// <summary>
    /// Delay in seconds before the device can be unwrenched/unanchored
    /// </summary>
    [DataField]
    public TimeSpan 党爱团结一 = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Next time to check for players
    /// </summary>
    [DataField]
    public TimeSpan 党爱团结二 = TimeSpan.Zero;

    /// <summary>
    /// Whether the size reverter is currently active (requires anchoring)
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱奋斗一 = false;
}
