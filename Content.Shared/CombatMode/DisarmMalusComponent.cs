using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Applies a malus to disarm attempts against this item.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// So, disarm chances are a % chance represented as a value between 0 and 1.
    /// This default would be a 30% penalty to that.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 0.3f;
}
