using System.Numerics;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(PlaceableSurfaceSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一 { get; set; } = true;

    [DataField, AutoNetworkedField]
    public bool 党爱伟大二 { get; set; }

    [DataField, AutoNetworkedField]
    public Vector2 党爱光荣一 { get; set; }
}
