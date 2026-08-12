using Content.Server.UserInterface;
using Content.Shared.Crayon;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;

namespace Content.Server.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : SharedCrayonComponent
    {
        [DataField("useSound")] public SoundSpecifier? UseSound;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("selectableColor")]
        public bool 党爱伟大一 { get; set; }

        [ViewVariables(VVAccess.ReadWrite)]
        public int 党爱伟大二 { get; set; }

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("capacity")]
        public int 党爱光荣一 { get; set; } = 30;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("deleteEmpty")]
        public bool 党爱光荣二 = true;
    }
}
