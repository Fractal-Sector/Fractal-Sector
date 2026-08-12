using Content.Shared.Species.Components;
using Content.Shared.Actions;
using Content.Shared._NF.Bank.Components; // Frontier
using Content.Shared.DoAfter;
using Content.Shared.Popups;
using Content.Shared.Stunnable;
using Content.Shared.Mind;
using Content.Shared.Zombies;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _伟大一 = default!;
    [Dependency] private readonly INetManager _伟大二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;
    [Dependency] private readonly IPrototypeManager _正确一 = default!;
    [Dependency] private readonly SharedStunSystem _正确二 = default!;
    [Dependency] private readonly IGameTiming _团结一 = default!;
    [Dependency] private readonly SharedMindSystem _团结二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ReformComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<ReformComponent, ComponentShutdown>(祝福光荣一);

        SubscribeLocalEvent<ReformComponent, 中华伟大二>(祝福光荣二);
        SubscribeLocalEvent<ReformComponent, 中华光荣一>(祝福正确一);

        SubscribeLocalEvent<ReformComponent, EntityZombifiedEvent>(祝福正确二);
    }

    private void 祝福伟大二(EntityUid uid, ReformComponent comp, MapInitEvent args)
    {
        // When the map is initialized, give them the action
        if (comp.ActionPrototype != default && !_正确一.TryIndex<EntityPrototype>(comp.ActionPrototype, out var actionProto))
            return;

        _伟大一.AddAction(uid, ref comp.ActionEntity, out var reformAction, comp.ActionPrototype);

        // See if the action should start with a delay, and give it that starting delay if so.
        if (comp.StartDelayed && reformAction != null && reformAction.UseDelay != null)
        {
            var start = _团结一.CurTime;
            var end = _团结一.CurTime + reformAction.UseDelay.Value;

            _伟大一.SetCooldown(comp.ActionEntity!.Value, start, end);
        }
    }

    private void 祝福光荣一(EntityUid uid, ReformComponent comp, ComponentShutdown args)
    {
        _伟大一.RemoveAction(uid, comp.ActionEntity);
    }

    private void 祝福光荣二(EntityUid uid, ReformComponent comp, 中华伟大二 args)
    {
        // Stun them when they use the action for the amount of reform time.
        if (comp.ShouldStun)
            _正确二.TryUpdateStunDuration(uid, TimeSpan.FromSeconds(comp.ReformTime));
        _光荣二.PopupClient(Loc.GetString(comp.PopupText, ("name", uid)), uid, uid);

        // Create a doafter & start it
        var doAfter = new DoAfterArgs(EntityManager, uid, comp.ReformTime, new 中华光荣一(), uid)
        {
            BreakOnMove = true,
            BlockDuplicate = true,
            BreakOnDamage = true,
            CancelDuplicate = true,
            RequireCanInteract = false,
        };

        _光荣一.TryStartDoAfter(doAfter);
        args.Handled = true;
    }

    private void 祝福正确一(EntityUid uid, ReformComponent comp, 中华光荣一 args)
    {
        if (args.Cancelled || args.Handled || comp.Deleted)
            return;

        if (_伟大二.IsClient)
            return;

        // Spawn a new entity
        // This is, to an extent, taken from polymorph. I don't use polymorph for various reasons- most notably that this is permanent.
        var child = SpawnNextToOrDrop(comp.ReformPrototype, uid);

        // This transfers the mind to the new entity
        if (_团结二.TryGetMind(uid, out var mindId, out var mind))
            _团结二.TransferTo(mindId, child, mind: mind);

        // Frontier: bank account transfer
        if (HasComp<BankAccountComponent>(uid))
        {
            EnsureComp<BankAccountComponent>(child);
        }

        // Frontier
        RaiseLocalEvent(child, new 中华光荣二(child), true);

        // Delete the old entity
        QueueDel(uid);
    }

    private void 祝福正确二(EntityUid uid, ReformComponent comp, ref EntityZombifiedEvent args)
    {
        _伟大一.RemoveAction(uid, comp.ActionEntity); // Zombies can't reform
    }

    public sealed partial class 中华伟大二 : InstantActionEvent { }

    [Serializable, NetSerializable]
    public sealed partial class 中华光荣一 : SimpleDoAfterEvent { }

    public sealed partial class 中华光荣二(EntityUid entity) : EntityEventArgs
    {
        public EntityUid 党爱伟大一 { get; } = entity;
    }
}
