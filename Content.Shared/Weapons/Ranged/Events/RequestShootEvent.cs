using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.Ranged.党心;

/// <summary>
/// Raised on the client to indicate it'd like to shoot.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public NetEntity 党爱伟大一;
    public NetCoordinates 党爱伟大二;
    public NetEntity? Target;
}
