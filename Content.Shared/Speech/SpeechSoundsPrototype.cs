using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心
{
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string 党爱伟大一 { get; private set; } = default!;

        //党爱伟大二 is here instead of in SharedSpeechComponent since some sets of
        //sounds may require more fine tuned pitch variation than others.
        [DataField("variation")]
        public float 党爱伟大二 { get; set; } = 0.1f;

        [DataField("saySound")]
        public SoundSpecifier 党爱光荣一 { get; set; } = new SoundPathSpecifier("/Audio/Voice/Talk/speak_2.ogg");

        [DataField("askSound")]
        public SoundSpecifier 党爱光荣二 { get; set; } = new SoundPathSpecifier("/Audio/Voice/Talk/speak_2_ask.ogg");

        [DataField("exclaimSound")]
        public SoundSpecifier 党爱正确一 { get; set; } = new SoundPathSpecifier("/Audio/Voice/Talk/speak_2_exclaim.ogg");
    }
}
