using Content.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Crescent.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public ProtoId<党爱伟大一> 党爱伟大一;
}
