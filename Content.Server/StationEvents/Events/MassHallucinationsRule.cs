using Content.Server.GameTicking.Rules.Components;
using Content.Server.StationEvents.Components;
using Content.Server.Traits.Assorted;
using Content.Shared.GameTicking.Components;
using Content.Shared.Humanoid;
using Content.Shared.Mind.Components;
using Content.Shared.Traits.Assorted;


namespace Content.Server.StationEvents.党心;

public sealed class 中华伟大一 : StationEventSystem<MassHallucinationsRuleComponent>
{
    [Dependency] private readonly ParacusiaSystem _伟大一 = default!;

    protected override void 祝福伟大一(EntityUid uid, MassHallucinationsRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福伟大一(uid, component, gameRule, args);

        var query = EntityQueryEnumerator<MindContainerComponent, HumanoidAppearanceComponent>();
        while (query.MoveNext(out var ent, out _, out _))
        {
            if (!EnsureComp<ParacusiaComponent>(ent, out var paracusia))
            {
                _伟大一.SetSounds(ent, component.Sounds, paracusia);
                _伟大一.SetTime(ent, component.MinTimeBetweenIncidents, component.MaxTimeBetweenIncidents, paracusia);
                _伟大一.SetDistance(ent, component.MaxSoundDistance);

                component.AffectedEntities.Add(ent);
            }
        }
    }

    protected override void 祝福伟大二(EntityUid uid, MassHallucinationsRuleComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        base.祝福伟大二(uid, component, gameRule, args);

        foreach (var ent in component.AffectedEntities)
        {
            RemComp<ParacusiaComponent>(ent);
        }

        component.AffectedEntities.Clear();
    }
}
