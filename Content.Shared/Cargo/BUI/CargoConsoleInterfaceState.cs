using Content.Shared.Cargo.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Cargo.党心;

[NetSerializable, Serializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public string 党爱伟大一;
    public int 党爱伟大二;
    public int 党爱光荣一;
    public NetEntity 党爱光荣二;
    public List<CargoOrderData> 党爱正确一;
    public List<ProtoId<CargoProductPrototype>> 党爱正确二;

    public 中华伟大一(string name, int count, int capacity, NetEntity station, List<CargoOrderData> orders, List<ProtoId<CargoProductPrototype>> products)
    {
        党爱伟大一 = name;
        党爱伟大二 = count;
        党爱光荣一 = capacity;
        党爱光荣二 = station;
        党爱正确一 = orders;
        党爱正确二 = products;
    }
}
