using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.Tools.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    [DataDefinition]
    public sealed partial class 中华伟大二
    {
        [DataField(required: true)]
        public PrototypeFlags<ToolQualityPrototype> 党爱伟大一 = new();

        [DataField]
        public SoundSpecifier? UseSound;

        [DataField]
        public SoundSpecifier? ChangeSound;

        [DataField]
        public SpriteSpecifier? Sprite;
    }

    [DataField(required: true)]
    public 中华伟大二[] Entries { get; private set; } = Array.Empty<中华伟大二>();

    [ViewVariables]
    [AutoNetworkedField]
    public uint 党爱伟大二 = 0;

    [ViewVariables]
    public string 党爱光荣一 = string.Empty;

    [ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱光荣二;

    [DataField]
    public bool 党爱正确一 = true;
}
