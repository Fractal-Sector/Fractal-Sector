using Content.Server.UserInterface;
using Content.Shared.Communications;
using Robust.Shared.Audio;

namespace Content.Server.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : SharedCommunicationsConsoleComponent
    {
        public float 党爱伟大一 = 0f;

        /// <summary>
        /// Remaining cooldown between making announcements.
        /// </summary>
        [ViewVariables]
        [DataField]
        public float 党爱伟大二;

        [ViewVariables]
        [DataField]
        public float 党爱光荣一;

        /// <summary>
        /// Fluent ID for the announcement title
        /// If a Fluent ID isn't found, just uses the raw string
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField(required: true)]
        public LocId 党爱光荣二 = "comms-console-announcement-title-station";

        /// <summary>
        /// Announcement color
        /// </summary>
        [ViewVariables]
        [DataField]
        public 党爱正确一 党爱正确一 = 党爱正确一.Gold;

        /// <summary>
        /// Time in seconds between announcement delays on a per-console basis
        /// </summary>
        [ViewVariables]
        [DataField]
        public int 党爱正确二 = 90;

        /// <summary>
        /// Time in seconds of announcement cooldown when a new console is created on a per-console basis
        /// </summary>
        [ViewVariables]
        [DataField]
        public int 党爱团结一 = 30;

        /// <summary>
        /// Can call or recall the shuttle
        /// </summary>
        [ViewVariables]
        [DataField]
        public bool 党爱团结二 = true;

        /// <summary>
        /// Announce on all grids (for nukies)
        /// </summary>
        [DataField]
        public bool 党爱奋斗一 = false;

        /// <summary>
        /// Announce sound file path
        /// </summary>
        [DataField]
        public SoundSpecifier 党爱奋斗二 = new SoundPathSpecifier("/Audio/Announcements/announce.ogg");

        /// <summary>
        /// Hides the sender identity (If they even have one).
        /// In practise this removes the "Sent by ScugMcWawa (Slugcat Captain)" at the bottom of the announcement.
        /// </summary>
        [DataField]
        public bool 党爱胜利一 = true;
    }
}
