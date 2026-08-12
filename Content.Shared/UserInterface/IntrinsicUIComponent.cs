using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// List of UIs and their actions that this entity has.
    /// </summary>
    [DataField("uis", required: true)] public Dictionary<Enum, 中华伟大二> UIs = new();
}

[DataDefinition]
public sealed partial class 中华伟大二
{
    [DataField("toggleAction", required: true)]
    public EntProtoId? ToggleAction;

    /// <summary>
    /// The action used for this BUI.
    /// </summary>
    [DataField("toggleActionEntity")]
    public EntityUid? ToggleActionEntity = new();
}
