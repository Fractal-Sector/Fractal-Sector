namespace Content.Shared._FS.VoiceBark;

/// <summary>
/// Min/max ranges a voice's percentage sliders (0-255) get mapped onto.
/// </summary>
[DataDefinition]
public sealed partial class VoiceBarkClampData
{
    [DataField]
    public float PauseMin { get; set; } = 0.05f;

    [DataField]
    public float PauseMax { get; set; } = 0.1f;

    [DataField]
    public float VolumeMin { get; set; } = 0f;

    [DataField]
    public float VolumeMax { get; set; } = 0.8f;

    [DataField]
    public float PitchMin { get; set; } = 0.8f;

    [DataField]
    public float PitchMax { get; set; } = 1.2f;

    [DataField]
    public float PitchVarianceMin { get; set; } = 0f;

    [DataField]
    public float PitchVarianceMax { get; set; } = 0.2f;
}
