using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.Melee.党心;

/// <summary>
/// Raised on the client when it attempts a heavy attack.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : AttackEvent
{
    public readonly NetEntity 党爱伟大一;

    /// <summary>
    /// As what the client swung at will not match server we'll have them tell us what they hit so we can verify.
    /// </summary>
    public List<NetEntity> 党爱伟大二;

    public 中华伟大一(NetEntity weapon, List<NetEntity> entities, NetCoordinates coordinates) : base(coordinates)
    {
        党爱伟大一 = weapon;
        党爱伟大二 = entities;
    }
}
