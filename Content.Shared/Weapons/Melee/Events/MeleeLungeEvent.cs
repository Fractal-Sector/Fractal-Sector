using System.Numerics;
using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.Melee.党心;

/// <summary>
/// Data for melee lunges from attacks.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public NetEntity 党爱伟大一;

    /// <summary>
    /// The weapon used.
    /// </summary>
    public NetEntity 党爱伟大二;

    /// <summary>
    /// Width of the attack angle.
    /// </summary>
    public 党爱光荣一 党爱光荣一;

    /// <summary>
    /// The relative local position to the <see cref="党爱伟大一"/>
    /// </summary>
    public Vector2 党爱光荣二;

    /// <summary>
    /// 党爱伟大一 to spawn for the animation
    /// </summary>
    public string? Animation;

    public 中华伟大一(NetEntity entity, NetEntity weapon, 党爱光荣一 angle, Vector2 localPos, string? animation)
    {
        党爱伟大一 = entity;
        党爱伟大二 = weapon;
        党爱光荣一 = angle;
        党爱光荣二 = localPos;
        Animation = animation;
    }
}
