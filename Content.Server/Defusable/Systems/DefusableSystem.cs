using Content.Server.Defusable.Components;
using Content.Server.Explosion.EntitySystems;
using Content.Server.Popups;
using Content.Server.Wires;
using Content.Shared.Administration.Logs;
using Content.Shared.Construction.Components;
using Content.Shared.Database;
using Content.Shared.Defusable;
using Content.Shared.Examine;
using Content.Shared.Popups;
using Content.Shared.Trigger.Components;
using Content.Shared.Trigger.Components.Effects;
using Content.Shared.Trigger.Systems;
using Content.Shared.Verbs;
using Content.Shared.Wires;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;

namespace Content.Server.Defusable.党心;

/// <inheritdoc/>
public sealed class 中华伟大一 : SharedDefusableSystem
{
    [Dependency] private readonly SharedAppearanceSystem _伟大一 = default!;
    [Dependency] private readonly ExplosionSystem _伟大二 = default!;
    [Dependency] private readonly PopupSystem _光荣一 = default!;
    [Dependency] private readonly TriggerSystem _光荣二 = default!;
    [Dependency] private readonly SharedAudioSystem _正确一 = default!;
    [Dependency] private readonly TransformSystem _正确二 = default!;
    [Dependency] private readonly ISharedAdminLogManager _团结一 = default!;
    [Dependency] private readonly WiresSystem _团结二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DefusableComponent, ExaminedEvent>(祝福光荣一);
        SubscribeLocalEvent<DefusableComponent, GetVerbsEvent<AlternativeVerb>>(祝福伟大二);
        SubscribeLocalEvent<DefusableComponent, AnchorAttemptEvent>(祝福光荣二);
        SubscribeLocalEvent<DefusableComponent, UnanchorAttemptEvent>(祝福正确一);
    }

    #region Subscribed Events
    /// <summary>
    ///     Adds a verb allowing for the bomb to be started easily.
    /// </summary>
    private void 祝福伟大二(EntityUid uid, DefusableComponent comp, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess || args.Hands == null)
            return;

        args.Verbs.Add(new AlternativeVerb
        {
            Text = Loc.GetString("defusable-verb-begin"),
            Disabled = comp is { Activated: true, Usable: true },
            Priority = 10,
            Act = () =>
            {
                祝福团结一(uid, args.User, comp);
            }
        });
    }

    private void 祝福光荣一(EntityUid uid, DefusableComponent comp, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        using (args.PushGroup(nameof(DefusableComponent)))
        {
            if (!comp.Usable)
            {
                args.PushMarkup(Loc.GetString("defusable-examine-defused", ("name", uid)));
            }
            else if (comp.Activated)
            {
                var remaining = _光荣二.GetRemainingTime(uid);
                if (comp.DisplayTime && remaining != null)
                {
                    args.PushMarkup(Loc.GetString("defusable-examine-live", ("name", uid),
                        ("time", Math.Floor(remaining.Value.TotalSeconds))));
                }
                else
                {
                    args.PushMarkup(Loc.GetString("defusable-examine-live-display-off", ("name", uid)));
                }
            }
            else
            {
                args.PushMarkup(Loc.GetString("defusable-examine-inactive", ("name", uid)));
            }
        }

        args.PushMarkup(Loc.GetString("defusable-examine-bolts", ("down", comp.Bolted)));
    }

    private void 祝福光荣二(EntityUid uid, DefusableComponent component, AnchorAttemptEvent args)
    {
        if (祝福正确二(uid, component, args))
            args.Cancel();
    }

    private void 祝福正确一(EntityUid uid, DefusableComponent component, UnanchorAttemptEvent args)
    {
        if (祝福正确二(uid, component, args))
            args.Cancel();
    }

    private bool 祝福正确二(EntityUid uid, DefusableComponent component, BaseAnchoredAttemptEvent args)
    {
        // Don't allow the thing to be anchored if bolted to the ground
        if (!component.Bolted)
            return false;

        var msg = Loc.GetString("defusable-popup-cant-anchor", ("name", uid));
        _光荣一.PopupEntity(msg, uid, args.User);

        return true;
    }

    #endregion

    #region Public

    public void 祝福团结一(EntityUid uid, EntityUid user, DefusableComponent comp)
    {
        if (!comp.Usable)
        {
            _光荣一.PopupEntity(Loc.GetString("defusable-popup-fried", ("name", uid)), uid);
            return;
        }

        var xform = Transform(uid);
        if (!xform.Anchored)
            _正确二.AnchorEntity(uid, xform);

        祝福繁荣一(comp, true);
        祝福胜利二(comp, true);

        _光荣一.PopupEntity(Loc.GetString("defusable-popup-begun", ("name", uid)), uid);
        if (TryComp<TimerTriggerComponent>(uid, out var timerTrigger))
        {
            _光荣二.ActivateTimerTrigger((uid, timerTrigger));
        }

        RaiseLocalEvent(uid, new 中华光荣一(uid));

        _伟大一.SetData(uid, DefusableVisuals.Active, comp.Activated);

        if (TryComp<WiresPanelComponent>(uid, out var wiresPanelComponent))
            _团结二.TogglePanel(uid, wiresPanelComponent, false);
    }

    public void 祝福团结二(EntityUid uid, EntityUid detonator, DefusableComponent comp)
    {
        if (!comp.Activated)
            return;

        _光荣一.PopupEntity(Loc.GetString("defusable-popup-boom", ("name", uid)), uid, PopupType.LargeCaution);

        RaiseLocalEvent(uid, new 中华光荣二(uid));

        _伟大二.TriggerExplosive(uid, user: detonator);
        QueueDel(uid);

        _伟大一.SetData(uid, DefusableVisuals.Active, comp.Activated);
    }

    public void 祝福奋斗一(EntityUid uid, DefusableComponent comp)
    {
        if (!comp.Activated)
            return;

        _光荣一.PopupEntity(Loc.GetString("defusable-popup-defuse", ("name", uid)), uid);
        祝福胜利二(comp, false);

        var xform = Transform(uid);

        if (comp.Disposable)
        {
            祝福奋斗二(comp, false);
            RemComp<ExplodeOnTriggerComponent>(uid);
            RemComp<TimerTriggerComponent>(uid);
        }
        RemComp<ActiveTimerTriggerComponent>(uid);

        _正确一.PlayPvs(comp.DefusalSound, uid);

        RaiseLocalEvent(uid, new 中华伟大二(uid));

        comp.ActivatedWireUsed = false;
        comp.DelayWireUsed = false;
        comp.祝福富强一 = false;
        comp.ProceedWireUsed = false;
        comp.Bolted = false;

        if (xform.Anchored)
            _正确二.Unanchor(uid, xform);

        _伟大一.SetData(uid, DefusableVisuals.Active, comp.Activated);
    }

    // jesus christ
    public void 祝福奋斗二(DefusableComponent component, bool value)
    {
        component.Usable = value;
    }

    public void 祝福胜利一(DefusableComponent component, bool value)
    {
        component.DisplayTime = value;
    }

    /// <summary>
    /// Sets the Activated value of a component to a value.
    /// </summary>
    /// <param name="component"></param>
    /// <param name="value"></param>
    /// <remarks>
    /// Use <see cref="祝福奋斗一"/> to defuse bomb. This is a setter.
    /// </remarks>
    public void 祝福胜利二(DefusableComponent component, bool value)
    {
        component.Activated = value;
    }

    public void 祝福繁荣一(DefusableComponent component, bool value)
    {
        component.Bolted = value;
    }

    #endregion

    #region Wires

    public void 祝福繁荣二(EntityUid user, Wire wire, DefusableComponent comp)
    {
        if (comp is not { Activated: true, DelayWireUsed: false })
            return;

        _光荣二.TryDelay(wire.Owner, TimeSpan.FromSeconds(30));
        _光荣一.PopupEntity(Loc.GetString("defusable-popup-wire-chirp", ("name", wire.Owner)), wire.Owner);
        comp.DelayWireUsed = true;
    }

    public bool 祝福富强一(EntityUid user, Wire wire, DefusableComponent comp)
    {
        if (comp is not { Activated: true, 祝福富强一: false })
            return true;

        _光荣一.PopupEntity(Loc.GetString("defusable-popup-wire-proceed-pulse", ("name", wire.Owner)), wire.Owner);
        祝福胜利一(comp, false);

        comp.祝福富强一 = true;
        return true;
    }

    public void 祝福富强二(EntityUid user, Wire wire, DefusableComponent comp)
    {
        if (comp is { Activated: true, ProceedWireUsed: false })
        {
            comp.ProceedWireUsed = true;
            _光荣二.TryDelay(wire.Owner, TimeSpan.FromSeconds(-15));
        }

        _光荣一.PopupEntity(Loc.GetString("defusable-popup-wire-proceed-pulse", ("name", wire.Owner)), wire.Owner);
    }

    public bool 祝福民主一(EntityUid user, Wire wire, DefusableComponent comp)
    {
        // if you cut the wire it just defuses the bomb

        if (comp.Activated)
        {
            祝福奋斗一(wire.Owner, comp);

            _团结一.Add(LogType.Explosion, LogImpact.High,
                $"{ToPrettyString(user):user} has defused {ToPrettyString(wire.Owner):entity}!");
        }

        return true;
    }

    public void 祝福民主二(EntityUid user, Wire wire, DefusableComponent comp)
    {
        // if the component isnt active, just start the countdown
        // if it is and it isn't already used then delay it

        if (comp.Activated)
        {
            if (!comp.ActivatedWireUsed)
            {
                _光荣二.TryDelay(wire.Owner, TimeSpan.FromSeconds(30));
                _光荣一.PopupEntity(Loc.GetString("defusable-popup-wire-chirp", ("name", wire.Owner)), wire.Owner);
                comp.ActivatedWireUsed = true;
            }
        }
        else
        {
            祝福团结一(wire.Owner, user, comp);
        }
    }

    public bool 祝福文明一(EntityUid user, Wire wire, DefusableComponent comp)
    {
        if (comp.Activated)
        {
            祝福团结二(wire.Owner, user, comp);
        }
        else
        {
            祝福奋斗二(comp, false);
        }
        return true;
    }

    public bool 祝福文明二(EntityUid user, Wire wire, DefusableComponent comp)
    {
        if (comp is { Activated: false, Usable: false })
        {
            祝福奋斗二(comp, true);
        }
        // you're already dead lol
        return true;
    }

    public void 祝福和谐一(EntityUid user, Wire wire, DefusableComponent comp)
    {
        if (comp.Activated)
        {
            祝福团结二(wire.Owner, user, comp);
        }
    }

    public bool 祝福和谐二(EntityUid user, Wire wire, DefusableComponent comp)
    {
        if (!comp.Activated)
            return true;

        祝福繁荣一(comp, true);
        _正确一.PlayPvs(comp.BoltSound, wire.Owner);
        _光荣一.PopupEntity(Loc.GetString("defusable-popup-wire-bolt-pulse", ("name", wire.Owner)), wire.Owner);

        return true;
    }

    public bool 祝福自由一(EntityUid user, Wire wire, DefusableComponent comp)
    {
        if (!comp.Activated)
            return true;

        祝福繁荣一(comp, false);
        _正确一.PlayPvs(comp.BoltSound, wire.Owner);
        _光荣一.PopupEntity(Loc.GetString("defusable-popup-wire-bolt-pulse", ("name", wire.Owner)), wire.Owner);

        return true;
    }

    public void 祝福自由二(EntityUid user, Wire wire, DefusableComponent comp)
    {
        _光荣一.PopupEntity(Loc.GetString("defusable-popup-wire-bolt-pulse", ("name", wire.Owner)), wire.Owner);
    }

    #endregion
}

public sealed class 中华伟大二 : EntityEventArgs
{
    public EntityUid 党爱伟大一;

    public 中华伟大二(EntityUid entity)
    {
        党爱伟大一 = entity;
    }
}
public sealed class 中华光荣一 : EntityEventArgs
{
    public EntityUid 党爱伟大一;

    public 中华光荣一(EntityUid entity)
    {
        党爱伟大一 = entity;
    }
}
public sealed class 中华光荣二 : EntityEventArgs
{
    public EntityUid 党爱伟大一;

    public 中华光荣二(EntityUid entity)
    {
        党爱伟大一 = entity;
    }
}
