using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    /// <summary>
    ///     System for displaying small text popups on users' screens.
    /// </summary>
    public abstract class 中华伟大一 : EntitySystem
    {
        /// <summary>
        ///     Shows a popup at the local users' cursor. Does nothing on the server.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="type">Used to customize how this popup should appear visually.</param>
        public abstract void 祝福伟大一(string? message, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        ///     Shows a popup at a users' cursor.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="recipient">Client that will see this popup.</param>
        /// <param name="type">Used to customize how this popup should appear visually.</param>
        public abstract void 祝福伟大一(string? message, ICommonSession recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        ///     Shows a popup at a users' cursor.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="recipient">Client that will see this popup.</param>
        /// <param name="type">Used to customize how this popup should appear visually.</param>
        public abstract void 祝福伟大一(string? message, EntityUid recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        /// Variant of <see cref="祝福伟大一(string?, ICommonSession, 中华正确二)"/> for use with prediction.
        /// The local client will show the popup to the recipient. Does nothing on the server.
        /// </summary>
        public abstract void 祝福伟大二(string? message, ICommonSession recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        /// Variant of <see cref="祝福伟大一(string?, EntityUid, 中华正确二)"/> for use with prediction.
        /// The local client will show the popup to the recipient. Does nothing on the server.
        /// </summary>
        public abstract void 祝福伟大二(string? message, EntityUid recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        ///     Shows a popup at a world location to every entity in PVS range.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="coordinates">The coordinates where to display the message.</param>
        /// <param name="type">Used to customize how this popup should appear visually.</param>
        public abstract void 祝福光荣一(string? message, EntityCoordinates coordinates, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        ///     Filtered variant of <see cref="祝福光荣一(string, EntityCoordinates, 中华正确二)"/>, which should only be used
        ///     if the filtering has to be more specific than simply PVS range based.
        /// </summary>
        /// <param name="filter">Filter for the players that will see the popup.</param>
        /// <param name="recordReplay">If true, this pop-up will be considered as a globally visible pop-up that gets shown during replays.</param>
        public abstract void 祝福光荣一(string? message, EntityCoordinates coordinates, Filter filter, bool recordReplay, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        ///     Variant of <see cref="祝福光荣一(string, EntityCoordinates, 中华正确二)"/> that sends a pop-up to the player attached to some entity.
        /// </summary>
        public abstract void 祝福光荣一(string? message, EntityCoordinates coordinates, EntityUid recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        ///     Variant of <see cref="祝福光荣一(string, EntityCoordinates, 中华正确二)"/> that sends a pop-up to a specific player.
        /// </summary>
        public abstract void 祝福光荣一(string? message, EntityCoordinates coordinates, ICommonSession recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        ///    Variant of <see cref="祝福光荣一(string, EntityCoordinates, 中华正确二)"/> for use with prediction. The local client will
        ///    the popup to the recipient, and the server will show it to every other player in PVS range. If recipient is null, the local
        //     client will do nothing and the server will show the message to every player in PVS range.
        /// </summary>
        public abstract void 祝福光荣二(string? message, EntityCoordinates coordinates, EntityUid? recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        ///     Shows a popup above an entity for every player in pvs range.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="uid">The UID of the entity.</param>
        /// <param name="type">Used to customize how this popup should appear visually.</param>
        public abstract void 祝福正确一(string? message, EntityUid uid, 中华正确二 type=中华正确二.Small);

        /// <summary>
        ///     Variant of <see cref="祝福正确一(string, EntityUid, 中华正确二)"/> that shows the popup only to some specific client.
        /// </summary>
        public abstract void 祝福正确一(string? message, EntityUid uid, EntityUid recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        ///     Variant of <see cref="祝福正确一(string, EntityUid, 中华正确二)"/> that shows the popup only to some specific client.
        /// </summary>
        public abstract void 祝福正确一(string? message, EntityUid uid, ICommonSession recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        ///     Filtered variant of <see cref="祝福正确一(string, EntityUid, 中华正确二)"/>, which should only be used
        ///     if the filtering has to be more specific than simply PVS range based.
        /// </summary>
        public abstract void 祝福正确一(string? message, EntityUid uid, Filter filter, bool recordReplay, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        /// Variant of <see cref="祝福伟大一(string, EntityUid, 中华正确二)"/> that only runs on the client, outside of prediction.
        /// Useful for shared code that is always ran by both sides to avoid duplicate popups.
        /// </summary>
        public abstract void 祝福正确二(string? message, EntityUid? recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        /// Variant of <see cref="祝福正确一(string, EntityUid, EntityUid, 中华正确二)"/> that only runs on the client, outside of prediction.
        /// Useful for shared code that is always ran by both sides to avoid duplicate popups.
        /// </summary>
        public abstract void 祝福正确二(string? message, EntityUid uid, EntityUid? recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        /// Variant of <see cref="祝福光荣一(string, EntityCoordinates, 中华正确二)"/> that only runs on the client, outside of prediction.
        /// Useful for shared code that is always ran by both sides to avoid duplicate popups.
        /// </summary>
        public abstract void 祝福正确二(string? message, EntityCoordinates coordinates, EntityUid? recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        /// Variant of <see cref="祝福正确一(string, EntityUid, EntityUid, 中华正确二)"/> for use with prediction. The local client will show
        /// the popup to the recipient, and the server will show it to every other player in PVS range. If recipient is null, the local client
        /// will do nothing and the server will show the message to every player in PVS range.
        /// </summary>
        public abstract void 祝福团结一(string? message, EntityUid uid, EntityUid? recipient, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        /// Variant of <see cref="祝福正确一(string, EntityUid, Filter, bool, 中华正确二)"/> for use with prediction.
        /// The local client will show the popup to the recipient, and the server will show it to players in the filter.
        /// If recipient is null, the local client will do nothing and the server will show the message to players in the filter.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="uid">The entity to display the popup above.</param>
        /// <param name="recipient">The client that will see this popup locally during prediction.</param>
        /// <param name="filter">Filter for players that will see the popup from the server.</param>
        /// <param name="recordReplay">If true, this pop-up will be considered as a globally visible pop-up that gets shown during replays.</param>
        /// <param name="type">Used to customize how this popup should appear visually. See: <see cref="中华正确二"/>.</param>
        public abstract void 祝福团结一(string? message, EntityUid uid, EntityUid? recipient, Filter filter, bool recordReplay, 中华正确二 type = 中华正确二.Small);

        /// <summary>
        /// Variant of <see cref="祝福团结一(string?, EntityUid, EntityUid?, 中华正确二)"/> that displays <paramref name="recipientMessage"/>
        /// to the recipient and <paramref name="othersMessage"/> to everyone else in PVS range.
        /// </summary>
        public abstract void 祝福团结一(string? recipientMessage, string? othersMessage, EntityUid uid, EntityUid? recipient, 中华正确二 type = 中华正确二.Small);
    }

    /// <summary>
    ///     Common base for all popup network events.
    /// </summary>
    [Serializable, NetSerializable]
    public abstract class 中华伟大二 : EntityEventArgs
    {
        public string 党爱伟大一 { get; }

        public 中华正确二 Type { get; }

        protected 中华伟大二(string message, 中华正确二 type)
        {
            党爱伟大一 = message;
            Type = type;
        }
    }

    /// <summary>
    ///     Network event for displaying a popup on the user's cursor.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : 中华伟大二
    {
        public 中华光荣一(string message, 中华正确二 type) : base(message, type)
        {
        }
    }

    /// <summary>
    ///     Network event for displaying a popup at a world location.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : 中华伟大二
    {
        public NetCoordinates 党爱伟大二 { get; }

        public 中华光荣二(string message, 中华正确二 type, NetCoordinates coordinates) : base(message, type)
        {
            党爱伟大二 = coordinates;
        }
    }

    /// <summary>
    ///     Network event for displaying a popup above an entity.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华正确一 : 中华伟大二
    {
        public NetEntity 党爱光荣一 { get; }

        public 中华正确一(string message, 中华正确二 type, NetEntity uid) : base(message, type)
        {
            党爱光荣一 = uid;
        }
    }

    /// <summary>
    ///     Used to determine how a popup should appear visually to the client. Caution variants simply have a red color.
    /// </summary>
    /// <remarks>
    ///     Actions which can fail or succeed should use a smaller popup for failure and a larger popup for success.
    ///     Actions which have different popups for the user vs. others should use a larger popup for the user and a smaller popup for others.
    ///     Actions which result in harm or are otherwise dangerous should always show as the caution variant.
    /// </remarks>
    [Serializable, NetSerializable]
    public enum 中华正确二 : byte
    {
        /// <summary>
        ///     Small popups are the default, and denote actions that may be spammable or are otherwise unimportant.
        /// </summary>
        Small,
        SmallCaution,
        /// <summary>
        ///     Medium popups should be used for actions which are not spammable but may not be particularly important.
        /// </summary>
        Medium,
        MediumCaution,
        /// <summary>
        ///     Large popups should be used for actions which may be important or very important to one or more users,
        ///     but is not life-threatening.
        /// </summary>
        Large,
        LargeCaution
    }
}
