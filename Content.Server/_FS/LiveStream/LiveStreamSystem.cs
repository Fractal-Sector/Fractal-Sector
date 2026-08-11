using Content.Server._NF.Bank;
using Content.Server.SurveillanceCamera;
using Content.Shared._FS.LiveStream;
using Robust.Shared.Timing;

namespace Content.Server._FS.LiveStream;

/// <summary>
/// Core live-streaming logic: starting/stopping a stream, viewers, chat, and donations. Reuses
/// <see cref="SurveillanceCameraSystem"/> for the actual video feed (PVS + the cam's EyeComponent) rather
/// than inventing a new one - a "stream" is just a surveillance camera with a friendlier PDA-facing wrapper.
/// </summary>
public sealed class LiveStreamSystem : EntitySystem
{
    [Dependency] private readonly SurveillanceCameraSystem _camera = default!;
    [Dependency] private readonly BankSystem _bank = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    /// <summary>Sane upper bound so a station full of streamers can't spam surveillance-camera viewer state.</summary>
    private const int MaxConcurrentStreams = 5;

    private readonly HashSet<EntityUid> _activeStreams = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LiveStreamCamComponent, ComponentShutdown>(OnCamShutdown);
    }

    private void OnCamShutdown(EntityUid uid, LiveStreamCamComponent component, ComponentShutdown args)
    {
        if (component.IsStreaming)
            StopStream(uid, component);
    }

    public bool TryStartStream(EntityUid cam, string title, EntityUid holder, out string? errorLocKey, LiveStreamCamComponent? component = null)
    {
        errorLocKey = null;

        if (!Resolve(cam, ref component))
        {
            errorLocKey = "live-stream-error-no-camera";
            return false;
        }

        if (component.IsStreaming)
        {
            errorLocKey = "live-stream-error-already-streaming";
            return false;
        }

        if (_activeStreams.Count >= MaxConcurrentStreams)
        {
            errorLocKey = "live-stream-error-too-many-streams";
            return false;
        }

        component.IsStreaming = true;
        component.StreamTitle = string.IsNullOrWhiteSpace(title) ? Loc.GetString("live-stream-default-title") : title;
        component.HolderUid = holder;
        component.ViewerCount = 0;
        component.ChatMessages.Clear();

        _activeStreams.Add(cam);
        _camera.SetActive(cam, true);

        AddChatMessage(cam, Loc.GetString("live-stream-chat-system-sender"), Loc.GetString("live-stream-chat-started"), true, component);

        return true;
    }

    public void StopStream(EntityUid cam, LiveStreamCamComponent? component = null)
    {
        if (!Resolve(cam, ref component) || !component.IsStreaming)
            return;

        AddChatMessage(cam, Loc.GetString("live-stream-chat-system-sender"), Loc.GetString("live-stream-chat-ended"), true, component);

        component.IsStreaming = false;
        component.HolderUid = null;
        component.ViewerCount = 0;
        _activeStreams.Remove(cam);
        _camera.SetActive(cam, false);
    }

    public bool TryAddViewer(EntityUid cam, EntityUid viewer, LiveStreamCamComponent? component = null)
    {
        if (!Resolve(cam, ref component) || !component.IsStreaming)
            return false;

        _camera.AddActiveViewer(cam, viewer);
        component.ViewerCount++;
        return true;
    }

    public void RemoveViewer(EntityUid cam, EntityUid viewer, LiveStreamCamComponent? component = null)
    {
        if (!Resolve(cam, ref component, false))
            return;

        _camera.RemoveActiveViewer(cam, viewer);
        component.ViewerCount = Math.Max(0, component.ViewerCount - 1);
    }

    public void AddChatMessage(EntityUid cam, string sender, string text, bool isSystem, LiveStreamCamComponent? component = null)
    {
        if (!Resolve(cam, ref component))
            return;

        component.ChatMessages.Add(new LiveStreamChatMessage(_timing.CurTime, sender, text, isSystem));

        var overflow = component.ChatMessages.Count - component.MaxChatMessages;
        if (overflow > 0)
            component.ChatMessages.RemoveRange(0, overflow);
    }

    /// <summary>Real bank transfer from viewer to streamer - not flavor text.</summary>
    public bool TrySendDonation(EntityUid viewer, EntityUid cam, int amount, out string? errorLocKey, LiveStreamCamComponent? component = null)
    {
        errorLocKey = null;

        if (amount <= 0)
        {
            errorLocKey = "live-stream-error-invalid-amount";
            return false;
        }

        if (!Resolve(cam, ref component) || !component.IsStreaming || component.HolderUid is not { } holder)
        {
            errorLocKey = "live-stream-error-not-streaming";
            return false;
        }

        if (!_bank.TryBankWithdraw(viewer, amount))
        {
            errorLocKey = "live-stream-error-insufficient-funds";
            return false;
        }

        _bank.TryBankDeposit(holder, amount);
        return true;
    }

    public LiveStreamCamComponent? GetStreamComponent(EntityUid cam) => CompOrNull<LiveStreamCamComponent>(cam);

    public List<LiveStreamInfo> GetActiveStreamInfos()
    {
        var list = new List<LiveStreamInfo>();

        foreach (var cam in _activeStreams)
        {
            if (!TryComp<LiveStreamCamComponent>(cam, out var comp) || comp.HolderUid is not { } holder)
                continue;

            list.Add(new LiveStreamInfo(GetNetEntity(cam), comp.StreamTitle, MetaData(holder).EntityName, comp.ViewerCount));
        }

        return list;
    }
}
