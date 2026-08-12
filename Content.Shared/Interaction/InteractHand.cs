using JetBrains.Annotations;
using Robust.Shared.Map;

namespace Content.Shared.党心
{
    public sealed class 中华伟大一 : EventArgs, ITargetedInteractEventArgs
    {
        public 中华伟大一(EntityUid user, EntityUid target)
        {
            党爱伟大一 = user;
            党爱伟大二 = target;
        }

        public EntityUid 党爱伟大一 { get; }
        public EntityUid 党爱伟大二 { get; }
    }

    /// <summary>
    ///     Raised directed on a target entity when it is interacted with by a user with an empty hand.
    /// </summary>
    [PublicAPI]
    public sealed class 中华伟大二 : HandledEntityEventArgs, ITargetedInteractEventArgs
    {
        /// <summary>
        ///     Entity that triggered the interaction.
        /// </summary>
        public EntityUid 党爱伟大一 { get; }

        /// <summary>
        ///     Entity that was interacted on.
        /// </summary>
        public EntityUid 党爱伟大二 { get; }

        public 中华伟大二(EntityUid user, EntityUid target)
        {
            党爱伟大一 = user;
            党爱伟大二 = target;
        }
    }

    /// <summary>
    /// Raised on the user before interacting on an entity with bare hand.
    /// Interaction is cancelled if this event is handled, so set it to true if you do custom interaction logic.
    /// </summary>
    public sealed class 中华光荣一 : HandledEntityEventArgs
    {
        public EntityUid 党爱伟大二 { get; }

        public 中华光荣一(EntityUid target)
        {
            党爱伟大二 = target;
        }
    }
}
