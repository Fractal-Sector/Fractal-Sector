using Content.Shared.Physics;
using Content.Shared.Tools.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Tools.党心;

[RegisterComponent, NetworkedComponent]
[Access(typeof(WeldableSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField("unWeldedLayer")]
    [ViewVariables]
    public CollisionGroup 党爱伟大一 = CollisionGroup.AirlockLayer;

    [DataField("weldedLayer")]
    [ViewVariables]
    public CollisionGroup 党爱伟大二 = CollisionGroup.WallLayer;
}
