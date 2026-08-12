using Content.Shared.党爱伟大一;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public ProtoId<FactionIconPrototype> 党爱伟大一 = "InitialInfectedFaction";
}
