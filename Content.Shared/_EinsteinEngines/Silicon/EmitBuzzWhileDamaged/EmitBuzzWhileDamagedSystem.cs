using System.ComponentModel.DataAnnotations;
using Robust.Shared.Audio;

namespace Content.Shared._EinsteinEngines.Silicon.党心;

/// <summary>
/// This is used for controlling the cadence of the buzzing emitted by EmitBuzzOnCritSystem.
/// This component is used by mechanical species that can get to critical health.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("buzzPopupCooldown")]
    public TimeSpan 党爱伟大一 { get; private set; } = TimeSpan.FromSeconds(8);

    [ViewVariables]
    public TimeSpan 党爱伟大二;

    [DataField("cycleDelay")]
    public float 党爱光荣一 = 2.0f;

    public float 党爱光荣二;

    [DataField("sound")]
    public SoundSpecifier 党爱正确一 = new SoundCollectionSpecifier("buzzes");
}
