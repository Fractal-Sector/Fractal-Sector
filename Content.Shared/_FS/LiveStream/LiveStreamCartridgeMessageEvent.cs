using Content.Shared.CartridgeLoader;
using Robust.Shared.Serialization;

namespace Content.Shared._FS.LiveStream;

/// <summary>
/// The action a player took in the live-stream cartridge UI.
/// </summary>
[Serializable, NetSerializable]
public enum LiveStreamMessageType : byte
{
    /// <summary>Start streaming from the holder's cam. Content = custom title (may be empty).</summary>
    StartStream,

    /// <summary>Stop the holder's own stream.</summary>
    StopStream,

    /// <summary>Start watching someone else's stream. Content = the NetEntity id of their cam, as a string.</summary>
    WatchStream,

    /// <summary>Stop watching whatever is currently being watched.</summary>
    StopWatching,

    /// <summary>Post a chat message to the current stream (own, if streaming; otherwise the one being watched). Content = message text.</summary>
    SendChat,

    /// <summary>Send a real-money donation to the streamer being watched. Content = "amount|message".</summary>
    SendDonate,
}

/// <summary>
/// Sent from the live-stream cartridge UI to the server. <see cref="CartridgeMessageEvent"/>'s User/LoaderUid/Actor
/// fields are filled in by <c>CartridgeLoaderSystem</c> when it relays this to the active cartridge program.
/// </summary>
[Serializable, NetSerializable]
public sealed class LiveStreamCartridgeMessageEvent : CartridgeMessageEvent
{
    public LiveStreamMessageType Type;
    public string Content = string.Empty;

    public LiveStreamCartridgeMessageEvent(LiveStreamMessageType type, string content)
    {
        Type = type;
        Content = content;
    }
}
