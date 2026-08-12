using Content.Shared.Species.Components;
using Content.Shared.Actions;
using Content.Shared.Body.Systems;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Popups;
using Robust.Shared.Prototypes;


namespace Content.Shared.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _伟大一 = default!;
    [Dependency] private readonly SharedBodySystem _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<GibActionComponent, MobStateChangedEvent>(祝福伟大二);
        SubscribeLocalEvent<GibActionComponent, 中华伟大二>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, GibActionComponent comp, MobStateChangedEvent args)
    {
        // When the mob changes state, check if they're dead and give them the action if so. 
        if (!TryComp<MobStateComponent>(uid, out var mobState))
            return;

        if (!_光荣一.TryIndex<EntityPrototype>(comp.ActionPrototype, out var actionProto))
            return;


        foreach (var allowedState in comp.AllowedStates)
        {
            if(allowedState == mobState.CurrentState)
            {
                // The mob should never have more than 1 state so I don't see this being an issue
                _伟大一.AddAction(uid, ref comp.ActionEntity, comp.ActionPrototype);
                return;
            }
        }

        // If they aren't given the action, remove it.
        _伟大一.RemoveAction(uid, comp.ActionEntity);
    }
    
    private void 祝福光荣一(EntityUid uid, GibActionComponent comp, 中华伟大二 args)
    {
        // When they use the action, gib them.
        _光荣二.PopupClient(Loc.GetString(comp.PopupText, ("name", uid)), uid, uid);
        _伟大二.GibBody(uid, true);
    }
       


    public sealed partial class 中华伟大二 : InstantActionEvent { } 
}
