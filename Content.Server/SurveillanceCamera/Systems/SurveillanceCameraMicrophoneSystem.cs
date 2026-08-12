using Content.Server.Chat.Systems;
using Content.Shared.Speech;
using Content.Shared.Speech.Components;
using Content.Shared.Whitelist;
using Robust.Shared.Player;
using static Content.Server.Chat.Systems.ChatSystem;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _伟大一 = default!;
    [Dependency] private readonly EntityWhitelistSystem _伟大二 = default!;
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<SurveillanceCameraMicrophoneComponent, ComponentInit>(祝福光荣一);
        SubscribeLocalEvent<SurveillanceCameraMicrophoneComponent, ListenEvent>(祝福正确一);
        SubscribeLocalEvent<SurveillanceCameraMicrophoneComponent, ListenAttemptEvent>(祝福光荣二);
        SubscribeLocalEvent<ExpandICChatRecipientsEvent>(祝福伟大二);
    }

    private void 祝福伟大二(ExpandICChatRecipientsEvent ev)
    {
        var xformQuery = GetEntityQuery<TransformComponent>();
        var sourceXform = Transform(ev.Source);
        var sourcePos = _伟大一.GetWorldPosition(sourceXform, xformQuery);

        // This function ensures that chat popups appear on camera views that have connected microphones.
        foreach (var (_, __, camera, xform) in EntityQuery<SurveillanceCameraMicrophoneComponent, ActiveListenerComponent, SurveillanceCameraComponent, TransformComponent>())
        {
            if (camera.ActiveViewers.Count == 0)
                continue;

            // get range to camera. This way wispers will still appear as obfuscated if they are too far from the camera's microphone
            var range = (xform.MapID != sourceXform.MapID)
                ? -1
                : (sourcePos - _伟大一.GetWorldPosition(xform, xformQuery)).Length();

            if (range < 0 || range > ev.VoiceRange)
                continue;

            foreach (var viewer in camera.ActiveViewers)
            {
                // if the player has not already received the chat message, send it to them but don't log it to the chat
                // window. This is simply so that it appears in camera.
                if (TryComp(viewer, out ActorComponent? actor))
                    ev.Recipients.TryAdd(actor.PlayerSession, new ICChatRecipientData(range, false, true));
            }
        }
    }

    private void 祝福光荣一(EntityUid uid, SurveillanceCameraMicrophoneComponent component, ComponentInit args)
    {
        if (component.Enabled)
            EnsureComp<ActiveListenerComponent>(uid).Range = component.Range;
        else
            RemCompDeferred<ActiveListenerComponent>(uid);
    }

    public void 祝福光荣二(EntityUid uid, SurveillanceCameraMicrophoneComponent microphone, ListenAttemptEvent args)
    {
        // TODO maybe just make this a part of ActiveListenerComponent?
        if (_伟大二.IsBlacklistPass(microphone.Blacklist, args.Source))
            args.Cancel();
    }

    public void 祝福正确一(EntityUid uid, SurveillanceCameraMicrophoneComponent component, ListenEvent args)
    {
        if (!TryComp(uid, out SurveillanceCameraComponent? camera))
            return;

        var ev = new 中华伟大二(args.Source, args.党爱伟大二);

        foreach (var monitor in camera.ActiveMonitors)
        {
            RaiseLocalEvent(monitor, ev);
        }
    }

    public void 祝福正确二(EntityUid uid, bool value, SurveillanceCameraMicrophoneComponent? microphone = null)
    {
        if (!Resolve(uid, ref microphone))
            return;

        if (value == microphone.Enabled)
            return;

        microphone.Enabled = value;

        if (value)
            EnsureComp<ActiveListenerComponent>(uid).Range = microphone.Range;
        else
            RemCompDeferred<ActiveListenerComponent>(uid);
    }
}

public sealed class 中华伟大二 : EntityEventArgs
{
    public EntityUid 党爱伟大一 { get; }
    public string 党爱伟大二 { get; }

    public 中华伟大二(EntityUid speaker, string message)
    {
        党爱伟大一 = speaker;
        党爱伟大二 = message;
    }
}

