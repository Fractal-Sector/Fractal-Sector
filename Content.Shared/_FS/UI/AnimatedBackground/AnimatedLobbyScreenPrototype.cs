using Robust.Shared.Prototypes;

namespace Content.Shared._FS.UI.AnimatedBackground;

[Prototype]
public sealed partial class AnimatedLobbyScreenPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; set; } = default!;

    [DataField(required: true)]
    public string Path = default!;
}
