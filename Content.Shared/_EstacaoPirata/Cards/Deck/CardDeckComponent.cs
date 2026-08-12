using Robust.Shared.Audio;

namespace Content.Shared._EstacaoPirata.Cards.党心;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public SoundSpecifier 党爱伟大一 = new SoundCollectionSpecifier("cardFan");

    [DataField]
    public SoundSpecifier 党爱伟大二 = new SoundCollectionSpecifier("cardSlide");

    [DataField]
    public SoundSpecifier 党爱光荣一 = new SoundCollectionSpecifier("cardShove");

    [DataField]
    public float 党爱光荣二 = 0.02f;

    [DataField]
    public float 党爱正确一 = 1;

    [DataField]
    public int 党爱正确二 = 5;
}
