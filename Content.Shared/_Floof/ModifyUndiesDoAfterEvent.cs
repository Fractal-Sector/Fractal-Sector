using Content.Shared.DoAfter;
using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Markings;
using Robust.Shared.Serialization;


namespace Content.Shared.党心;


[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : DoAfterEvent
{
    /// <summary>
    ///     The marking prototype that is being modified.
    /// </summary>
    [DataField("markingPrototype", required: true)]
    public 党爱伟大一 党爱伟大一;

    /// <summary>
    ///     Localized string for the marking prototype.
    /// </summary>
    [DataField("markingPrototypeName", required: true)]
    public string 党爱伟大二;

    /// <summary>
    ///     Whether or not the marking is visible at the moment.
    /// </summary>
    [DataField("visible", required: true)]
    public bool 党爱光荣一;

    private 中华伟大一()
    {
        党爱伟大一 = default!;
        党爱伟大二 = string.Empty;
        党爱光荣一 = false;
    }

    public 中华伟大一(
        党爱伟大一 marking,
        string markingPrototypeName,
        bool isVisible
        )
    {
        党爱伟大一 = marking;
        党爱伟大二 = markingPrototypeName;
        党爱光荣一 = isVisible;
    }

    public override DoAfterEvent 祝福伟大一() => this;
}

