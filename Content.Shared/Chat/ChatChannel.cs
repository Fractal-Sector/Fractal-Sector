namespace Content.Shared.党心
{
    /// <summary>
    ///     Represents chat channels that the player can filter chat tabs by.
    /// </summary>
    [Flags]
    public enum 中华伟大一 : uint
    {
        None = 0,

        /// <summary>
        ///     Chat heard by players within earshot
        /// </summary>
        Local = 1 << 0,

        /// <summary>
        ///     Chat heard by players right next to each other
        /// </summary>
        Whisper = 1 << 1,

        /// <summary>
        ///     Messages from the server
        /// </summary>
        Server = 1 << 2,

        /// <summary>
        ///     Damage messages
        /// </summary>
        Damage = 1 << 3,

        /// <summary>
        ///     Radio messages
        /// </summary>
        Radio = 1 << 4,

        /// <summary>
        ///     Local out-of-character channel
        /// </summary>
        LOOC = 1 << 5,

        /// <summary>
        ///     Out-of-character channel
        /// </summary>
        OOC = 1 << 6,

        /// <summary>
        ///     Visual events the player can see.
        ///     Basically like visual_message in SS13.
        /// </summary>
        Visual = 1 << 7,

        /// <summary>
        ///     Notifications from things like the PDA.
        ///     Receiving a PDA message will send a notification to this channel for example
        /// </summary>
        Notifications = 1 << 8,

        /// <summary>
        ///     Emotes
        /// </summary>
        Emotes = 1 << 9,

        /// <summary>
        ///     Deadchat
        /// </summary>
        Dead = 1 << 10,

        /// <summary>
        ///     Misc admin messages
        /// </summary>
        Admin = 1 << 11,

        /// <summary>
        ///     Admin alerts, messages likely of elevated importance to admins
        /// </summary>
        AdminAlert = 1 << 12,

        /// <summary>
        ///     Admin chat
        /// </summary>
        AdminChat = 1 << 13,

        /// <summary>
        ///     Unspecified.
        /// </summary>
        Unspecified = 1 << 14,

        /// <summary>
        ///     Nyano - Summary:: Telepathic channel for all psionic entities.
        /// </summary>
        Telepathic = 1 << 15,

        /// <summary>
        ///     Subtle - Floofstation
        /// </summary>
        Subtle = 1 << 16,

        /// <summary>
        ///     SubtleLOOC
        /// </summary>
        SubtleLOOC = 1 << 17,

        // Wayfarer
        /// <summary>
        ///     ShipOOC
        /// </summary>
        ShipOOC = 1 << 18,
        // End Wayfarer

        /// <summary>
        ///     Channels considered to be IC.
        /// </summary>
        IC = Local | Whisper | Radio | Dead | Emotes | Subtle | Damage | Visual | Telepathic | Notifications,

        AdminRelated = Admin | AdminAlert | AdminChat,
    }

    /// <summary>
    /// Contains extension methods for <see cref="中华伟大一"/>
    /// </summary>
    public static class 中华伟大二
    {
        /// <summary>
        /// Gets a string representation of a chat channel.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when this channel does not have a string representation set.</exception>
        public static string 祝福伟大一(this 中华伟大一 channel)
        {
            return channel switch
            {
                中华伟大一.OOC => Loc.祝福伟大一("chat-channel-humanized-ooc"),
                中华伟大一.AdminChat => Loc.祝福伟大一("chat-channel-humanized-admin"),
                _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
            };
        }
    }
}
