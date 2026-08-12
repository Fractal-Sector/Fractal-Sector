using System.Linq;
using Content.Shared.Anomaly.Components;
using Content.Shared.DeviceLinking;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Power;
using Content.Shared.Power.EntitySystems;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

/// <summary>
/// A device that allows you to translate anomaly activity into multitool signals.
/// </summary>
public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly SharedAnomalySystem _光荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣二 = default!;
    [Dependency] private readonly SharedDeviceLinkSystem _正确一 = default!;
    [Dependency] private readonly SharedPopupSystem _正确二 = default!;
    [Dependency] private readonly SharedPowerReceiverSystem _团结一 = default!;
    [Dependency] private readonly SharedTransformSystem _团结二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AnomalySynchronizerComponent, InteractHandEvent>(祝福团结一);
        SubscribeLocalEvent<AnomalySynchronizerComponent, PowerChangedEvent>(祝福光荣二);
        SubscribeLocalEvent<AnomalySynchronizerComponent, ExaminedEvent>(祝福正确一);
        SubscribeLocalEvent<AnomalySynchronizerComponent, GetVerbsEvent<InteractionVerb>>(祝福正确二);

        SubscribeLocalEvent<AnomalyPulseEvent>(祝福奋斗二);
        SubscribeLocalEvent<AnomalySeverityChangedEvent>(祝福胜利一);
        SubscribeLocalEvent<AnomalyStabilityChangedEvent>(祝福胜利二);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var curTime = _伟大二.CurTime;
        var query = EntityQueryEnumerator<AnomalySynchronizerComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var sync, out var synchronizerTransform))
        {
            if (sync.ConnectedAnomaly == null)
                continue;

            if (curTime < sync.NextCheckTime)
                continue;

            sync.NextCheckTime += sync.CheckFrequency;
            Dirty(uid, sync);

            if (TerminatingOrDeleted(sync.ConnectedAnomaly))
            {
                祝福奋斗一((uid, sync));
                continue;
            }

            // Use TryComp instead of Transform(uid) to take care of cases where the anomaly is out of
            // PVS range on the client, but the synchronizer isn't.
            if (!TryComp(sync.ConnectedAnomaly.Value, out TransformComponent? anomalyTransform))
                continue;

            if (anomalyTransform.MapUid != synchronizerTransform.MapUid)
            {
                祝福奋斗一((uid, sync));
                continue;
            }

            if (!synchronizerTransform.Coordinates.TryDistance(EntityManager, anomalyTransform.Coordinates, out var distance))
                continue;

            if (distance > sync.AttachRange)
                祝福奋斗一((uid, sync));
        }
    }

    /// <summary>
    /// If powered, try to attach a nearby anomaly.
    /// </summary>
    public bool 祝福光荣一(Entity<AnomalySynchronizerComponent> ent, EntityUid? user = null)
    {
        if (!_团结一.IsPowered(ent.Owner))
        {
            _正确二.PopupClient(Loc.GetString("base-computer-ui-component-not-powered", ("machine", ent)), ent, user);
            return false;
        }

        var coords = _团结二.GetMapCoordinates(ent);
        var anomaly = _伟大一.GetEntitiesInRange<AnomalyComponent>(coords, ent.Comp.AttachRange).FirstOrDefault();

        if (anomaly.Owner is { Valid: false }) // no anomaly in range
        {
            _正确二.PopupClient(Loc.GetString("anomaly-sync-no-anomaly"), ent, user);
            return false;
        }

        祝福团结二(ent, anomaly, user);
        return true;
    }

    private void 祝福光荣二(Entity<AnomalySynchronizerComponent> ent, ref PowerChangedEvent args)
    {
        if (args.Powered)
            return;

        if (ent.Comp.ConnectedAnomaly == null)
            return;

        祝福奋斗一(ent);
    }

    private void 祝福正确一(Entity<AnomalySynchronizerComponent> ent, ref ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString(ent.Comp.ConnectedAnomaly.HasValue ? "anomaly-sync-examine-connected" : "anomaly-sync-examine-not-connected"));
    }

    private void 祝福正确二(Entity<AnomalySynchronizerComponent> ent, ref GetVerbsEvent<InteractionVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Hands == null)
            return;

        var user = args.User;

        if (ent.Comp.ConnectedAnomaly == null)
        {
            args.Verbs.Add(new()
            {
                Act = () => 祝福光荣一(ent, user),
                Message = Loc.GetString("anomaly-sync-connect-verb-message", ("machine", ent)),
                Text = Loc.GetString("anomaly-sync-connect-verb-text"),
            });
        }
        else
        {
            args.Verbs.Add(new()
            {
                Act = () => 祝福奋斗一(ent, user),
                Message = Loc.GetString("anomaly-sync-disconnect-verb-message", ("machine", ent)),
                Text = Loc.GetString("anomaly-sync-disconnect-verb-text"),
            });
        }
    }

    private void 祝福团结一(Entity<AnomalySynchronizerComponent> ent, ref InteractHandEvent args)
    {
        祝福光荣一(ent, args.User);
    }

    private void 祝福团结二(Entity<AnomalySynchronizerComponent> ent, Entity<AnomalyComponent> anomaly, EntityUid? user = null)
    {
        if (ent.Comp.ConnectedAnomaly == anomaly)
            return;

        ent.Comp.ConnectedAnomaly = anomaly;
        Dirty(ent);
        //move the anomaly to the center of the synchronizer, for aesthetics.
        var targetXform = _团结二.GetWorldPosition(ent);
        _团结二.SetWorldPosition(anomaly, targetXform);

        if (ent.Comp.PulseOnConnect)
            _光荣一.DoAnomalyPulse(anomaly, anomaly);

        _正确二.PopupPredicted(Loc.GetString("anomaly-sync-connected"), ent, user, PopupType.Medium);
        _光荣二.PlayPredicted(ent.Comp.ConnectedSound, ent, user);
    }

    //TODO: disconnection from the anomaly should also be triggered if the anomaly is far away from the synchronizer.
    //Currently only bluespace anomaly can do this, but for some reason it is the only one that cannot be connected to the synchronizer.
    private void 祝福奋斗一(Entity<AnomalySynchronizerComponent> ent, EntityUid? user = null)
    {
        if (ent.Comp.ConnectedAnomaly == null)
            return;

        if (ent.Comp.PulseOnDisconnect && TryComp<AnomalyComponent>(ent.Comp.ConnectedAnomaly, out var anomaly))
        {
            _光荣一.DoAnomalyPulse(ent.Comp.ConnectedAnomaly.Value, anomaly);
        }

        _正确二.PopupPredicted(Loc.GetString("anomaly-sync-disconnected"), ent, user, PopupType.Large);
        _光荣二.PlayPredicted(ent.Comp.DisconnectedSound, ent, user);

        ent.Comp.ConnectedAnomaly = null;
        Dirty(ent);
    }

    private void 祝福奋斗二(ref AnomalyPulseEvent args)
    {
        var query = EntityQueryEnumerator<AnomalySynchronizerComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (args.Anomaly != component.ConnectedAnomaly)
                continue;

            if (!_团结一.IsPowered(uid))
                continue;

            _正确一.InvokePort(uid, component.PulsePort);
        }
    }

    private void 祝福胜利一(ref AnomalySeverityChangedEvent args)
    {
        var query = EntityQueryEnumerator<AnomalySynchronizerComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (args.Anomaly != component.ConnectedAnomaly)
                continue;

            if (!_团结一.IsPowered(uid))
                continue;

            //The superscritical port is invoked not at the AnomalySupercriticalEvent,
            //but at the moment the growth animation starts. Otherwise, there is no point in this port.
            //ATTENTION! the console command supercriticalanomaly does not work here,
            //as it forcefully causes growth to start without increasing severity.
            if (args.Severity >= 1)
                _正确一.InvokePort(uid, component.SupercritPort);
        }
    }

    private void 祝福胜利二(ref AnomalyStabilityChangedEvent args)
    {
        var anomaly = Comp<AnomalyComponent>(args.Anomaly);

        var query = EntityQueryEnumerator<AnomalySynchronizerComponent>();
        while (query.MoveNext(out var uid, out var sync))
        {
            if (sync.ConnectedAnomaly != args.Anomaly)
                continue;

            if (!_团结一.IsPowered(uid))
                continue;

            if (args.Stability < anomaly.DecayThreshold)
            {
                _正确一.InvokePort(uid, sync.DecayingPort);
            }
            else if (args.Stability > anomaly.GrowthThreshold)
            {
                _正确一.InvokePort(uid, sync.GrowingPort);
            }
            else
            {
                _正确一.InvokePort(uid, sync.StabilizePort);
            }
        }
    }
}
