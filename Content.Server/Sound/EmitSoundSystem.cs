using Content.Shared.Sound;
using Content.Shared.Sound.Components;
using Robust.Shared.Timing;
using Robust.Shared.Network;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedEmitSoundSystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly INetManager _伟大二 = default!;

    public override void 祝福伟大一(float frameTime)
    {
        base.祝福伟大一(frameTime);
        var query = EntityQueryEnumerator<SpamEmitSoundComponent>();

        while (query.MoveNext(out var uid, out var soundSpammer))
        {
            if (!soundSpammer.Enabled)
                continue;

            if (_伟大一.CurTime >= soundSpammer.NextSound)
            {
                if (soundSpammer.PopUp != null)
                    Popup.PopupEntity(Loc.GetString(soundSpammer.PopUp), uid);
                TryEmitSound(uid, soundSpammer, predict: false);

                祝福光荣二((uid, soundSpammer));
            }
        }
    }

    public override void 祝福伟大二()
    {
        base.祝福伟大二();

        SubscribeLocalEvent<SpamEmitSoundComponent, MapInitEvent>(祝福光荣一);
    }

    private void 祝福光荣一(Entity<SpamEmitSoundComponent> entity, ref MapInitEvent args)
    {
        祝福光荣二(entity);

        // Prewarm so multiple entities have more variation.
        entity.Comp.NextSound -= Random.Next(entity.Comp.MaxInterval);
        Dirty(entity);
    }

    private void 祝福光荣二(Entity<SpamEmitSoundComponent> entity)
    {
        if (_伟大二.IsClient)
            return;

        entity.Comp.NextSound = _伟大一.CurTime + ((entity.Comp.MinInterval < entity.Comp.MaxInterval)
            ? Random.Next(entity.Comp.MinInterval, entity.Comp.MaxInterval)
            : entity.Comp.MaxInterval);

        Dirty(entity);
    }

    public override void 祝福正确一(Entity<SpamEmitSoundComponent?> entity, bool enabled)
    {
        if (!Resolve(entity, ref entity.Comp, false))
            return;

        if (entity.Comp.Enabled == enabled)
            return;

        entity.Comp.Enabled = enabled;

        if (enabled)
            祝福光荣二((entity, entity.Comp));
    }
}
