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

namespace Content.Shared.InteractionVerbs;

public abstract class SharedInteractionVerbsSystem : EntitySystem
{
    [Dependency] protected readonly IPrototypeManager PrototypeManager = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly ISerializationManager _serializationManager = default!;
    [Dependency] private readonly ActionBlockerSystem _actionBlocker = default!;
    [Dependency] private readonly SharedInteractionSystem _interaction = default!;
    [Dependency] private readonly INetManager _netManager = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;

    protected InteractionAction.VerbDependencies _verbDependencies = default!;

    public override void Initialize()
    {
        base.Initialize();

        _verbDependencies = new InteractionAction.VerbDependencies(
            EntityManager,
            PrototypeManager,
            _random,
            _timing,
            _serializationManager
        );

        SubscribeLocalEvent<InteractionVerbsComponent, GetVerbsEvent<InteractionVerb>>(OnGetInteractionVerbs);
        SubscribeLocalEvent<GetVerbsEvent<InteractionVerb>>(OnGetGlobalInteractionVerbs);
    }

    private void OnGetGlobalInteractionVerbs(GetVerbsEvent<InteractionVerb> args)
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

    private void OnGetInteractionVerbs(EntityUid uid, InteractionVerbsComponent component, GetVerbsEvent<InteractionVerb> args)
    {
        // Don't show verbs if we can't interact
        if (!args.CanInteract && !args.CanAccess)
            return;

        var user = args.User;
        var target = uid;
        var hasHands = args.Hands != null;

    }

    private bool IsVerbApplicable(InteractionVerbPrototype proto, EntityUid user, EntityUid target, bool hasHands, bool canAccess, bool canInteract)
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

    protected virtual void TryPerformVerb(InteractionVerbPrototype proto, EntityUid user, EntityUid target)
    {
        // This will be implemented in server-side system
    }
}
