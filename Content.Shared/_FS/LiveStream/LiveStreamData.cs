using Robust.Shared.Serialization;

namespace Content.Shared._FS.LiveStream;

/// <summary>
/// A single chat message posted in a live stream (by a viewer, the streamer, or the system).
/// </summary>
[Serializable, NetSerializable]
public sealed class LiveStreamChatMessage
{
    public TimeSpan Time;
    public string Sender;
    public string Text;
    public bool IsSystem;

    public LiveStreamChatMessage(TimeSpan time, string sender, string text, bool isSystem = false)
    {
        Time = time;
        Sender = sender;
        Text = text;
        IsSystem = isSystem;
    }
}

/// <summary>
/// Summary of one active stream, shown in the list of streams a viewer can tune into.
/// </summary>
[Serializable, NetSerializable]
public sealed class LiveStreamInfo
{
    public NetEntity CamNetEntity;
    public string Title;
    public string StreamerName;
    public int ViewerCount;

    public LiveStreamInfo(NetEntity camNetEntity, string title, string streamerName, int viewerCount)
    {
        CamNetEntity = camNetEntity;
        Title = title;
        StreamerName = streamerName;
        ViewerCount = viewerCount;
    }
}
