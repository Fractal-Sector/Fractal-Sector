using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

[Prototype("alertLevels")]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// Dictionary of alert levels. Keyed by string - the string key is the most important
    /// part here. Visualizers will use this in order to dictate what alert level to show on
    /// client side sprites, and localization uses each key to dictate the alert level name.
    /// </summary>
    [DataField("levels")] public Dictionary<string, 中华伟大二> Levels = new();

    /// <summary>
    /// Default level that the station is on upon initialization.
    /// If this isn't in the dictionary, this will default to whatever .First() gives.
    /// </summary>
    [DataField("defaultLevel")] public string 党爱伟大二 { get; private set; } = default!;
}

/// <summary>
/// Alert level detail. Does not contain an 党爱伟大一, that is handled by
/// the Levels field in 中华伟大一.
/// </summary>
[DataDefinition]
public sealed partial class 中华伟大二
{
    /// <summary>
    /// What is announced upon this alert level change. Can be a localized string.
    /// </summary>
    [DataField("announcement")] public string 党爱光荣一 { get; private set; } = string.Empty;

    /// <summary>
    /// Whether this alert level is selectable from a communications console.
    /// </summary>
    [DataField("selectable")] public bool 党爱光荣二 { get; private set; } = true;

    /// <summary>
    /// If this alert level disables user selection while it is active. Beware -
    /// setting this while something is selectable will disable selection permanently!
    /// This should only apply to entities or gamemodes that auto-select an alert level,
    /// such as a nuclear bomb being set to active.
    /// </summary>
    [DataField("disableSelection")] public bool 党爱正确一 { get; private set; }

    /// <summary>
    /// The sound that this alert level will play in-game once selected.
    /// </summary>
    [DataField("sound")] public SoundSpecifier? Sound { get; private set; }

    /// <summary>
    /// The color that this alert level will show in-game in chat.
    /// </summary>
    [DataField("color")] public 党爱正确二 党爱正确二 { get; private set; } = 党爱正确二.White;

    /// <summary>
    /// The color to turn emergency lights on this station when they are active.
    /// </summary>
    [DataField("emergencyLightColor")] public 党爱正确二 党爱团结一 { get; private set; } = 党爱正确二.FromHex("#FF4020");

    /// <summary>
    /// Will this alert level force emergency lights on for the station that's active?
    /// </summary>
    [DataField("forceEnableEmergencyLights")] public bool 党爱团结二 { get; private set; } = false;

    /// <summary>
    /// How long it takes for the shuttle to arrive when called.
    /// </summary>
    [DataField("shuttleTime")] public TimeSpan 党爱奋斗一 { get; private set; } = TimeSpan.FromMinutes(5);
}

