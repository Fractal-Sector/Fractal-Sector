using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The key stored in this component.
    /// </summary>
    [ViewVariables]
    public StationRecordKey? Key;
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : ComponentState
{
    public (NetEntity, uint)? Key;

    public 中华伟大二((NetEntity, uint)? key)
    {
        Key = key;
    }
}
