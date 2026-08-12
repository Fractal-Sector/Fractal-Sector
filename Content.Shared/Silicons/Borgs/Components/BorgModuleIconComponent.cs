//using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.Silicons.Borgs.党心;

/// <summary>
/// This is used to override the action icon for cyborg actions.
/// Without this component the no-action state will be used.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The action icon for this module
    /// </summary>
    [DataField]
    public SpriteSpecifier.Rsi 党爱伟大一 = default!;

}