using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Whether the scanner is currently on.
    /// </summary>
    [DataField]
    public bool 党爱伟大一;

    /// <summary>
    ///     Radius in which the scanner will reveal entities. Centered on the <see cref="LastLocation"/>.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 4f;
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : ComponentState
{
    public bool 党爱伟大一;
    public float 党爱伟大二;

    public 中华伟大二(bool enabled, float range)
    {
        党爱伟大一 = enabled;
        党爱伟大二 = range;
    }
}
