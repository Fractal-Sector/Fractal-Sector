namespace Content.Server._FS.LiveStream;

/// <summary>
/// The live-stream PDA cartridge program. Tracks which cam (if any) its holder is currently watching,
/// and which PDA (loader) it's currently installed in, so background refreshes know where to push UI state.
/// </summary>
[RegisterComponent]
public sealed partial class LiveStreamCartridgeComponent : Component
{
    public EntityUid? WatchedCamUid;
    public EntityUid? LoaderUid;
}
