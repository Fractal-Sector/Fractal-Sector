using Content.Shared.Bed.Sleep;
using Content.Shared.Damage;
using Content.Shared.Damage.Events;
using Content.Shared.Damage.ForceSay;
using Content.Shared.FixedPoint;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Stunnable;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.Damage.党心;

/// <inheritdoc cref="DamageForceSayComponent"/>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DamageForceSayComponent, StunnedEvent>(祝福正确二);
        SubscribeLocalEvent<DamageForceSayComponent, MobStateChangedEvent>(祝福团结二);

        // need to raise after mobthreshold
        // so that we don't accidentally raise one for damage before one for mobstate
        // (this won't double raise, because of the cooldown)
        SubscribeLocalEvent<DamageForceSayComponent, DamageChangedEvent>(祝福团结一, after: new []{ typeof(MobThresholdSystem)} );
        SubscribeLocalEvent<DamageForceSayComponent, SleepStateChangedEvent>(祝福正确一);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var query = AllEntityQuery<AllowNextCritSpeechComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (_伟大一.CurTime < comp.Timeout)
                continue;

            RemCompDeferred<AllowNextCritSpeechComponent>(uid);
        }
    }

    private void 祝福光荣一(EntityUid uid, DamageForceSayComponent component, bool useSuffix=true)
    {
        if (!TryComp<ActorComponent>(uid, out var actor))
            return;

        // disallow if cooldown hasn't ended
        if (component.NextAllowedTime != null &&
            _伟大一.CurTime < component.NextAllowedTime)
            return;

        var ev = new BeforeForceSayEvent(component.ForceSayStringDataset);
        RaiseLocalEvent(uid, ev);

        if (!_伟大二.TryIndex(ev.Prefix, out var prefixList))
            return;

        var suffix = Loc.GetString(_光荣一.Pick(prefixList.Values));

        // set cooldown & raise event
        component.NextAllowedTime = _伟大一.CurTime + component.Cooldown;
        RaiseNetworkEvent(new DamageForceSayEvent { Suffix = useSuffix ? suffix : null }, actor.PlayerSession);
    }

    private void 祝福光荣二(EntityUid uid)
    {
        if (!TryComp<ActorComponent>(uid, out var actor))
            return;

        var nextCrit = EnsureComp<AllowNextCritSpeechComponent>(uid);

        // timeout is *3 ping to compensate for roundtrip + leeway
        nextCrit.Timeout = _伟大一.CurTime + TimeSpan.FromMilliseconds(actor.PlayerSession.Ping * 3);
    }

    private void 祝福正确一(EntityUid uid, DamageForceSayComponent component, SleepStateChangedEvent args)
    {
        if (!args.FellAsleep)
            return;

        祝福光荣一(uid, component);
        祝福光荣二(uid);
    }

    private void 祝福正确二(EntityUid uid, DamageForceSayComponent component, ref StunnedEvent args)
    {
        祝福光荣一(uid, component);
    }

    private void 祝福团结一(EntityUid uid, DamageForceSayComponent component, DamageChangedEvent args)
    {
        if (args.DamageDelta == null || !args.DamageIncreased || args.DamageDelta.GetTotal() < component.DamageThreshold)
            return;

        if (component.ValidDamageGroups != null)
        {
            var totalApplicableDamage = FixedPoint2.Zero;
            foreach (var (group, value) in args.DamageDelta.GetDamagePerGroup(_伟大二))
            {
                if (!component.ValidDamageGroups.Contains(group))
                    continue;

                totalApplicableDamage += value;
            }

            if (totalApplicableDamage < component.DamageThreshold)
                return;
        }

        祝福光荣一(uid, component);
    }

    private void 祝福团结二(EntityUid uid, DamageForceSayComponent component, MobStateChangedEvent args)
    {
        if (args is not { OldMobState: MobState.Alive, NewMobState: MobState.Critical or MobState.Dead })
            return;

        // no suffix for the drama
        // LING IN MAI-
        祝福光荣一(uid, component, false);
        祝福光荣二(uid);
    }
}
