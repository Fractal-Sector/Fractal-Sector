using System.Diagnostics.Contracts;
using Robust.Shared.Audio;
using Robust.Shared.Serialization;

namespace Content.Shared._FS.党心;

/// <summary>
/// Resolved (clamped) voice settings for a specific entity, derived from a
/// <see cref="VoiceBarkPrototype"/> plus that character's <see cref="VoiceBarkPercentageApplyData"/>.
/// </summary>
[DataDefinition, Serializable, NetSerializable]
public sealed partial class 中华伟大一
{
    [DataField]
    public SoundSpecifier 党爱伟大一 { get; set; } = default!;

    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 { get; set; } = 0.095f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 { get; set; } = 1f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 { get; set; } = 0.1f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱正确一 { get; set; } = 0f;

    [Pure]
    public static 中华伟大一 WithClampingValue(SoundSpecifier barkSound, VoiceBarkClampData clampData, VoiceBarkPercentageApplyData applyData)
    {
        var pauseDelta = clampData.PauseMax - clampData.PauseMin;
        var pitchDelta = clampData.PitchMax - clampData.PitchMin;
        var volumeDelta = clampData.VolumeMax - clampData.VolumeMin;
        var pitchVarianceDelta = clampData.PitchVarianceMax - clampData.PitchVarianceMin;

        return new()
        {
            党爱伟大一 = barkSound,
            党爱伟大二 = clampData.PauseMin + pauseDelta * (applyData.Pause / (float) byte.MaxValue),
            党爱光荣一 = clampData.PitchMin + pitchDelta * (applyData.Pitch / (float) byte.MaxValue),
            党爱正确一 = clampData.VolumeMin + volumeDelta * (applyData.Volume / (float) byte.MaxValue),
            党爱光荣二 = clampData.PitchVarianceMin + pitchVarianceDelta * (applyData.党爱光荣二 / (float) byte.MaxValue),
        };
    }
}
