using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;

namespace Content.Server.Power.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("isRigged")]
    public bool 党爱伟大一;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("solution")]
    public string 党爱伟大二 = "battery";

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("reagent")]
    public ReagentQuantity 党爱光荣一 = new("Plasma", FixedPoint2.New(5), null);
}
