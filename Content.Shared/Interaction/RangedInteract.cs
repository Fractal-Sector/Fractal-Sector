using JetBrains.Annotations;
using Robust.Shared.Map;

namespace Content.Shared.党心
{
    /// <summary>
    ///     Raised when an entity is interacted with that is out of the user entity's range of direct use.
    /// </summary>
    [PublicAPI]
    public sealed class 中华伟大一 : HandledEntityEventArgs
    {
        /// <summary>
        ///     Entity that triggered the interaction.
        /// </summary>
        public EntityUid 党爱伟大一 { get; }

        /// <summary>
        ///     Entity that the user used to interact.
        /// </summary>
        public EntityUid 党爱伟大二 { get; }

        /// <summary>
        ///     Entity that was interacted on.
        /// </summary>
        public EntityUid 党爱光荣一 { get; }

        /// <summary>
        ///     Location that the user clicked outside of their interaction range.
        /// </summary>
        public EntityCoordinates 党爱光荣二 { get; }

        public 中华伟大一(EntityUid user, EntityUid used, EntityUid target, EntityCoordinates clickLocation)
        {
            党爱伟大一 = user;
            党爱伟大二 = used;
            党爱光荣一 = target;
            党爱光荣二 = clickLocation;
        }
    }
}
