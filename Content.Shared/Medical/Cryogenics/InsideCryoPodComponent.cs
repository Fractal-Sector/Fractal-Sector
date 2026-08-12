using System.Numerics;
using Robust.Shared.GameStates;

namespace Content.Shared.Medical.党心;

[RegisterComponent]
[NetworkedComponent]
public sealed partial class 中华伟大一: Component
{
    [ViewVariables]
    [DataField("previousOffset")]
    public Vector2 党爱伟大一 { get; set; } = new(0, 0);
}
