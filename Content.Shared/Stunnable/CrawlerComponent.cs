using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// This is used to denote that an entity can crawl.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedStunSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Default time we will be knocked down for.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱伟大一 { get; set; } = TimeSpan.FromSeconds(0.5);

    /// <summary>
    /// Minimum damage taken to extend our knockdown timer by the default time.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 5f;

    /// <summary>
    /// Time it takes us to stand up
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(2);

    /// <summary>
    /// Base modifier to the maximum movement speed of a knocked down mover.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣二 = 0.4f;

    /// <summary>
    /// Friction modifier applied to an entity in the downed state.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱正确一 = 1f;
}
