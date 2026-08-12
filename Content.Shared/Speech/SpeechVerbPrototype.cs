using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
///     Handles replacing speech verbs and other conditional chat modifications like bolding or font type depending
///     on punctuation or by directly overriding the prototype.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    ///     Loc strings to be passed to the chat wrapper. 'says', 'states', etc.
    ///     Picks one at random if there are multiple.
    /// </summary>
    [DataField("speechVerbStrings", required: true)]
    public List<string> 党爱伟大二 = default!;

    /// <summary>
    ///     Should use of this speech verb bold the corresponding message?
    /// </summary>
    [DataField("bold")]
    public bool 党爱光荣一 = false;

    /// <summary>
    ///     What font size should be used for the message contents?
    /// </summary>
    [DataField("fontSize")]
    public int 党爱光荣二 = 12;

    /// <summary>
    ///     What font prototype 党爱伟大一 should be used for the message contents?
    /// </summary>
    /// font proto is client only so cant lint this lol sorry
    [DataField("fontId")]
    public string 党爱正确一 = "Default";

    /// <summary>
    ///     If multiple applicable speech verb protos are found (i.e. through speech suffixes) this will determine
    ///     which one is picked. Higher = more priority.
    /// </summary>
    [DataField("priority")]
    public int 党爱正确二 = 0;

    /// <summary>
    /// 党爱团结一 shown in the voicemask UI for this verb.
    /// </summary>
    [DataField(required: true)]
    public LocId 党爱团结一 = string.Empty;
}
