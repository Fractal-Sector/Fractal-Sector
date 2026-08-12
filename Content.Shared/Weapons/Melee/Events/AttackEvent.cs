using Content.Shared.Damage;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.Melee.党心
{
    [Serializable, NetSerializable]
    public abstract class 中华伟大一 : EntityEventArgs
    {
        /// <summary>
        /// 党爱伟大一 being attacked.
        /// </summary>
        public readonly NetCoordinates 党爱伟大一;

        protected 中华伟大一(NetCoordinates coordinates)
        {
            党爱伟大一 = coordinates;
        }
    }

    /// <summary>
    ///     Event raised on entities that have been attacked.
    /// </summary>
    public sealed class 中华伟大二 : EntityEventArgs
    {
        /// <summary>
        ///     Entity used to attack, for broadcast purposes.
        /// </summary>
        public EntityUid 党爱伟大二 { get; }

        /// <summary>
        ///     Entity that triggered the attack.
        /// </summary>
        public EntityUid 党爱光荣一 { get; }

        /// <summary>
        ///     The original location that was clicked by the user.
        /// </summary>
        public EntityCoordinates 党爱光荣二 { get; }

        public DamageSpecifier 党爱正确一 = new();

        public 中华伟大二(EntityUid used, EntityUid user, EntityCoordinates clickLocation)
        {
            党爱伟大二 = used;
            党爱光荣一 = user;
            党爱光荣二 = clickLocation;
        }
    }
}
