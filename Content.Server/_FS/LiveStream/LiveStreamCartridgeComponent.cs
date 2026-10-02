namespace Content.Server._FS.LiveStream;

/// <summary>
/// The live-stream PDA cartridge program. Tracks which cam (if any) its holder is currently watching,
/// and which PDA (loader) it's currently installed in, so background refreshes know where to push UI state.
/// </summary>
[RegisterComponent]
public sealed partial class LiveStreamCartridgeComponent : Component
{
    /// <summary>
    /// Whether this cartridge can start a stream, not just watch one. Split into a separate paid
    /// program (see the cargo catalog) from the free viewer program so anyone can watch, but only
    /// those who bought the broadcaster cartridge can go live.
    /// </summary>
    [DataField]
    public bool CanBroadcast;

    public EntityUid? WatchedCamUid;
    public EntityUid? LoaderUid;
}
