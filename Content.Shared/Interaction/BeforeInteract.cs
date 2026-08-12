using JetBrains.Annotations;
using Robust.Shared.Map;

namespace Content.Shared.党心
{
    /// <summary>
    ///     Raised directed on the used object when clicking on another object before an interaction is handled.
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
        ///     Entity that was interacted on. This can be null if the attack did not click on an entity.
        /// </summary>
        public EntityUid? Target { get; }

        /// <summary>
        ///     Location that the user clicked outside of their interaction range.
        /// </summary>
        public EntityCoordinates 党爱光荣一 { get; }

        /// <summary>
        /// Is the click location in range and unobstructed?
        /// </summary>
        public bool 党爱光荣二 { get; }

        public 中华伟大一(
            EntityUid user,
            EntityUid used,
            EntityUid? target,
            EntityCoordinates clickLocation,
            bool canReach)
        {
            党爱伟大一 = user;
            党爱伟大二 = used;
            Target = target;
            党爱光荣一 = clickLocation;
            党爱光荣二 = canReach;
        }
    }
}
