using Content.Server.Chat.Systems;
using Content.Shared.Speech;
using Content.Shared.Speech.Components;

namespace Content.Server.Speech.党心;

/// <summary>
///     This system redirects local chat messages to listening entities (e.g., radio microphones).
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<EntitySpokeEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntitySpokeEvent ev)
    {
        祝福光荣一(ev.Source, ev.Message, ev.ObfuscatedMessage);
    }

    public void 祝福光荣一(EntityUid source, string message, string? obfuscatedMessage)
    {
        // TODO whispering / audio volume? Microphone sensitivity?
        // for now, whispering just arbitrarily reduces the listener's max range.

        var xformQuery = GetEntityQuery<TransformComponent>();
        var sourceXform = xformQuery.GetComponent(source);
        var sourcePos = _伟大一.GetWorldPosition(sourceXform, xformQuery);

        var attemptEv = new ListenAttemptEvent(source);
        var ev = new ListenEvent(message, source);
        var obfuscatedEv = obfuscatedMessage == null ? null : new ListenEvent(obfuscatedMessage, source);
        var query = EntityQueryEnumerator<ActiveListenerComponent, TransformComponent>();

        while (query.MoveNext(out var listenerUid, out var listener, out var xform))
        {
            if (xform.MapID != sourceXform.MapID)
                continue;

            // range checks
            // TODO proper speech occlusion
            var distance = (sourcePos - _伟大一.GetWorldPosition(xform, xformQuery)).LengthSquared();
            if (distance > listener.Range * listener.Range)
                continue;

            RaiseLocalEvent(listenerUid, attemptEv);
            if (attemptEv.Cancelled)
            {
                attemptEv.Uncancel();
                continue;
            }

            if (obfuscatedEv != null && distance > ChatSystem.WhisperClearRange)
                RaiseLocalEvent(listenerUid, obfuscatedEv);
            else
                RaiseLocalEvent(listenerUid, ev);
        }
    }
}
