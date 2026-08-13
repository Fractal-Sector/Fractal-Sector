using Content.Server._NF.Bank;
using Content.Server.CartridgeLoader;
using Content.Shared._FS.LiveStream;
using Content.Shared.CartridgeLoader;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Inventory;
using Content.Shared.Popups;

namespace Content.Server._FS.LiveStream;

/// <summary>
/// Wires the live-stream PDA cartridge's UI to <see cref="LiveStreamSystem"/>: finds the holder's physical
/// stream cam, dispatches button-press messages, and keeps every open cartridge's UI state fresh.
/// </summary>
public sealed class LiveStreamCartridgeSystem : EntitySystem
{
    [Dependency] private readonly CartridgeLoaderSystem? _cartridgeLoaderSystem = default!;
    [Dependency] private readonly LiveStreamSystem _liveStream = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly BankSystem _bank = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LiveStreamCartridgeComponent, CartridgeMessageEvent>(OnUiMessage);
        SubscribeLocalEvent<LiveStreamCartridgeComponent, CartridgeUiReadyEvent>(OnUiReady);
        SubscribeLocalEvent<LiveStreamCartridgeComponent, CartridgeDeactivatedEvent>(OnDeactivated);
    }

    private void OnUiReady(EntityUid uid, LiveStreamCartridgeComponent component, CartridgeUiReadyEvent args)
    {
        component.LoaderUid = args.Loader;
        UpdateUiState(uid, args.Loader, component);
    }

    private void OnDeactivated(EntityUid uid, LiveStreamCartridgeComponent component, CartridgeDeactivatedEvent args)
    {
        if (component.WatchedCamUid is { } cam)
        {
            _liveStream.RemoveViewer(cam, args.Loader);
            component.WatchedCamUid = null;
        }
    }

    private void OnUiMessage(EntityUid uid, LiveStreamCartridgeComponent component, CartridgeMessageEvent args)
    {
        if (args is not LiveStreamCartridgeMessageEvent message)
            return;

        var loaderUid = GetEntity(args.LoaderUid);
        var holder = args.Actor;
        component.LoaderUid = loaderUid;

        switch (message.Type)
        {
            case LiveStreamMessageType.StartStream:
                if (!component.CanBroadcast)
                {
                    ShowError(holder, "live-stream-error-no-broadcast-access");
                }
                else if (FindStreamCam(holder) is { } startCam)
                {
                    if (!_liveStream.TryStartStream(startCam, message.Content, holder, out var startErr))
                        ShowError(holder, startErr);
                }
                else
                {
                    ShowError(holder, "live-stream-error-no-camera");
                }

                UpdateAllLiveUIs();
                break;

            case LiveStreamMessageType.StopStream:
                if (FindStreamCam(holder) is { } stopCam)
                    _liveStream.StopStream(stopCam);

                UpdateAllLiveUIs();
                break;

            case LiveStreamMessageType.WatchStream:
                if (component.WatchedCamUid is { } prevCam)
                    _liveStream.RemoveViewer(prevCam, holder);

                component.WatchedCamUid = null;

                if (EntityManager.TryParseNetEntity(message.Content, out var watchCam) && watchCam is { } watchCamValue)
                {
                    if (_liveStream.TryAddViewer(watchCamValue, holder))
                        component.WatchedCamUid = watchCamValue;
                    else
                        ShowError(holder, "live-stream-error-not-streaming");
                }

                UpdateUiState(uid, loaderUid, component);
                break;

            case LiveStreamMessageType.StopWatching:
                if (component.WatchedCamUid is { } watchedCam)
                {
                    _liveStream.RemoveViewer(watchedCam, holder);
                    component.WatchedCamUid = null;
                }

                UpdateUiState(uid, loaderUid, component);
                break;

            case LiveStreamMessageType.SendChat:
                if (SendChat(component, holder, message.Content) is { } chatCam)
                    UpdateUIsWatching(chatCam);
                break;

            case LiveStreamMessageType.SendDonate:
                if (SendDonation(component, holder, message.Content) is { } donateCam)
                    UpdateUIsWatching(donateCam);
                break;
        }
    }

    private EntityUid? SendChat(LiveStreamCartridgeComponent component, EntityUid holder, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        EntityUid? targetCam = null;

        if (FindStreamCam(holder) is { } ownCam && _liveStream.GetStreamComponent(ownCam) is { IsStreaming: true })
            targetCam = ownCam;
        else if (component.WatchedCamUid is { } watched)
            targetCam = watched;

        if (targetCam is not { } cam)
            return null;

        _liveStream.AddChatMessage(cam, MetaData(holder).EntityName, text, false);
        return cam;
    }

    private EntityUid? SendDonation(LiveStreamCartridgeComponent component, EntityUid holder, string content)
    {
        if (component.WatchedCamUid is not { } cam)
            return null;

        var parts = content.Split('|', 2);
        if (parts.Length < 1 || !int.TryParse(parts[0], out var amount))
            return null;

        if (!_liveStream.TrySendDonation(holder, cam, amount, out var err))
        {
            ShowError(holder, err);
            return null;
        }

        var donateMessage = parts.Length > 1 ? parts[1] : string.Empty;
        var senderName = MetaData(holder).EntityName;
        var text = string.IsNullOrWhiteSpace(donateMessage)
            ? Loc.GetString("live-stream-chat-donate", ("sender", senderName), ("amount", amount))
            : Loc.GetString("live-stream-chat-donate-message", ("sender", senderName), ("amount", amount), ("message", donateMessage));

        _liveStream.AddChatMessage(cam, Loc.GetString("live-stream-chat-system-sender"), text, true);
        return cam;
    }

    private void ShowError(EntityUid holder, string? locKey)
    {
        if (locKey is null)
            return;

        _popup.PopupEntity(Loc.GetString(locKey), holder, holder);
    }

    /// <summary>Finds a stream cam the holder is carrying - hands first, then worn inventory slots.</summary>
    private EntityUid? FindStreamCam(EntityUid holder)
    {
        foreach (var held in _hands.EnumerateHeld(holder))
        {
            if (HasComp<LiveStreamCamComponent>(held))
                return held;
        }

        if (_inventory.TryGetContainerSlotEnumerator(holder, out var enumerator))
        {
            while (enumerator.NextItem(out var item, out _))
            {
                if (HasComp<LiveStreamCamComponent>(item))
                    return item;
            }
        }

        return null;
    }

    private void UpdateUiState(EntityUid uid, EntityUid loaderUid, LiveStreamCartridgeComponent? component)
    {
        if (!Resolve(uid, ref component))
            return;

        // The cartridge's own UI messages carry the real actor; for a background refresh (e.g. after someone
        // else's stream started) we only have the PDA (loader), so fall back to whoever is holding/wearing it.
        var holder = Transform(loaderUid).ParentUid;

        var cam = FindStreamCam(holder);
        var camComp = cam is { } c ? _liveStream.GetStreamComponent(c) : null;

        var state = new LiveStreamCartridgeUiState
        {
            HasCamera = cam != null,
            CanBroadcast = component.CanBroadcast,
            IsStreaming = camComp?.IsStreaming ?? false,
            ViewerCount = camComp?.ViewerCount ?? 0,
            StreamTitle = camComp?.StreamTitle ?? string.Empty,
            Balance = _bank.TryGetBalance(holder, out var balance) ? balance : 0,
            ActiveStreams = _liveStream.GetActiveStreamInfos(),
        };

        if (component.WatchedCamUid is { } watchedCam && TryComp<LiveStreamCamComponent>(watchedCam, out var watchedComp))
        {
            state.WatchedCamNetEntity = GetNetEntity(watchedCam);
            state.WatchedStreamerName = watchedComp.HolderUid is { } wHolder ? MetaData(wHolder).EntityName : string.Empty;
            state.ChatMessages = new List<LiveStreamChatMessage>(watchedComp.ChatMessages);
        }
        else if (camComp is { IsStreaming: true })
        {
            state.ChatMessages = new List<LiveStreamChatMessage>(camComp.ChatMessages);
        }

        _cartridgeLoaderSystem?.UpdateCartridgeUiState(loaderUid, state);
    }

    /// <summary>Refreshes every open live-stream cartridge - used after anything that changes the global stream
    /// list (starting/stopping a stream changes what everyone browsing the list sees).</summary>
    private void UpdateAllLiveUIs()
    {
        var query = EntityQueryEnumerator<LiveStreamCartridgeComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.LoaderUid is { } loader)
                UpdateUiState(uid, loader, comp);
        }
    }

    /// <summary>
    /// Refreshes only the cartridges that actually care about <paramref name="cam"/> - its streamer and its
    /// current viewers - instead of every open cartridge on the station. A busy chat would otherwise force a
    /// full state resend (up to 100 chat lines + the stream list) to everyone's PDA on every single message.
    /// </summary>
    private void UpdateUIsWatching(EntityUid cam)
    {
        var query = EntityQueryEnumerator<LiveStreamCartridgeComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.LoaderUid is not { } loader)
                continue;

            var holder = Transform(loader).ParentUid;
            var isRelevant = comp.WatchedCamUid == cam || FindStreamCam(holder) == cam;

            if (isRelevant)
                UpdateUiState(uid, loader, comp);
        }
    }
}
