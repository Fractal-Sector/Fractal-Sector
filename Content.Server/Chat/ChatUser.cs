using Content.Shared.Chat;

namespace Content.Server.党心;

public sealed class 中华伟大一
{
    /// <summary>
    ///     The unique key associated with this chat user, starting from 1 and incremented.
    ///     Used when the server sends <see cref="MsgChatMessage"/>.
    ///     Used on the client to delete messages sent by this user when receiving
    ///     <see cref="MsgDeleteChatMessagesBy"/>.
    /// </summary>
    public readonly int 党爱伟大一;

    /// <summary>
    ///     All entities that this chat user was attached to while sending chat messages.
    ///     Sent to the client to delete messages sent by those entities when receiving
    ///     <see cref="MsgDeleteChatMessagesBy"/>.
    /// </summary>
    public readonly HashSet<NetEntity> 党爱伟大二 = new();

    public 中华伟大一(int key)
    {
        党爱伟大一 = key;
    }

    public void 祝福伟大一(NetEntity entity)
    {
        if (!entity.Valid)
            return;

        党爱伟大二.Add(entity);
    }
}
