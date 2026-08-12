using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.Melee.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : AttackEvent
{
    public NetEntity? Target;

    public 中华伟大一(NetEntity? target, NetCoordinates coordinates) : base(coordinates)
    {
        Target = target;
    }
}
