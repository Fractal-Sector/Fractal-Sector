using System.Diagnostics.Contracts;
using Robust.Shared.Audio;
using Robust.Shared.Serialization;

namespace Content.Shared._FS.VoiceBark;

/// <summary>
/// Resolved (clamped) voice settings for a specific entity, derived from a
/// <see cref="VoiceBarkPrototype"/> plus that character's <see cref="VoiceBarkPercentageApplyData"/>.
/// </summary>
[DataDefinition, Serializable, NetSerializable]
public sealed partial class VoiceBarkVoiceData
{
    [DataField]
    public SoundSpecifier BarkSound { get; set; } = default!;

    [ViewVariables(VVAccess.ReadWrite)]
    public float PauseAverage { get; set; } = 0.095f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float PitchAverage { get; set; } = 1f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float PitchVariance { get; set; } = 0.1f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float VolumeAverage { get; set; } = 0f;

    [Pure]
    public static VoiceBarkVoiceData WithClampingValue(SoundSpecifier barkSound, VoiceBarkClampData clampData, VoiceBarkPercentageApplyData applyData)
    {
        var pauseDelta = clampData.PauseMax - clampData.PauseMin;
        var pitchDelta = clampData.PitchMax - clampData.PitchMin;
        var volumeDelta = clampData.VolumeMax - clampData.VolumeMin;
        var pitchVarianceDelta = clampData.PitchVarianceMax - clampData.PitchVarianceMin;

        return new()
        {
            BarkSound = barkSound,
            PauseAverage = clampData.PauseMin + pauseDelta * (applyData.Pause / (float) byte.MaxValue),
            PitchAverage = clampData.PitchMin + pitchDelta * (applyData.Pitch / (float) byte.MaxValue),
            VolumeAverage = clampData.VolumeMin + volumeDelta * (applyData.Volume / (float) byte.MaxValue),
            PitchVariance = clampData.PitchVarianceMin + pitchVarianceDelta * (applyData.PitchVariance / (float) byte.MaxValue),
        };
    }
}
