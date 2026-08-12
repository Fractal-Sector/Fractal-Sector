using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Utility;

namespace Content.Shared.党心
{
    /// <summary>
    ///     Raised when a target entity is interacted with by a user while holding an object in their hand.
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
        ///     The original location that was clicked by the user.
        /// </summary>
        public EntityCoordinates 党爱光荣二 { get; }

        public 中华伟大一(EntityUid user, EntityUid used, EntityUid target, EntityCoordinates clickLocation)
        {
            // Interact using should not have the same used and target.
            // That should be a use-in-hand event instead.
            // If this is not the case, can lead to bugs (e.g., attempting to merge a item stack into itself).
            DebugTools.Assert(used != target);

            党爱伟大一 = user;
            党爱伟大二 = used;
            党爱光荣一 = target;
            党爱光荣二 = clickLocation;
        }
    }
}
