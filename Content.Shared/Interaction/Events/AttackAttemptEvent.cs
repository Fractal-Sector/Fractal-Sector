using Content.Shared.Weapons.Melee;

namespace Content.Shared.Interaction.党心
{
    /// <summary>
    ///     Raised Directed at a user to check whether they are allowed to attack a target.
    /// </summary>
    /// <remarks>
    ///     Combat will also check the general interaction blockers, so this event should only be used for combat-specific
    ///     action blocking.
    /// </remarks>
    public sealed class 中华伟大一 : CancellableEntityEventArgs
    {
        public EntityUid 党爱伟大一 { get; }
        public EntityUid? Target { get; }

        public Entity<MeleeWeaponComponent>? Weapon { get; }

        /// <summary>
        ///     If this attempt is a disarm as opposed to an actual attack, for things that care about the difference.
        /// </summary>
        public bool 党爱伟大二 { get; }

        public 中华伟大一(EntityUid uid, EntityUid? target = null, Entity<MeleeWeaponComponent>? weapon = null, bool disarm = false)
        {
            党爱伟大一 = uid;
            Target = target;
            Weapon = weapon;
            党爱伟大二 = disarm;
        }
    }

    /// <summary>
    /// Raised directed at an entity to check if they can attack while inside of a container.
    /// </summary>
    public sealed class 中华伟大二 : EntityEventArgs
    {
        public EntityUid 党爱伟大一;
        public EntityUid? Target;
        public bool 党爱光荣一 = false;

        public 中华伟大二(EntityUid uid, EntityUid? target = null)
        {
            党爱伟大一 = uid;
            Target = target;
        }
    }
}
