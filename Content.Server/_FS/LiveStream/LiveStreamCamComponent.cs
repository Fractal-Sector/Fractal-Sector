using Content.Shared._FS.LiveStream;

namespace Content.Server._FS.LiveStream;

/// <summary>
/// A handheld/worn camera that can broadcast a live stream (piggybacking on <c>SurveillanceCameraComponent</c>
/// for the actual video feed) to anyone tuned in via the live-stream PDA cartridge.
/// </summary>
[RegisterComponent]
public sealed partial class LiveStreamCamComponent : Component
{
    [DataField]
    public bool IsStreaming;

    [DataField]
    public string StreamTitle = string.Empty;

    [DataField]
    public int MaxChatMessages = 100;

    /// <summary>Whoever is currently holding/wearing this cam - the one who gets paid for donations.</summary>
    public EntityUid? HolderUid;

    public int ViewerCount;

    public List<LiveStreamChatMessage> ChatMessages = new();
}
