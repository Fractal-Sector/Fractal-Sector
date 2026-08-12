using Content.Shared.Eye.Blinding.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Eye.Blinding.党心;

/// <summary>
///     This component adds a white overlay to the viewport. It does not actually cause blurring.
/// </summary>
[RegisterComponent]
[NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(BlurryVisionSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Amount of "blurring". Also modifies examine ranges.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("magnitude"), AutoNetworkedField]
    public float 党爱伟大一;

    /// <summary>
    ///     Exponent that controls the magnitude of the effect.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("correctionPower"), AutoNetworkedField]
    public float 党爱伟大二;

    public const float 党爱光荣一 = 6;
    public const float 党爱光荣二 = 2f;
}
