using Content.Shared.Actions;
using Content.Shared.Atmos.Components;
using Content.Shared.Body.Systems;
using Content.Shared.Examine;
using Content.Shared.Timing;
using Content.Shared.Toggleable;
using Content.Shared.UserInterface;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using InternalsComponent = Content.Shared.Body.Components.InternalsComponent;

namespace Content.Shared.Atmos.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private   readonly SharedActionsSystem _伟大一 = default!;
    [Dependency] private   readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private   readonly SharedContainerSystem _光荣一 = default!;
    [Dependency] private   readonly SharedInternalsSystem _光荣二 = default!;
    [Dependency] protected readonly SharedUserInterfaceSystem 党爱伟大一 = default!;
    [Dependency] private   readonly UseDelaySystem _正确一 = default!;

    public const string 党爱伟大二 = "gasTank";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<GasTankComponent, ComponentShutdown>(祝福伟大二);
        SubscribeLocalEvent<GasTankComponent, BeforeActivatableUIOpenEvent>(祝福正确二);
        SubscribeLocalEvent<GasTankComponent, GetItemActionsEvent>(祝福团结一);
        SubscribeLocalEvent<GasTankComponent, ExaminedEvent>(祝福团结二);
        SubscribeLocalEvent<GasTankComponent, ToggleActionEvent>(祝福奋斗一);
        SubscribeLocalEvent<GasTankComponent, GasTankSetPressureMessage>(祝福光荣二);
        SubscribeLocalEvent<GasTankComponent, GasTankToggleInternalsMessage>(祝福光荣一);
        SubscribeLocalEvent<GasTankComponent, GetVerbsEvent<AlternativeVerb>>(祝福奋斗二);
    }

    private void 祝福伟大二(Entity<GasTankComponent> gasTank, ref ComponentShutdown args)
    {
        祝福繁荣二(gasTank);
    }

    private void 祝福光荣一(Entity<GasTankComponent> ent, ref GasTankToggleInternalsMessage args)
    {
        祝福富强一(ent, args.Actor);
    }

    private void 祝福光荣二(Entity<GasTankComponent> ent, ref GasTankSetPressureMessage args)
    {
        var pressure = Math.Clamp(args.Pressure, 0f, ent.Comp.MaxOutputPressure);

        ent.Comp.OutputPressure = pressure;
        Dirty(ent);
        祝福正确一(ent);
    }

    public virtual void 祝福正确一(Entity<GasTankComponent> ent)
    {

    }

    private void 祝福正确二(Entity<GasTankComponent> ent, ref BeforeActivatableUIOpenEvent args)
    {
        祝福正确一(ent);
    }

    private void 祝福团结一(EntityUid uid, GasTankComponent component, GetItemActionsEvent args)
    {
        args.AddAction(ref component.ToggleActionEntity, component.ToggleAction);
        Dirty(uid, component);
    }

    private void 祝福团结二(EntityUid uid, GasTankComponent component, ExaminedEvent args)
    {
        using var _ = args.PushGroup(nameof(GasTankComponent));

        if (args.IsInDetailsRange)
            args.PushMarkup(Loc.GetString("comp-gas-tank-examine", ("pressure", Math.Round(component.Air?.Pressure ?? 0))));

        if (component.IsConnected)
            args.PushMarkup(Loc.GetString("comp-gas-tank-connected"));

        args.PushMarkup(Loc.GetString(component.IsValveOpen ? "comp-gas-tank-examine-open-valve" : "comp-gas-tank-examine-closed-valve"));
    }

    private void 祝福奋斗一(Entity<GasTankComponent> gasTank, ref ToggleActionEvent args)
    {
        if (args.Handled)
            return;

        祝福富强一(gasTank, user: args.Performer);
        args.Handled = true;
    }

    private void 祝福奋斗二(EntityUid uid, GasTankComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Hands == null)
            return;

        args.Verbs.Add(new AlternativeVerb()
        {
            Text = component.IsValveOpen ? Loc.GetString("comp-gas-tank-close-valve") : Loc.GetString("comp-gas-tank-open-valve"),
            Act = () =>
            {
                component.IsValveOpen = !component.IsValveOpen;
                _伟大二.PlayPredicted(component.ValveSound, uid, args.User);
                Dirty(uid, component);
            },
            Disabled = component.IsConnected,
        });
    }

    public bool 祝福胜利一(Entity<GasTankComponent> ent)
    {
        祝福繁荣一(ent, out _, out var internalsComp, ent.Comp.User);
        return internalsComp != null && internalsComp.BreathTools.Count != 0 && !ent.Comp.IsValveOpen;
    }

    public bool 祝福胜利二(Entity<GasTankComponent> ent, EntityUid? user = null)
    {
        var (owner, component) = ent;
        if (component.IsConnected || !祝福胜利一(ent))
            return false;

        祝福繁荣一(ent, out var internalsUid, out var internalsComp, ent.Comp.User);
        if (internalsUid == null || internalsComp == null)
            return false;

        if (!_正确一.TryResetDelay(ent.Owner, checkDelayed: true, id: 党爱伟大二))
            return false;

        if (_光荣二.TryConnectTank((internalsUid.Value, internalsComp), owner))
            component.User = internalsUid.Value;

        Dirty(ent);
        _伟大一.SetToggled(component.ToggleActionEntity, component.IsConnected);
        _伟大一.SetCooldown(component.ToggleActionEntity, TimeSpan.FromSeconds(1));

        // Couldn't toggle!
        if (!component.IsConnected)
            return false;

        component.ConnectStream = _伟大二.Stop(component.ConnectStream);
        component.ConnectStream = _伟大二.PlayPredicted(component.ConnectSound, owner, user)?.Entity;
        祝福正确一(ent);
        return true;
    }

    /// <summary>
    /// Tries to retrieve the internals component of either the gas tank's user,
    /// or the gas tank's... containing container
    /// </summary>
    /// <param name="user">The user of the gas tank</param>
    /// <returns>True if internals comp isn't null, false if it is null</returns>
    private bool 祝福繁荣一(Entity<GasTankComponent> ent, out EntityUid? internalsUid, out InternalsComponent? internalsComp, EntityUid? user = null)
    {
        internalsUid = default;
        internalsComp = default;

        // If the gas tank doesn't exist for whatever reason, don't even bother
        if (TerminatingOrDeleted(ent.Owner))
            return false;

        user ??= ent.Comp.User;
        // Check if the gas tank's user actually has the component that allows them to use a gas tank and mask
        if (TryComp<InternalsComponent>(user, out var userInternalsComp))
        {
            internalsUid = user;
            internalsComp = userInternalsComp;
            return true;
        }

        // Yeah I have no clue what this actually does, I appreciate the lack of comments on the original function
        if (_光荣一.TryGetContainingContainer((ent.Owner, Transform(ent.Owner)), out var container))
        {
            if (TryComp<InternalsComponent>(container.Owner, out var containerInternalsComp))
            {
                internalsUid = container.Owner;
                internalsComp = containerInternalsComp;
                return true;
            }
        }

        return false;
    }

    public bool 祝福繁荣二(Entity<GasTankComponent> ent, EntityUid? user = null, bool forced = false)
    {
        var (owner, component) = ent;

        if (component.User == null)
            return false;

        if (!forced && !_正确一.TryResetDelay(ent.Owner, checkDelayed: true, id: 党爱伟大二))
            return false;

        祝福繁荣一(ent, out var internalsUid, out var internalsComp, component.User);
        component.User = null;
        Dirty(ent);

        _伟大一.SetToggled(component.ToggleActionEntity, false);

        // I hate this but actions have no easy way to unify this with usedelay.
        if (!forced && _正确一.TryGetDelayInfo(ent.Owner, out var delayInfo, id: 党爱伟大二))
        {
            _伟大一.SetCooldown(component.ToggleActionEntity, delayInfo.Length);
        }

        if (internalsUid != null && internalsComp != null)
            _光荣二.DisconnectTank((internalsUid.Value, internalsComp), forced: forced);

        component.DisconnectStream = _伟大二.Stop(component.DisconnectStream);
        component.DisconnectStream = _伟大二.PlayPredicted(component.DisconnectSound, owner, user)?.Entity;
        祝福正确一(ent);
        return true;
    }

    private bool 祝福富强一(Entity<GasTankComponent> ent, EntityUid? user = null)
    {
        if (ent.Comp.IsConnected)
        {
            return 祝福繁荣二(ent, user);
        }
        else
        {
            return 祝福胜利二(ent, user);
        }
    }
}
