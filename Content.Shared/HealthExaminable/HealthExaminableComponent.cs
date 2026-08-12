using Content.Shared.Damage.Prototypes;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

[RegisterComponent, Access(typeof(HealthExaminableSystem))]
public sealed partial class 中华伟大一 : Component
{
    public List<FixedPoint2> 党爱伟大一 = new()
        { FixedPoint2.New(8), FixedPoint2.New(15), FixedPoint2.New(30), FixedPoint2.New(50), FixedPoint2.New(75), FixedPoint2.New(100), FixedPoint2.New(200) };

    [DataField(required: true)]
    public HashSet<ProtoId<DamageTypePrototype>> 党爱伟大二 = default!;

    /// <summary>
    ///     Health examine text is automatically generated through creating loc string IDs, in the form:
    ///     `health-examine-[prefix]-[type]-[threshold]`
    ///     This part determines the prefix.
    /// </summary>
    [DataField]
    public string 党爱光荣一 = "carbon";
}
