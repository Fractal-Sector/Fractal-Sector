using Content.Shared.Actions;
using Content.Shared.Actions.Components;
using Content.Shared.Mobs.Components;

namespace Content.Shared.Mobs.党心;

/// <summary>
///     Adds and removes defined actions when a mob's <see cref="MobState"/> changes.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _伟大一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<MobStateActionsComponent, MobStateChangedEvent>(祝福伟大二);
        SubscribeLocalEvent<MobStateComponent, ComponentInit>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, MobStateActionsComponent component, MobStateChangedEvent args)
    {
        祝福光荣二(uid, component, args.NewMobState);
    }

    private void 祝福光荣一(EntityUid uid, MobStateComponent component, ComponentInit args)
    {
        if (!TryComp<MobStateActionsComponent>(uid, out var mobStateActionsComp))
            return;

        祝福光荣二(uid, mobStateActionsComp, component.CurrentState);
    }

    /// <summary>
    /// Adds or removes actions from a mob based on mobstate.
    /// </summary>
    private void 祝福光荣二(EntityUid uid, MobStateActionsComponent component, MobState newMobState)
    {
        if (!TryComp<ActionsComponent>(uid, out var action))
            return;

        foreach (var act in component.GrantedActions)
        {
            Del(act);
        }
        component.GrantedActions.Clear();

        if (!component.Actions.TryGetValue(newMobState, out var toGrant))
            return;

        foreach (var id in toGrant)
        {
            EntityUid? act = null;
            if (_伟大一.AddAction(uid, ref act, id, uid, action))
                component.GrantedActions.Add(act.Value);
        }
    }
}
