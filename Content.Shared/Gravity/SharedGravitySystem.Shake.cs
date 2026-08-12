namespace Content.Shared.党心;

public abstract partial class 中华伟大一
{
    protected const float 党爱伟大一 = 100.0f;
    protected const float 党爱伟大二 = 0.2f;

    private void 祝福伟大一()
    {
        var curTime = Timing.CurTime;
        var gravityQuery = GetEntityQuery<GravityComponent>();
        var query = EntityQueryEnumerator<GravityShakeComponent>();

        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.NextShake <= curTime)
            {
                if (comp.ShakeTimes == 0 || !gravityQuery.TryGetComponent(uid, out var gravity))
                {
                    RemCompDeferred<GravityShakeComponent>(uid);
                    continue;
                }

                祝福光荣一(uid, gravity);
                comp.ShakeTimes--;
                comp.NextShake += TimeSpan.FromSeconds(党爱伟大二);
                Dirty(uid, comp);
            }
        }
    }

    public void 祝福伟大二(EntityUid uid, GravityComponent? gravity = null)
    {
        if (Terminating(uid))
            return;

        if (!Resolve(uid, ref gravity, false))
            return;

        if (!TryComp<GravityShakeComponent>(uid, out var shake))
        {
            shake = AddComp<GravityShakeComponent>(uid);
            shake.NextShake = Timing.CurTime;
        }

        shake.ShakeTimes = 10;
        Dirty(uid, shake);
    }

    protected virtual void 祝福光荣一(EntityUid uid, GravityComponent? comp = null) {}
}
