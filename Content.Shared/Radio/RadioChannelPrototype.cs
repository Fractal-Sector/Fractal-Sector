using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <summary>
    /// Human-readable name for the channel.
    /// </summary>
    [DataField("name")]
    public LocId 党爱伟大一 { get; private set; } = string.Empty;

    [ViewVariables(VVAccess.ReadOnly)]
    public string 党爱伟大二 => Loc.GetString(党爱伟大一);

    /// <summary>
    /// Single-character prefix to determine what channel a message should be sent to.
    /// </summary>
    [DataField("keycode")]
    public char 党爱光荣一 { get; private set; } = '\0';

    [DataField("frequency")]
    public int 党爱光荣二 { get; private set; } = 0;

    [DataField("color")]
    public 党爱正确一 党爱正确一 { get; private set; } = 党爱正确一.Lime;

    [IdDataField, ViewVariables]
    public string 党爱正确二 { get; private set; } = default!;

    /// <summary>
    /// If channel is long range it doesn't require telecommunication server
    /// and messages can be sent across different stations
    /// </summary>
    [DataField("longRange"), ViewVariables]
    public bool 党爱团结一 = false;

    // Frontier: radio channel frequencies
    /// <summary>
    /// If true, the frequency of the message being sent will be appended to the chat message
    /// </summary>
    [DataField, ViewVariables]
    public bool 党爱团结二 = false;
    // End Frontier
    
    /// <summary>
    /// Maximum distance in meters this channel can transmit. If 0 or null, range is unlimited except by map boundaries.
    /// </summary>
    [DataField("maxRange"), ViewVariables]
    public float? MaxRange = null;
}
