using Content.Shared.CartridgeLoader;
using Robust.Shared.Serialization;

namespace Content.Shared._FS.LiveStream;

/// <summary>
/// Full UI state pushed to the live-stream PDA cartridge: whether the holder has a stream cam, its own
/// stream status, the list of streams available to watch, and (if watching/streaming) the chat log.
/// </summary>
[Serializable, NetSerializable]
public sealed class LiveStreamCartridgeUiState : BoundUserInterfaceState
{
    public bool HasCamera;
    public bool CanBroadcast;
    public bool IsStreaming;
    public int ViewerCount;
    public string StreamTitle = string.Empty;
    public int Balance;

    public List<LiveStreamInfo> ActiveStreams = new();

    public NetEntity? WatchedCamNetEntity;
    public string WatchedStreamerName = string.Empty;

    public List<LiveStreamChatMessage> ChatMessages = new();
}
