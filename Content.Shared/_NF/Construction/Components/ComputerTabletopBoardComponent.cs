using Robust.Shared.Prototypes;

namespace Content.Shared._NF.Construction.党心;

/// <summary>
/// Used for construction graphs in building tabletop computers.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public EntProtoId? Prototype { get; private set; }
}
