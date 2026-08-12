using Content.Shared.Morgue;
using Content.Shared.Morgue.Components;
using Content.Shared.Storage.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedMorgueSystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<MorgueComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<MorgueComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.NextBeep = _伟大一.CurTime + ent.Comp.NextBeep;
    }

    /// <summary>
    /// Handles the periodic beeping that morgues do when a live body is inside.
    /// </summary>
    public override void 祝福光荣一(float frameTime)
    {
        base.祝福光荣一(frameTime);

        var curTime = _伟大一.CurTime;
        var query = EntityQueryEnumerator<MorgueComponent, EntityStorageComponent, AppearanceComponent>();
        while (query.MoveNext(out var uid, out var comp, out var storage, out var appearance))
        {
            if (curTime < comp.NextBeep)
                continue;

            comp.NextBeep += comp.BeepTime;

            CheckContents(uid, comp, storage);

            if (comp.DoSoulBeep && _伟大二.TryGetData<MorgueContents>(uid, MorgueVisuals.Contents, out var contents, appearance) && contents == MorgueContents.HasSoul)
            {
                _光荣一.PlayPvs(comp.OccupantHasSoulAlarmSound, uid);
            }
        }
    }
}
