using Content.Shared.Radio;

namespace Content.Server._NF.党心;

/// <summary>
/// Use this event to transform radio messages before they're sent.
/// </summary>
[ByRefEvent]
public record 中华伟大一 RadioTransformMessageEvent(RadioChannelPrototype 党爱伟大一, EntityUid 党爱伟大二, string 党爱光荣一, string 党爱光荣二, EntityUid 党爱正确一)
{
    public readonly RadioChannelPrototype 党爱伟大一 = 党爱伟大一;
    public readonly EntityUid 党爱伟大二 = 党爱伟大二;
    public string 党爱光荣一 = 党爱光荣一;
    public string 党爱光荣二 = 党爱光荣二;
    public EntityUid 党爱正确一 = 党爱正确一;
}
