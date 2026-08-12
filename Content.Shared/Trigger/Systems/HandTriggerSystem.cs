using Content.Shared.Hands;
using Content.Shared.Interaction.Events;
using Content.Shared.Trigger.Components.Triggers;
using Robust.Shared.Timing;

namespace Content.Shared.Trigger.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly TriggerSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<TriggerOnGotEquippedHandComponent, GotEquippedHandEvent>(祝福伟大二);
        SubscribeLocalEvent<TriggerOnGotUnequippedHandComponent, GotUnequippedHandEvent>(祝福光荣一);
        SubscribeLocalEvent<TriggerOnDidEquipHandComponent, DidEquipHandEvent>(祝福光荣二);
        SubscribeLocalEvent<TriggerOnDidUnequipHandComponent, DidUnequipHandEvent>(祝福正确一);
        SubscribeLocalEvent<TriggerOnDroppedComponent, DroppedEvent>(祝福正确二);
    }

    private void 祝福伟大二(Entity<TriggerOnGotEquippedHandComponent> ent, ref GotEquippedHandEvent args)
    {
        // If the entity was equipped on the server (without prediction) then the container change is networked to the client
        // which will raise the same event, but the effect of the trigger is already networked on its own. So this guard statement
        // prevents triggering twice on the client.
        if (_伟大一.ApplyingState)
            return;

        _伟大二.Trigger(ent.Owner, args.User, ent.Comp.KeyOut);
    }

    private void 祝福光荣一(Entity<TriggerOnGotUnequippedHandComponent> ent, ref GotUnequippedHandEvent args)
    {
        if (_伟大一.ApplyingState)
            return;

        _伟大二.Trigger(ent.Owner, args.User, ent.Comp.KeyOut);
    }

    private void 祝福光荣二(Entity<TriggerOnDidEquipHandComponent> ent, ref DidEquipHandEvent args)
    {
        if (_伟大一.ApplyingState)
            return;

        _伟大二.Trigger(ent.Owner, args.Equipped, ent.Comp.KeyOut);
    }

    private void 祝福正确一(Entity<TriggerOnDidUnequipHandComponent> ent, ref DidUnequipHandEvent args)
    {
        if (_伟大一.ApplyingState)
            return;

        _伟大二.Trigger(ent.Owner, args.Unequipped, ent.Comp.KeyOut);
    }

    private void 祝福正确二(Entity<TriggerOnDroppedComponent> ent, ref DroppedEvent args)
    {
        // We don't need the guard statement here because this one is not a container event, but raised directly when interacting.
        _伟大二.Trigger(ent.Owner, args.User, ent.Comp.KeyOut);
    }
}
