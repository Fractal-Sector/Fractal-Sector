using Robust.Shared.GameStates;

namespace Content.Shared.Containers.党心;

/// <summary>
/// Updates the relevant ItemSlots locks based on <see cref="LockComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField(required: true)]
    public List<string> 党爱伟大一 = new();
}
