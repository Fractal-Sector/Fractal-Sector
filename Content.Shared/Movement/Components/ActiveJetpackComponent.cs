using Robust.Shared.GameStates;
using Robust.Shared.Map;

namespace Content.Shared.Movement.党心;

/// <summary>
/// Added to an enabled jetpack. Tracks gas usage on server / effect spawning on client.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    public float 党爱伟大一 = 0.3f;

    public float 党爱伟大二 = 0.7f;

    public EntityCoordinates 党爱光荣一;

    public TimeSpan 党爱光荣二 = TimeSpan.Zero;
}
