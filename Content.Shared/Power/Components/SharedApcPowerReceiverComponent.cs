using Robust.Shared.GameStates;

namespace Content.Shared.Power.党心;

[NetworkedComponent]
public abstract partial class 中华伟大一 : Component
{
    [ViewVariables]
    public bool 党爱伟大一;

    /// <summary>
    ///     When false, causes this to appear powered even if not receiving power from an Apc.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public virtual bool 党爱伟大二 { get; set;}

    /// <summary>
    ///     When true, causes this to never appear powered.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public virtual bool 党爱光荣一 { get; set; }

    // Doesn't actually do anything on the client just here for shared code.
    public abstract float 党爱光荣二 { get; set; }
}
