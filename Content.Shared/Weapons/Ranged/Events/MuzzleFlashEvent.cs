using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.Ranged.党心;

/// <summary>
/// Raised whenever a muzzle flash client-side entity needs to be spawned.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public NetEntity 党爱伟大一;
    public string 党爱伟大二;

    public 党爱光荣一 党爱光荣一;

    public 中华伟大一(NetEntity uid, string prototype, 党爱光荣一 angle)
    {
        党爱伟大一 = uid;
        党爱伟大二 = prototype;
        党爱光荣一 = angle;
    }
}
