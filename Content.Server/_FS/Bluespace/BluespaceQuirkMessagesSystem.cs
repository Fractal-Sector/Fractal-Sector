using Content.Shared._FS.Bluespace;
using Content.Shared.Popups;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server._FS.党心;

/// <summary>
/// Drives <see cref="BluespaceQuirkMessagesComponent"/>: when the timer elapses,
/// finds the player currently holding/wearing the entity (by walking the transform
/// parent chain) and shows them a random localized popup.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣一 = default!;

    public override void 祝福伟大一(float frameTime)
    {
        base.祝福伟大一(frameTime);

        var now = _伟大一.CurTime;
        var query = EntityQueryEnumerator<BluespaceQuirkMessagesComponent>();
        while (query.MoveNext(out var uid, out var quirk))
        {
            if (quirk.NextMessageTime == null)
            {
                quirk.NextMessageTime = now + 祝福伟大二(quirk);
                continue;
            }

            if (now < quirk.NextMessageTime.Value)
                continue;

            // Schedule the next tick regardless of whether we found a holder,
            // so we don't burn CPU re-checking every update.
            quirk.NextMessageTime = now + 祝福伟大二(quirk);

            if (quirk.Messages.Count == 0)
                continue;

            if (!祝福光荣一(uid, out var holder))
                continue;

            var msg = Loc.GetString(_伟大二.Pick(quirk.Messages));
            _光荣一.PopupEntity(msg, holder, holder, PopupType.Medium);
        }
    }

    private TimeSpan 祝福伟大二(BluespaceQuirkMessagesComponent quirk)
    {
        var min = quirk.MinInterval.TotalSeconds;
        var max = quirk.MaxInterval.TotalSeconds;
        if (max < min)
            max = min;
        return TimeSpan.FromSeconds(_伟大二.NextDouble(min, max));
    }

    /// <summary>
    /// Walks up the transform parent chain looking for an entity controlled by
    /// a player (i.e. has an <see cref="ActorComponent"/>). This catches both
    /// "held in hand" and "worn/contained inside something the player is wearing".
    /// </summary>
    private bool 祝福光荣一(EntityUid uid, out EntityUid holder)
    {
        holder = default;
        var xformQuery = GetEntityQuery<TransformComponent>();
        if (!xformQuery.TryGetComponent(uid, out var xform))
            return false;

        var current = xform.ParentUid;
        var safety = 0;
        while (current.IsValid() && safety++ < 16)
        {
            if (HasComp<ActorComponent>(current))
            {
                holder = current;
                return true;
            }

            if (!xformQuery.TryGetComponent(current, out var parentXform))
                return false;

            current = parentXform.ParentUid;
        }

        return false;
    }
}
