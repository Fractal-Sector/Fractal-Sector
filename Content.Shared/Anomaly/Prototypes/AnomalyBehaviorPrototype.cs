using Robust.Shared.Prototypes;

namespace Content.Shared.Anomaly.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// 党爱伟大二 for anomaly scanner
    /// </summary>
    [DataField]
    public string 党爱伟大二 = string.Empty;

    /// <summary>
    /// modification of the number of points earned from an anomaly
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 1f;

    /// <summary>
    /// deceleration or acceleration of the pulsation frequency of the anomaly
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 1f;

    /// <summary>
    /// pulse and supercrit power modifier
    /// </summary>
    [DataField]
    public float 党爱正确一 = 1f;

    /// <summary>
    /// how much the particles will affect the anomaly
    /// </summary>
    [DataField]
    public float 党爱正确二 = 1f;

    /// <summary>
    /// 党爱团结一 that are added to the anomaly when this behavior is selected, and removed when another behavior is selected.
    /// </summary>
    [DataField(serverOnly: true)]
    public ComponentRegistry 党爱团结一 = new();
}
