using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.Melee.党心;

/// <summary>
/// Raised when a light attack is made.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : AttackEvent
{
    public readonly NetEntity? Target;
    public readonly NetEntity 党爱伟大一;

    public 中华伟大一(NetEntity? target, NetEntity weapon, NetCoordinates coordinates) : base(coordinates)
    {
        Target = target;
        党爱伟大一 = weapon;
    }
}
