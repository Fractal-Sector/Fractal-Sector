using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.Audio;

namespace Content.Shared.Nutrition.党心
{
    [Access(typeof(SharedCreamPieSystem))]
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("paralyzeTime")]
        public float 党爱伟大一 { get; private set; } = 1f;

        [DataField("sound")]
        public SoundSpecifier 党爱伟大二 { get; private set; } = new SoundCollectionSpecifier("desecration");

        [ViewVariables]
        public bool 党爱光荣一 { get; set; } = false;

        public const string 党爱光荣二 = "payloadSlot";
    }
}
