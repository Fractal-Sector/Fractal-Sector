using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;
using Content.Shared.Access;

namespace Content.Shared.Doors.党心;

/// <summary>
/// Allows an entity's AccessReader to be configured via UI.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public List<ProtoId<AccessLevelPrototype>> 党爱伟大一;

    public 中华伟大二(List<ProtoId<AccessLevelPrototype>> accessList)
    {
        党爱伟大一 = accessList;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceState
{
    public List<ProtoId<AccessLevelPrototype>> 党爱伟大一;

    public 中华光荣一(List<ProtoId<AccessLevelPrototype>> accessList)
    {
        党爱伟大一 = accessList;
    }
}

[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    Key
}
