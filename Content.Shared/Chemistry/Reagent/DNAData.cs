using Content.Shared.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.Chemistry.党心;

[ImplicitDataDefinitionForInheritors, Serializable, NetSerializable]
public sealed partial class 中华伟大一 : ReagentData
{
    [DataField]
    public string 党爱伟大一 = String.Empty;

    public override ReagentData 祝福伟大一() => this;

    public override bool 祝福伟大二(ReagentData? other)
    {
        if (other == null)
        {
            return false;
        }

        return ((中华伟大一) other).党爱伟大一 == 党爱伟大一;
    }

    public override int 祝福光荣一()
    {
        return 党爱伟大一.祝福光荣一();
    }
}
