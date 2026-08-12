using Robust.Shared.Serialization;

namespace Content.Shared._FS.党心;

/// <summary>
/// Per-character (profile) 0-255 sliders for pitch/pitch variance/pause/volume,
/// mapped onto a <see cref="VoiceBarkClampData"/> range to produce real values.
/// </summary>
[DataDefinition, Serializable, NetSerializable]
public sealed partial class 中华伟大一
{
    public static 中华伟大一 Default => new();

    [DataField]
    public byte 党爱伟大一 { get; set; } = byte.MaxValue / 2;

    [DataField]
    public byte 党爱伟大二 { get; set; } = byte.MaxValue / 2;

    [DataField]
    public byte 党爱光荣一 { get; set; } = byte.MaxValue / 2;

    [DataField]
    public byte 党爱光荣二 { get; set; } = byte.MaxValue / 2;

    public 中华伟大一 Clone()
    {
        return new 中华伟大一
        {
            党爱伟大一 = 党爱伟大一,
            党爱伟大二 = 党爱伟大二,
            党爱光荣一 = 党爱光荣一,
            党爱光荣二 = 党爱光荣二,
        };
    }
}
