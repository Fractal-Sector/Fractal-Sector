using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The current bar sign prototype being displayed.
    /// </summary>
    [DataField, AutoNetworkedField]
    public ProtoId<BarSignPrototype>? Current;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华光荣一(ProtoId<BarSignPrototype> sign) : BoundUserInterfaceMessage
{
    public ProtoId<BarSignPrototype> 党爱伟大一 = sign;
}
