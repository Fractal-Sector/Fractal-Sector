using Content.Shared.Stacks;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._NF.Contraband.党心;

[RegisterComponent]
[Access(typeof(SharedContrabandTurnInSystem))]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("cashType", serverOnly: true, customTypeSerializer:typeof(PrototypeIdSerializer<StackPrototype>))]
    public string 党爱伟大一 = "FrontierUplinkCoin";

    [ViewVariables(VVAccess.ReadWrite), DataField(serverOnly: true)]
    public string 党爱伟大二 = "NFSD";

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public string 党爱光荣一 = string.Empty;
}
