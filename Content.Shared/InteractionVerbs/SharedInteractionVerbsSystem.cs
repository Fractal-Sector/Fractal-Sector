using Content.Shared.ActionBlocker;
using Content.Shared.DoAfter;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] protected readonly IPrototypeManager 党爱伟大一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _伟大一 = default!;
    [Dependency] private readonly SharedPopupSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly IRobustRandom _光荣二 = default!;
    [Dependency] private readonly IGameTiming _正确一 = default!;
    [Dependency] private readonly ISerializationManager _正确二 = default!;
    [Dependency] private readonly ActionBlockerSystem _团结一 = default!;
    [Dependency] private readonly SharedInteractionSystem _团结二 = default!;
    [Dependency] private readonly INetManager _奋斗一 = default!;
    [Dependency] private readonly SharedHandsSystem _奋斗二 = default!;

    protected InteractionAction.VerbDependencies 党爱伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        党爱伟大二 = new InteractionAction.VerbDependencies(
            EntityManager,
            党爱伟大一,
            _光荣二,
            _正确一,
            _正确二
        );

        SubscribeLocalEvent<InteractionVerbsComponent, GetVerbsEvent<InteractionVerb>>(祝福光荣一);
        SubscribeLocalEvent<GetVerbsEvent<InteractionVerb>>(祝福伟大二);
    }

    private void 祝福伟大二(GetVerbsEvent<InteractionVerb> args)
    {
        // Skip if entity has InteractionVerbsComponent - those are handled separately
        if (HasComp<InteractionVerbsComponent>(args.Target))
            return;

        // Don't show verbs if we can't interact
        if (!args.CanInteract && !args.CanAccess)
            return;

        var user = args.User;
        var target = args.Target;
        var hasHands = args.Hands != null;

    }

    private void 祝福光荣一(EntityUid uid, InteractionVerbsComponent component, GetVerbsEvent<InteractionVerb> args)
    {
        // Don't show verbs if we can't interact
        if (!args.CanInteract && !args.CanAccess)
            return;

        var user = args.User;
        var target = uid;
        var hasHands = args.Hands != null;

    }

    private bool 祝福光荣二(InteractionVerbPrototype proto, EntityUid user, EntityUid target, bool hasHands, bool canAccess, bool canInteract)
    {
        if (proto.Abstract)
            return false;

        if (!proto.AllowSelfInteract && user == target)
            return false;

        if (proto.RequiresHands && !hasHands)
            return false;

        if (proto.RequiresCanAccess && !canAccess)
            return false;

        // Check range
        var transform = Transform(user);
        var targetTransform = Transform(target);
        if (!transform.Coordinates.TryDistance(EntityManager, targetTransform.Coordinates, out var distance))
            return false;

        if (distance < proto.Range.Min || distance > proto.Range.Max)
            return false;

        return true;
    }

    protected virtual void 祝福正确一(InteractionVerbPrototype proto, EntityUid user, EntityUid target)
    {
        // This will be implemented in server-side system
    }
}
