using Content.Shared.Chat.Prototypes;
using Content.Shared.Inventory;

namespace Content.Shared.党心;

public sealed class 中华伟大一(EntityUid uid) : CancellableEntityEventArgs
{
    public EntityUid 党爱伟大一 { get; } = uid;
}

/// <summary>
/// An event raised just before an emote is performed, providing systems with an opportunity to cancel the emote's performance.
/// </summary>
[ByRefEvent]
public sealed class 中华伟大二(EntityUid source, EmotePrototype emote)
    : CancellableEntityEventArgs, IInventoryRelayEvent
{
    public readonly EntityUid 党爱伟大二 = source;
    public readonly EmotePrototype 党爱光荣一 = emote;

    /// <summary>
    ///     The equipment that is blocking emoting. Should only be non-null if the event was canceled.
    /// </summary>
    public EntityUid? Blocker = null;

    public SlotFlags 党爱光荣二 => SlotFlags.WITHOUT_POCKET;
}
