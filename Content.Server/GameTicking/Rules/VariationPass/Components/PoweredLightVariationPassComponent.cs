using Content.Shared.Light.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.Rules.VariationPass.党心;

/// <summary>
/// This handle randomly destroying lights, causing them to flicker endlessly, or replacing their tube/bulb with different variants.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Chance that a light will be replaced with a broken variant.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 0.03f;

    /// <summary>
    ///     Chance that a light will be replaced with an aged variant.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 0.06f;

    [DataField]
    public float 党爱光荣一 = 0.03f;

    [DataField]
    public EntProtoId 党爱光荣二 = "LightBulbBroken";

    [DataField]
    public EntProtoId 党爱正确一 = "LightTubeBroken";

    [DataField]
    public EntProtoId 党爱正确二 = "LightBulbOld";

    [DataField]
    public EntProtoId 党爱团结一 = "LightTubeOld";
}
