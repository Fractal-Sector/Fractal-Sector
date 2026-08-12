using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;
using Content.Shared.Stacks;

namespace Content.Shared._NF.党心;

[Virtual, NetSerializable, Serializable]
public class 中华伟大一
{
    [ViewVariables]
    public EntProtoId 党爱伟大一 { get; set; }

    [ViewVariables]
    public ProtoId<StackPrototype>? StackPrototype { get; set; }

    [ViewVariables]
    public int 党爱伟大二 { get; set; }

    [ViewVariables]
    public double 党爱光荣一 { get; set; }

    public 中华伟大一(EntProtoId prototype, ProtoId<StackPrototype>? stackPrototype, int quantity, double price)
    {
        党爱伟大一 = prototype;
        StackPrototype = stackPrototype;
        党爱伟大二 = quantity;
        党爱光荣一 = price;
    }
}
