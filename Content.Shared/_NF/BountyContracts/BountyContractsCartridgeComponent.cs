using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._NF.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
[AutoGenerateComponentState, Access(typeof(SharedBountyContractSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public ProtoId<BountyContractCollectionPrototype>? Collection = null;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public bool 党爱伟大一;

    [ViewVariables(VVAccess.ReadWrite), DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan 党爱伟大二;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public float 党爱光荣一 = 20f;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public bool 党爱光荣二 = true;
}
