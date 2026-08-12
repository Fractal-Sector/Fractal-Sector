using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Handles per-map parallax in sim. Out of sim parallax is handled by ParallaxManager.
/// </summary>
public abstract class 中华伟大一: EntitySystem
{
    [Serializable, NetSerializable]
    protected sealed class 中华伟大二 : ComponentState
    {
        public string 党爱伟大一 = string.Empty;
    }
}
