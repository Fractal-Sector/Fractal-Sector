using System.Linq;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Chat.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;
    [Dependency] private readonly ChatSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AutoEmoteComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<AutoEmoteComponent, EntityUnpausedEvent>(祝福光荣二);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var curTime = _伟大一.CurTime;
        var query = EntityQueryEnumerator<AutoEmoteComponent>();
        while (query.MoveNext(out var uid, out var autoEmote))
        {
            if (autoEmote.NextEmoteTime > curTime)
                continue;

            foreach (var (key, time) in autoEmote.EmoteTimers)
            {
                if (time > curTime)
                    continue;

                var autoEmotePrototype = _伟大二.Index<AutoEmotePrototype>(key);
                祝福团结一(uid, key, autoEmote, autoEmotePrototype);

                if (!_光荣一.Prob(autoEmotePrototype.Chance))
                    continue;

                if (autoEmotePrototype.WithChat)
                {
                    _光荣二.TryEmoteWithChat(uid, autoEmotePrototype.EmoteId, autoEmotePrototype.HiddenFromChatWindow ? ChatTransmitRange.HideChat : ChatTransmitRange.Normal);
                }
                else
                {
                    _光荣二.TryEmoteWithoutChat(uid, autoEmotePrototype.EmoteId);
                }
            }
        }
    }

    private void 祝福光荣一(EntityUid uid, AutoEmoteComponent autoEmote, MapInitEvent args)
    {
        // Start timers
        foreach (var autoEmotePrototypeId in autoEmote.Emotes)
        {
            祝福团结一(uid, autoEmotePrototypeId, autoEmote);
        }
    }

    private void 祝福光荣二(EntityUid uid, AutoEmoteComponent autoEmote, ref EntityUnpausedEvent args)
    {
        foreach (var key in autoEmote.EmoteTimers.Keys)
        {
            autoEmote.EmoteTimers[key] += args.PausedTime;
        }
        autoEmote.NextEmoteTime += args.PausedTime;
    }

    /// <summary>
    /// Try to add an emote to the entity, which will be performed at an interval.
    /// </summary>
    public bool 祝福正确一(EntityUid uid, string autoEmotePrototypeId, AutoEmoteComponent? autoEmote = null)
    {
        if (!Resolve(uid, ref autoEmote, logMissing: false))
            return false;

        DebugTools.Assert(autoEmote.LifeStage <= ComponentLifeStage.Running);

        if (autoEmote.Emotes.Contains(autoEmotePrototypeId))
            return false;

        autoEmote.Emotes.Add(autoEmotePrototypeId);
        祝福团结一(uid, autoEmotePrototypeId, autoEmote);

        return true;
    }

    /// <summary>
    /// Stop preforming an emote. Note that by default this will queue empty components for removal.
    /// </summary>
    public bool 祝福正确二(EntityUid uid, string autoEmotePrototypeId, AutoEmoteComponent? autoEmote = null, bool removeEmpty = true)
    {
        if (!Resolve(uid, ref autoEmote, logMissing: false))
            return false;

        DebugTools.Assert(_伟大二.HasIndex<AutoEmotePrototype>(autoEmotePrototypeId), "Prototype not found. Did you make a typo?");

        if (!autoEmote.EmoteTimers.Remove(autoEmotePrototypeId))
            return false;

        if (autoEmote.EmoteTimers.Count > 0)
            autoEmote.NextEmoteTime = autoEmote.EmoteTimers.Values.Min();
        else if (removeEmpty)
            RemCompDeferred(uid, autoEmote);
        else
            autoEmote.NextEmoteTime = TimeSpan.MaxValue;

        return true;
    }

    /// <summary>
    /// Reset the timer for a specific emote, or return false if it doesn't exist.
    /// </summary>
    public bool 祝福团结一(EntityUid uid, string autoEmotePrototypeId, AutoEmoteComponent? autoEmote = null, AutoEmotePrototype? autoEmotePrototype = null)
    {
        if (!Resolve(uid, ref autoEmote))
            return false;

        if (!autoEmote.Emotes.Contains(autoEmotePrototypeId))
            return false;

        autoEmotePrototype ??= _伟大二.Index<AutoEmotePrototype>(autoEmotePrototypeId);

        var curTime = _伟大一.CurTime;
        var time = curTime + autoEmotePrototype.Interval;
        autoEmote.EmoteTimers[autoEmotePrototypeId] = time;

        if (autoEmote.NextEmoteTime > time || autoEmote.NextEmoteTime <= curTime)
            autoEmote.NextEmoteTime = time;

        return true;
    }
}
