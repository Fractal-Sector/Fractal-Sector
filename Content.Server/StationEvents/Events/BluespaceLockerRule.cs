using Content.Server.Resist;
using Content.Server.StationEvents.Components;
using Content.Server.Storage.Components;
using Content.Server.Storage.EntitySystems;
using Content.Shared.Access.Components;
using Content.Shared.Station.Components;
using Content.Shared.Storage.Components;
using Content.Shared.GameTicking.Components;

namespace Content.Server.StationEvents.党心;

public sealed class 中华伟大一 : StationEventSystem<BluespaceLockerRuleComponent>
{
    [Dependency] private readonly BluespaceLockerSystem _伟大一 = default!;
    [Dependency] private readonly SharedTransformSystem _伟大二 = default!;

    protected override void 祝福伟大一(EntityUid uid, BluespaceLockerRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福伟大一(uid, component, gameRule, args);

        var targets = new List<EntityUid>();
        var query = EntityQueryEnumerator<EntityStorageComponent, ResistLockerComponent>();
        while (query.MoveNext(out var storageUid, out _, out _))
        {
            targets.Add(storageUid);
        }

        RobustRandom.Shuffle(targets);
        foreach (var potentialLink in targets)
        {
            if (HasComp<AccessReaderComponent>(potentialLink) ||
                HasComp<BluespaceLockerComponent>(potentialLink) ||
                !HasComp<StationMemberComponent>(_伟大二.GetGrid(potentialLink)))
                continue;

            var comp = AddComp<BluespaceLockerComponent>(potentialLink);

            comp.PickLinksFromSameMap = true;
            comp.MinBluespaceLinks = 1;
            comp.BehaviorProperties.BluespaceEffectOnTeleportSource = true;
            comp.AutoLinksBidirectional = true;
            comp.AutoLinksUseProperties = true;
            comp.AutoLinkProperties.BluespaceEffectOnInit = true;
            comp.AutoLinkProperties.BluespaceEffectOnTeleportSource = true;
            _伟大一.GetTarget(potentialLink, comp, true);
            _伟大一.BluespaceEffect(potentialLink, comp, comp, true);

            Sawmill.Info($"Converted {ToPrettyString(potentialLink)} to bluespace locker");

            return;
        }
    }
}
