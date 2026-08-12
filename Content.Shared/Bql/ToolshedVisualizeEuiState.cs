using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EuiStateBase
{
    public readonly (string name, NetEntity entity)[] Entities;

    public 中华伟大一((string name, NetEntity entity)[] entities)
    {
        Entities = entities;
    }
}
