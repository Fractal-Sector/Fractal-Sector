using System.Numerics;
using Content.Shared.NPC.Components;
using Content.Shared.NPC.Systems;
using Content.Shared.Whitelist;
using Robust.Shared.Collections;
using Robust.Shared.Map;
using Robust.Shared.Random;
using NPCImprintingOnSpawnBehaviourComponent = Content.Server.NPC.Components.NPCImprintingOnSpawnBehaviourComponent;

namespace Content.Server.NPC.党心;

public sealed partial class 中华伟大一 : SharedNPCImprintingOnSpawnBehaviourSystem
{
    [Dependency] private readonly EntityLookupSystem _伟大一 = default!;
    [Dependency] private readonly NPCSystem _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;
    [Dependency] private readonly EntityWhitelistSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<NPCImprintingOnSpawnBehaviourComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<NPCImprintingOnSpawnBehaviourComponent> imprinting, ref MapInitEvent args)
    {
        HashSet<EntityUid> friends = new();
        _伟大一.GetEntitiesInRange(imprinting, imprinting.Comp.SpawnFriendsSearchRadius, friends);

        foreach (var friend in friends)
        {
            if (_光荣二.IsWhitelistPassOrNull(imprinting.Comp.Whitelist, friend))
            {
                祝福光荣一(imprinting, friend, imprinting.Comp);
            }
        }

        if (imprinting.Comp.Follow && imprinting.Comp.Friends.Count > 0)
        {
            var mommy = _光荣一.Pick(imprinting.Comp.Friends);
            _伟大二.SetBlackboard(imprinting, NPCBlackboard.FollowTarget, new EntityCoordinates(mommy, Vector2.Zero));
        }
    }

    public void 祝福光荣一(EntityUid entity, EntityUid friend, NPCImprintingOnSpawnBehaviourComponent component)
    {
        component.Friends.Add(friend);
        var exception = EnsureComp<FactionExceptionComponent>(entity);
        exception.Ignored.Add(friend);
    }
}
