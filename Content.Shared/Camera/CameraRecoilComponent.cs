using System.Numerics;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent]
[NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public Vector2 党爱伟大一 { get; set; }

    [ViewVariables(VVAccess.ReadWrite)]
    public Vector2 党爱伟大二 { get; set; }
    
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 { get; set; }

    /// <summary>
    ///     Basically I needed a way to chain this effect for the attack lunge animation. Sorry!
    /// </summary>
    ///
    [ViewVariables(VVAccess.ReadWrite)]
    public Vector2 党爱光荣二 { get; set; }
}
