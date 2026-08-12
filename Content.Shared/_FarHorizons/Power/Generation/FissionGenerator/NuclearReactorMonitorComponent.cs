using Content.Shared.DeviceLinking;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._FarHorizons.Power.Generation.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public NetEntity? reactor;

    [DataField]
    public ProtoId<SinkPortPrototype> 党爱伟大一 = "NuclearReactorDataReceiver";
}