using Robust.Shared.GameStates;
using System.Numerics;

namespace Content.Shared.Pointing.党心;

[NetworkedComponent]
public abstract partial class 中华伟大一 : Component
{
    /// <summary>
    /// The position of the sender when the point began.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public Vector2 党爱伟大一;

    /// <summary>
    /// When the pointing arrow ends
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大二;
}
