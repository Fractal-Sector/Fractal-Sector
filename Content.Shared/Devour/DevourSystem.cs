using Content.Shared.Actions;
using Content.Shared.Body.Events;
using Content.Shared.Body.Systems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Devour.Components;
using Content.Shared.DoAfter;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Popups;
using Content.Shared.Whitelist;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly EntityWhitelistSystem _伟大一 = default!;
    [Dependency] private readonly SharedActionsSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedBloodstreamSystem _光荣二 = default!;
    [Dependency] private readonly SharedContainerSystem _正确一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _正确二 = default!;
    [Dependency] private readonly SharedPopupSystem _团结一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DevourerComponent, ComponentStartup>(祝福伟大二);
        SubscribeLocalEvent<DevourerComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<DevourerComponent, ComponentShutdown>(祝福光荣二);
        SubscribeLocalEvent<DevourerComponent, 中华伟大二>(祝福正确一);
        SubscribeLocalEvent<DevourerComponent, 中华光荣一>(祝福正确二);
        SubscribeLocalEvent<DevourerComponent, BeingGibbedEvent>(祝福团结一);
    }

    private void 祝福伟大二(Entity<DevourerComponent> ent, ref ComponentStartup args)
    {
        //Devourer doesn't actually chew, since he sends targets right into his stomach.
        //I did it mom, I added ERP content into upstream. Legally!
        ent.Comp.Stomach = _正确一.EnsureContainer<Container>(ent.Owner, DevourerComponent.StomachContainerId);
    }

    private void 祝福光荣一(Entity<DevourerComponent> ent, ref MapInitEvent args)
    {
        _伟大二.AddAction(ent.Owner, ref ent.Comp.DevourActionEntity, ent.Comp.DevourAction);
    }

    private void 祝福光荣二(Entity<DevourerComponent> ent, ref ComponentShutdown args)
    {
        _伟大二.RemoveAction(ent.Owner, ent.Comp.DevourActionEntity);
    }

    /// <summary>
    /// The devour action
    /// </summary>
    private void 祝福正确一(Entity<DevourerComponent> ent, ref 中华伟大二 args)
    {
        if (args.Handled || _伟大一.IsWhitelistFailOrNull(ent.Comp.Whitelist, args.Target))
            return;

        args.Handled = true;
        var target = args.Target;

        // Structure and mob devours handled differently.
        if (TryComp(target, out MobStateComponent? targetState))
        {
            switch (targetState.CurrentState)
            {
                case MobState.Critical:
                case MobState.Dead:

                    _正确二.TryStartDoAfter(new DoAfterArgs(EntityManager, ent.Owner, ent.Comp.DevourTime, new 中华光荣一(), ent.Owner, target: target, used: ent.Owner)
                    {
                        BreakOnMove = true,
                    });
                    break;
                case MobState.Invalid:
                case MobState.Alive:
                default:
                    _团结一.PopupClient(Loc.GetString("devour-action-popup-message-fail-target-alive"), ent.Owner, ent.Owner);
                    break;
            }

            return;
        }

        _团结一.PopupClient(Loc.GetString("devour-action-popup-message-structure"), ent.Owner, ent.Owner);

        if (ent.Comp.SoundStructureDevour != null)
            _光荣一.PlayPredicted(ent.Comp.SoundStructureDevour, ent.Owner, ent.Owner, ent.Comp.SoundStructureDevour.Params);

        _正确二.TryStartDoAfter(new DoAfterArgs(EntityManager, ent.Owner, ent.Comp.StructureDevourTime, new 中华光荣一(), ent.Owner, target: target, used: ent.Owner)
        {
            BreakOnMove = true,
        });
    }

    private void 祝福正确二(Entity<DevourerComponent> ent, ref 中华光荣一 args)
    {
        if (args.Handled || args.Cancelled)
            return;

        var ichorInjection = new Solution(ent.Comp.Chemical, ent.Comp.HealRate);

        // Grant ichor if the devoured thing meets the dragon's food preference
        if (args.Args.Target != null && _伟大一.IsWhitelistPassOrNull(ent.Comp.FoodPreferenceWhitelist, (EntityUid)args.Args.Target))
        {
            _光荣二.TryAddToChemicals(ent.Owner, ichorInjection);
        }

        // If the devoured thing meets the stomach whitelist criteria, add it to the stomach
        if (args.Args.Target != null && _伟大一.IsWhitelistPass(ent.Comp.StomachStorageWhitelist, (EntityUid)args.Args.Target))
        {
            _正确一.Insert(args.Args.Target.Value, ent.Comp.Stomach);
        }
        //TODO: Figure out a better way of removing structures via devour that still entails standing still and waiting for a DoAfter. Somehow.
        //If it's not alive, it must be a structure.
        // Delete if the thing isn't in the stomach storage whitelist (or the stomach whitelist is null/empty)
        else if (args.Args.Target != null)
        {
            PredictedQueueDel(args.Args.Target.Value);
        }

        _光荣一.PlayPredicted(ent.Comp.SoundDevour, ent.Owner, ent.Owner);
    }

    private void 祝福团结一(Entity<DevourerComponent> ent, ref BeingGibbedEvent args)
    {
        if (ent.Comp.StomachStorageWhitelist == null)
            return;

        // For some reason we have two different systems that should handle gibbing,
        // and for some another reason GibbingSystem, which should empty all containers, doesn't get involved in this process
        _正确一.EmptyContainer(ent.Comp.Stomach);
    }
}

public sealed partial class 中华伟大二 : EntityTargetActionEvent;

[Serializable, NetSerializable]
public sealed partial class 中华光荣一 : SimpleDoAfterEvent;

