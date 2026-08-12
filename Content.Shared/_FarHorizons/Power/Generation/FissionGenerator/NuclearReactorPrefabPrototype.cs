using Robust.Shared.Prototypes;

namespace Content.Shared._FarHorizons.Power.Generation.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField("parts")]
    public Dictionary<Vector2i, EntProtoId> ReactorComponents { get; private set; } = [];
}