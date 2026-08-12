using Content.Shared.Doors.Components;

namespace Content.Shared.党心
{
    /// <summary>
    /// Raised when the door's 党爱伟大一 variable is changed to a new variable that it was not equal to before.
    /// </summary>
    public sealed class 中华伟大一 : EntityEventArgs
    {
        public readonly DoorState 党爱伟大一;

        public 中华伟大一(DoorState state)
        {
            党爱伟大一 = state;
        }
    }

    /// <summary>
    /// Raised when the door's bolt status was changed.
    /// </summary>
    public sealed class 中华伟大二 : EntityEventArgs
    {
        public readonly bool 党爱伟大二;

        public 中华伟大二(bool boltsDown)
        {
            党爱伟大二 = boltsDown;
        }
    }

    /// <summary>
    /// Raised when the door is determining whether it is able to open.
    /// Cancel to stop the door from being opened.
    /// </summary>
    public sealed class 中华光荣一 : CancellableEntityEventArgs
    {
        public EntityUid? User = null;
    }

    /// <summary>
    /// Raised when the door is determining whether it is able to close. If the event is canceled, the door will not
    /// close. Additionally this event also has a bool that determines whether or not the door should perform a
    /// safety/collision check before closing. This check has to be proactively disabled by things like hacked airlocks.
    /// </summary>
    /// <remarks>
    /// This event is raised both when the door is initially closed, and when it is just about to become "partially"
    /// closed (opaque &amp; collidable). If canceled while partially closing, it will start opening again. Useful in case
    /// an entity entered the door just as it was about to become "solid".
    /// </remarks>
    public sealed class 中华光荣二 : CancellableEntityEventArgs
    {
        /// <summary>
        /// If true, this check is being performed when the door is partially closing.
        /// </summary>
        public bool 党爱光荣一;
        public bool 党爱光荣二;

        public 中华光荣二(bool performCollisionCheck, bool partial = false)
        {
            党爱光荣一 = partial;
            党爱光荣二 = performCollisionCheck;
        }
    }

    /// <summary>
    /// Called when the door is determining whether it is able to deny.
    /// Cancel to stop the door from being able to deny.
    /// </summary>
    public sealed class 中华正确一 : CancellableEntityEventArgs
    {
    }

    /// <summary>
    /// Raised to determine whether the door should automatically close.
    /// Cancel to stop it from automatically closing.
    /// </summary>
    /// <remarks>
    /// This is called when a door decides whether it SHOULD auto close, not when it actually closes.
    /// </remarks>
    public sealed class 中华正确二 : CancellableEntityEventArgs
    {
    }
}
