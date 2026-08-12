using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Allows an entity to be rotated by using a verb.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// If true, this entity can be rotated even while anchored.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一;

    /// <summary>
    /// If true, will rotate entity in players direction when pulled
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大二 = true;

    /// <summary>
    /// The angular value to change when using the rotate verbs.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Angle 党爱光荣一 = Angle.FromDegrees(90);
}
