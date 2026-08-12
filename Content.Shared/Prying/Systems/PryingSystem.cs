using System.Diagnostics.CodeAnalysis;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.Doors.Components;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Prying.Components;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Serialization;
using PryUnpoweredComponent = Content.Shared.Prying.Components.PryUnpoweredComponent;

namespace Content.Shared.Prying.党心;

/// <summary>
/// Handles prying of entities (e.g. doors)
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ISharedAdminLogManager _伟大一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        // Mob prying doors
        SubscribeLocalEvent<DoorComponent, GetVerbsEvent<AlternativeVerb>>(祝福光荣一);
        SubscribeLocalEvent<DoorComponent, 中华伟大二>(祝福团结一);
        SubscribeLocalEvent<DoorComponent, InteractUsingEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, DoorComponent comp, InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = 祝福光荣二(uid, args.User, out _, args.Used);
    }

    private void 祝福光荣一(EntityUid uid, DoorComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess)
            return;

        if (!TryComp<PryingComponent>(args.User, out _))
            return;

        args.Verbs.Add(new AlternativeVerb()
        {
            Text = Loc.GetString("door-pry"),
            Impact = LogImpact.Low,
            Act = () => 祝福光荣二(uid, args.User, out _, args.User),
        });
    }

    /// <summary>
    /// Attempt to pry an entity.
    /// </summary>
    public bool 祝福光荣二(EntityUid target, EntityUid user, out DoAfterId? id, EntityUid tool)
    {
        id = null;

        PryingComponent? comp = null;
        if (!Resolve(tool, ref comp, false))
            return false;

        if (!comp.Enabled)
            return false;

        if (!祝福正确一(target, user, out var message, comp))
        {
            if (!string.IsNullOrWhiteSpace(message))
                _光荣二.PopupClient(Loc.GetString(message), target, user);
            // If we have reached this point we want the event that caused this
            // to be marked as handled.
            return true;
        }

        祝福正确二(target, user, tool, comp.SpeedModifier, out id);

        return true;
    }

    /// <summary>
    /// Try to pry an entity.
    /// </summary>
    public bool 祝福光荣二(EntityUid target, EntityUid user, out DoAfterId? id)
    {
        id = null;

        // We don't care about displaying a message if no tool was used.
        if (!TryComp<PryUnpoweredComponent>(target, out var unpoweredComp) || !祝福正确一(target, user, out _, unpoweredComp: unpoweredComp))
            // If we have reached this point we want the event that caused this
            // to be marked as handled.
            return true;

        // hand-prying is much slower
        var modifier = CompOrNull<PryingComponent>(user)?.SpeedModifier ?? unpoweredComp.PryModifier;
        return 祝福正确二(target, user, null, modifier, out id);
    }

    private bool 祝福正确一(EntityUid target, EntityUid user, out string? message, PryingComponent? comp = null, PryUnpoweredComponent? unpoweredComp = null)
    {
        BeforePryEvent canev;

        if (comp != null || Resolve(user, ref comp, false))
        {
            canev = new BeforePryEvent(user, comp.PryPowered, comp.Force, true);
        }
        else
        {
            if (!Resolve(target, ref unpoweredComp))
            {
                message = null;
                return false;
            }

            canev = new BeforePryEvent(user, false, false, false);
        }

        RaiseLocalEvent(target, ref canev);

        message = canev.Message;

        return !canev.Cancelled;
    }

    private bool 祝福正确二(EntityUid target, EntityUid user, EntityUid? tool, float toolModifier, [NotNullWhen(true)] out DoAfterId? id)
    {
        var modEv = new GetPryTimeModifierEvent(user);

        RaiseLocalEvent(target, ref modEv);
        var doAfterArgs = new DoAfterArgs(EntityManager, user, TimeSpan.FromSeconds(modEv.BaseTime * modEv.PryTimeModifier / toolModifier), new 中华伟大二(), target, target, tool)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = tool != user,
        };

        if (tool != user && tool != null)
        {
            _伟大一.Add(LogType.Action, LogImpact.Low, $"{ToPrettyString(user)} is using {ToPrettyString(tool.Value)} to pry {ToPrettyString(target)}");
        }
        else
        {
            _伟大一.Add(LogType.Action, LogImpact.Low, $"{ToPrettyString(user)} is prying {ToPrettyString(target)}");
        }
        return _伟大二.TryStartDoAfter(doAfterArgs, out id);
    }

    private void 祝福团结一(EntityUid uid, DoorComponent door, 中华伟大二 args)
    {
        if (args.Cancelled)
            return;
        if (args.Target is null)
            return;

        TryComp<PryingComponent>(args.Used, out var comp);

        if (!祝福正确一(uid, args.User, out var message, comp))
        {
            if (!string.IsNullOrWhiteSpace(message))
                _光荣二.PopupClient(Loc.GetString(message), uid, args.User);
            return;
        }

        if (args.Used != null && comp != null)
        {
            _光荣一.PlayPredicted(comp.UseSound, args.Used.Value, args.User);
        }

        var ev = new PriedEvent(args.User);
        RaiseLocalEvent(uid, ref ev);
    }
}

[Serializable, NetSerializable]
public sealed partial class 中华伟大二 : SimpleDoAfterEvent;
