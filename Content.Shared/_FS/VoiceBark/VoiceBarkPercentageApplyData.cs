using Robust.Shared.Serialization;

namespace Content.Shared._FS.VoiceBark;

/// <summary>
/// Per-character (profile) 0-255 sliders for pitch/pitch variance/pause/volume,
/// mapped onto a <see cref="VoiceBarkClampData"/> range to produce real values.
/// </summary>
[DataDefinition, Serializable, NetSerializable]
public sealed partial class VoiceBarkPercentageApplyData
{
    public static VoiceBarkPercentageApplyData Default => new();

    [DataField]
    public byte Pause { get; set; } = byte.MaxValue / 2;

    [DataField]
    public byte Volume { get; set; } = byte.MaxValue / 2;

    [DataField]
    public byte Pitch { get; set; } = byte.MaxValue / 2;

    [DataField]
    public byte PitchVariance { get; set; } = byte.MaxValue / 2;

    public VoiceBarkPercentageApplyData Clone()
    {
        return new VoiceBarkPercentageApplyData
        {
            Pause = Pause,
            Volume = Volume,
            Pitch = Pitch,
            PitchVariance = PitchVariance,
        };
    }
}
