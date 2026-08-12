using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// This is used for items that need
/// multiple hands to be able to be picked up
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public int 党爱伟大一 = 2;
}
