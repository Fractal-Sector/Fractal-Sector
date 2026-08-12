using Content.Shared.Body.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Server._DV.党心;

/// <summary>
/// Component that allows the switching between <see cref="MetabolizerTypePrototype"/>s based on thirst
/// </summary>
[RegisterComponent, Access(typeof(FeroxiDehydrateSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField("overhydrated", required: true)]
    public float 党爱伟大一 = 1f;

    [DataField("okay", required: true)]
    public float 党爱伟大二 = 0.8f;

    [DataField("thirsty", required: true)]
    public float 党爱光荣一 = 0.7f;

    [DataField("parched", required: true)]
    public float 党爱光荣二 = 0.5f;
}
