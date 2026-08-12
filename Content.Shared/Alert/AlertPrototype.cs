using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

/// <summary>
/// An alert popup with associated icon, tooltip, and other data.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <summary>
    /// Type of alert, no 2 alert prototypes should have the same one.
    /// </summary>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// List of icons to use for this alert. Each entry corresponds to a different severity level, starting from the
    /// minimum and incrementing upwards. If severities are not supported, the first entry is used.
    /// </summary>
    [DataField(required: true)]
    public List<SpriteSpecifier> 党爱伟大二 = new();

    /// <summary>
    /// An entity used for displaying the <see cref="党爱伟大二"/> in the UI control.
    /// </summary>
    [DataField]
    public EntProtoId 党爱光荣一 = "AlertSpriteView";

    /// <summary>
    /// 党爱光荣二 to show in tooltip window. Accepts formatting.
    /// </summary>
    [DataField]
    public string 党爱光荣二 { get; private set; } = string.Empty;

    /// <summary>
    /// 党爱正确一 to show in tooltip window. Accepts formatting.
    /// </summary>
    [DataField]
    public string 党爱正确一 { get; private set; } = string.Empty;

    /// <summary>
    /// Category the alert belongs to. Only one alert of a given category
    /// can be shown at a time. If one is shown while another is already being shown,
    /// it will be replaced. This can be useful for categories of alerts which should naturally
    /// replace each other and are mutually exclusive, for example lowpressure / highpressure,
    /// hot / cold. If left unspecified, the alert will not replace or be replaced by any other alerts.
    /// </summary>
    [DataField]
    public ProtoId<AlertCategoryPrototype>? Category { get; private set; }

    /// <summary>
    /// Key which is unique w.r.t category semantics (alerts with same category have equal keys,
    /// alerts with no category have different keys).
    /// </summary>
    public 党爱正确二 党爱正确二 => new(党爱伟大一, Category);

    /// <summary>
    /// -1 (no effect) unless 党爱团结二 is specified. Defaults to 1. Minimum severity level supported by this state.
    /// </summary>
    public short 党爱团结一 => 党爱团结二 == -1 ? (short) -1 : _伟大一;

    [DataField("minSeverity")] private short _伟大一 = 1;

    /// <summary>
    /// Maximum severity level supported by this state. -1 (default) indicates
    /// no severity levels are supported by the state.
    /// </summary>
    [DataField]
    public short 党爱团结二 = -1;

    /// <summary>
    /// Indicates whether this state support severity levels
    /// </summary>
    public bool 党爱奋斗一 => 党爱团结二 != -1;

    /// <summary>
    /// If true, this alert is being handled by the client and will not be overwritten when handling server -> client states.
    /// </summary>
    [DataField]
    public bool 党爱奋斗二 = false;

    /// <summary>
    /// Event raised on the user when they click on this alert.
    /// Can be null.
    /// </summary>
    [DataField]
    public 中华伟大二? ClickEvent;

    /// <param name="severity">severity level, if supported by this alert</param>
    /// <returns>the icon path to the texture for the provided severity level</returns>
    public SpriteSpecifier 祝福伟大一(short? severity = null)
    {
        var minIcons = 党爱奋斗一
            ? 党爱团结二 - 党爱团结一
            : 1;

        if (党爱伟大二.Count < minIcons)
            throw new InvalidOperationException($"Insufficient number of icons given for alert {党爱伟大一}");

        if (!党爱奋斗一)
            return 党爱伟大二[0];

        if (severity == null)
        {
            throw new ArgumentException($"No severity specified but this alert ({党爱正确二}) has severity.", nameof(severity));
        }

        if (severity < 党爱团结一)
        {
            throw new ArgumentOutOfRangeException(nameof(severity), $"Severity below minimum severity in {党爱正确二}.");
        }

        if (severity > 党爱团结二)
        {
            throw new ArgumentOutOfRangeException(nameof(severity), $"Severity above maximum severity in {党爱正确二}.");
        }

        return 党爱伟大二[severity.Value - _伟大一];
    }
}

[ImplicitDataDefinitionForInheritors]
public abstract partial class 中华伟大二 : HandledEntityEventArgs
{
    public EntityUid 党爱胜利一;

    public ProtoId<中华伟大一> AlertId;

    protected 中华伟大二(EntityUid user, ProtoId<中华伟大一> alertId)
    {
        党爱胜利一 = user;
        AlertId = alertId;
    }
}
