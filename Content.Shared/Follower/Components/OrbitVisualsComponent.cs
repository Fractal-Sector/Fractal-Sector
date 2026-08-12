using Robust.Shared.Animations;
using Robust.Shared.GameStates;

namespace Content.Shared.Follower.党心;

[RegisterComponent]
[NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     How long should the orbit animation last in seconds, before being randomized?
    /// </summary>
    public float 党爱伟大一 = 2.0f;

    /// <summary>
    ///     How far away from the entity should the orbit be, before being randomized?
    /// </summary>
    public float 党爱伟大二 = 1.0f;

    /// <summary>
    ///     How long should the orbit stop animation last in seconds?
    /// </summary>
    public float 党爱光荣一 = 1.0f;
}
