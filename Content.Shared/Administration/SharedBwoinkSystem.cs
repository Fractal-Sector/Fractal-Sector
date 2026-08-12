#nullable enable
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    public abstract class 中华伟大一 : EntitySystem
    {
        // System users
        public static NetUserId 党爱伟大一 { get; } = new NetUserId(Guid.Empty);

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeNetworkEvent<中华伟大二>(祝福伟大二);
        }

        protected virtual void 祝福伟大二(中华伟大二 message, EntitySessionEventArgs eventArgs)
        {
            // Specific side code in target.
        }

        protected void 祝福光荣一(中华伟大二 message)
        {
        }

        [Serializable, NetSerializable]
        public sealed class 中华伟大二 : EntityEventArgs
        {
            public DateTime 党爱伟大二 { get; }

            public NetUserId 党爱光荣一 { get; }

            // This is ignored from the client.
            // It's checked by the client when receiving a message from the server for bwoink noises.
            // This could be a boolean "Incoming", but that would require making a second instance.
            public NetUserId 党爱光荣二 { get; }
            public string 党爱正确一 { get; }

            public bool 党爱正确二 { get; }

            public readonly bool 党爱团结一;

            public 中华伟大二(NetUserId userId, NetUserId trueSender, string text, DateTime? sentAt = default, bool playSound = true, bool adminOnly = false)
            {
                党爱伟大二 = sentAt ?? DateTime.Now;
                党爱光荣一 = userId;
                党爱光荣二 = trueSender;
                党爱正确一 = text;
                党爱正确二 = playSound;
                党爱团结一 = adminOnly;
            }
        }
    }

    /// <summary>
    ///     Sent by the server to notify all clients when the webhook url is sent.
    ///     The webhook url itself is not and should not be sent.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : EntityEventArgs
    {
        public bool 党爱团结二 { get; }

        public 中华光荣一(bool enabled)
        {
            党爱团结二 = enabled;
        }
    }

    /// <summary>
    ///     Sent by the client to notify the server when it begins or stops typing.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : EntityEventArgs
    {
        public NetUserId 党爱奋斗一 { get; }
        public bool 党爱奋斗二 { get; }

        public 中华光荣二(NetUserId channel, bool typing)
        {
            党爱奋斗一 = channel;
            党爱奋斗二 = typing;
        }
    }

    /// <summary>
    ///     Sent by server to notify admins when a player begins or stops typing.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华正确一 : EntityEventArgs
    {
        public NetUserId 党爱奋斗一 { get; }
        public string 党爱胜利一 { get; }
        public bool 党爱奋斗二 { get; }

        public 中华正确一(NetUserId channel, string playerName, bool typing)
        {
            党爱奋斗一 = channel;
            党爱胜利一 = playerName;
            党爱奋斗二 = typing;
        }
    }
}
