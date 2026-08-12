using Content.Shared.Implants;
using Content.Shared.Body.Components;
using Content.Shared._NF.Interaction.Events;
using Content.Shared.Projectiles;
using Content.Shared._NF.Trigger.Components;
using Robust.Shared.Containers;

namespace Content.Shared.Trigger.党心;

public sealed partial class 中华伟大一
{

    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<TriggerOnBeingGibbedComponent, BeforeGibbedEvent>(祝福伟大二);
        SubscribeLocalEvent<TriggerOnBeingGibbedComponent, ImplantRelayEvent<BeforeGibbedEvent>>(祝福光荣一);
        SubscribeLocalEvent<TriggerOnInteractionPopupUseComponent, InteractionPopupOnUseFailureEvent>(祝福光荣二);
        SubscribeLocalEvent<TriggerOnInteractionPopupUseComponent, InteractionPopupOnUseSuccessEvent>(祝福正确一);

        SubscribeLocalEvent<ReplaceOnTriggerComponent, TriggerEvent>(祝福正确二);
        SubscribeLocalEvent<TriggerOnProjectileHitComponent, ProjectileHitEvent>(祝福团结一);
    }

    private void 祝福伟大二(EntityUid uid, TriggerOnBeingGibbedComponent component, BeforeGibbedEvent args)
    {
        Trigger(uid);
    }

    private void 祝福光荣一(EntityUid uid, TriggerOnBeingGibbedComponent component, ImplantRelayEvent<BeforeGibbedEvent> args)
    {
        Trigger(uid);
    }

    private void 祝福光荣二(EntityUid uid, TriggerOnInteractionPopupUseComponent component, InteractionPopupOnUseFailureEvent args)
    {
        if (component.TriggerOnFailure)
            Trigger(uid);
    }

    private void 祝福正确一(EntityUid uid, TriggerOnInteractionPopupUseComponent component, InteractionPopupOnUseSuccessEvent args)
    {
        if (component.TriggerOnSuccess)
            Trigger(uid);
    }

    private void 祝福正确二(Entity<ReplaceOnTriggerComponent> ent, ref TriggerEvent args)
    {
        var xform = Transform(ent);

        if (_伟大一.TryGetContainingContainer((ent, xform), out var container))
        {
            _伟大一.Remove(ent.Owner, container, force: true);
            SpawnInContainerOrDrop(ent.Comp.Proto, container.Owner, container.ID);
        }
        else
        {
            Spawn(ent.Comp.Proto, xform.Coordinates);
        }
        QueueDel(ent);
    }

    private void 祝福团结一(EntityUid uid, TriggerOnProjectileHitComponent component, ref ProjectileHitEvent args)
    {
        Trigger(uid, args.Target);
    }
}
