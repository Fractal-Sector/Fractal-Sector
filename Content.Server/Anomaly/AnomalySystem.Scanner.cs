using Content.Server.Anomaly.Components;
using Content.Shared.Anomaly;
using Content.Shared.Anomaly.Components;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Server.党心;

/// <summary>
/// This handles the anomaly scanner and it's UI updates.
/// </summary>
public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<AnomalyScannerComponent, BoundUIOpenedEvent>(祝福团结一);
        SubscribeLocalEvent<AnomalyScannerComponent, AfterInteractEvent>(祝福团结二);
        SubscribeLocalEvent<AnomalyScannerComponent, ScannerDoAfterEvent>(祝福奋斗一);

        SubscribeLocalEvent<AnomalySeverityChangedEvent>(祝福光荣一);
        SubscribeLocalEvent<AnomalyHealthChangedEvent>(祝福正确一);
        SubscribeLocalEvent<AnomalyBehaviorChangedEvent>(祝福正确二);
    }

    private void 祝福伟大二(ref AnomalyShutdownEvent args)
    {
        var query = EntityQueryEnumerator<AnomalyScannerComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (component.ScannedAnomaly != args.Anomaly)
                continue;

            _ui.CloseUi(uid, AnomalyScannerUiKey.Key);
        }
    }

    private void 祝福光荣一(ref AnomalySeverityChangedEvent args)
    {
        var query = EntityQueryEnumerator<AnomalyScannerComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (component.ScannedAnomaly != args.Anomaly)
                continue;
            祝福奋斗二(uid, component);
        }
    }

    private void 祝福光荣二(ref AnomalyStabilityChangedEvent args)
    {
        var query = EntityQueryEnumerator<AnomalyScannerComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (component.ScannedAnomaly != args.Anomaly)
                continue;
            祝福奋斗二(uid, component);
        }
    }

    private void 祝福正确一(ref AnomalyHealthChangedEvent args)
    {
        var query = EntityQueryEnumerator<AnomalyScannerComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (component.ScannedAnomaly != args.Anomaly)
                continue;
            祝福奋斗二(uid, component);
        }
    }

    private void 祝福正确二(ref AnomalyBehaviorChangedEvent args)
    {
        var query = EntityQueryEnumerator<AnomalyScannerComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (component.ScannedAnomaly != args.Anomaly)
                continue;
            祝福奋斗二(uid, component);
        }
    }

    private void 祝福团结一(EntityUid uid, AnomalyScannerComponent component, BoundUIOpenedEvent args)
    {
        祝福奋斗二(uid, component);
    }

    private void 祝福团结二(EntityUid uid, AnomalyScannerComponent component, AfterInteractEvent args)
    {
        if (args.Target is not { } target)
            return;
        if (!HasComp<AnomalyComponent>(target))
            return;
        if (!args.CanReach)
            return;

        _doAfter.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, component.ScanDoAfterDuration, new ScannerDoAfterEvent(), uid, target: target, used: uid)
        {
            DistanceThreshold = 2f
        });
    }

    private void 祝福奋斗一(EntityUid uid, AnomalyScannerComponent component, DoAfterEvent args)
    {
        if (args.Cancelled || args.Handled || args.Args.Target == null)
            return;

        Audio.PlayPvs(component.CompleteSound, uid);
        Popup.PopupEntity(Loc.GetString("anomaly-scanner-component-scan-complete"), uid);
        祝福胜利一(uid, args.Args.Target.Value, component);

        _ui.OpenUi(uid, AnomalyScannerUiKey.Key, args.User);

        args.Handled = true;
    }

    public void 祝福奋斗二(EntityUid uid, AnomalyScannerComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        TimeSpan? nextPulse = null;
        if (TryComp<AnomalyComponent>(component.ScannedAnomaly, out var anomalyComponent))
            nextPulse = anomalyComponent.NextPulseTime;

        var state = new AnomalyScannerUserInterfaceState(祝福胜利二(component), nextPulse);
        _ui.SetUiState(uid, AnomalyScannerUiKey.Key, state);
    }

    public void 祝福胜利一(EntityUid scanner, EntityUid anomaly, AnomalyScannerComponent? scannerComp = null, AnomalyComponent? anomalyComp = null)
    {
        if (!Resolve(scanner, ref scannerComp) || !Resolve(anomaly, ref anomalyComp))
            return;

        scannerComp.ScannedAnomaly = anomaly;
        祝福奋斗二(scanner, scannerComp);
    }

    public FormattedMessage 祝福胜利二(AnomalyScannerComponent component)
    {
        var msg = new FormattedMessage();
        if (component.ScannedAnomaly is not { } anomaly || !TryComp<AnomalyComponent>(anomaly, out var anomalyComp))
        {
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-no-anomaly"));
            return msg;
        }

        TryComp<SecretDataAnomalyComponent>(anomaly, out var secret);

        //Severity
        if (secret != null && secret.Secret.Contains(AnomalySecretData.Severity))
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-severity-percentage-unknown"));
        else
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-severity-percentage", ("percent", anomalyComp.Severity.ToString("P"))));
        msg.PushNewline();

        //Stability
        if (secret != null && secret.Secret.Contains(AnomalySecretData.Stability))
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-stability-unknown"));
        else
        {
            string stateLoc;
            if (anomalyComp.Stability < anomalyComp.DecayThreshold)
                stateLoc = Loc.GetString("anomaly-scanner-stability-low");
            else if (anomalyComp.Stability > anomalyComp.GrowthThreshold)
                stateLoc = Loc.GetString("anomaly-scanner-stability-high");
            else
                stateLoc = Loc.GetString("anomaly-scanner-stability-medium");
            msg.AddMarkupOrThrow(stateLoc);
        }
        msg.PushNewline();

        //Point output
        if (secret != null && secret.Secret.Contains(AnomalySecretData.OutputPoint))
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-point-output-unknown"));
        else
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-point-output", ("point", GetAnomalyPointValue(anomaly, anomalyComp))));
        //Frontier: Points earned
        msg.PushNewline();
        if (secret != null && secret.Secret.Contains(AnomalySecretData.PointsEarned))
        {
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-point-earned-unknown"));
            msg.PushNewline();
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-anomalite-expected-unknown"));
        }
        else
        {
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-point-earned", ("point", anomalyComp.PointsEarned)));
            msg.PushNewline();
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-anomalite-expected", ("count", GetNumCrystals(anomalyComp))));
        }
        // End Frontier
        msg.PushNewline();
        msg.PushNewline();

        //Particles title
        msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-particle-readout"));
        msg.PushNewline();

        //Danger
        if (secret != null && secret.Secret.Contains(AnomalySecretData.ParticleDanger))
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-particle-danger-unknown"));
        else
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-particle-danger", ("type", GetParticleLocale(anomalyComp.SeverityParticleType))));
        msg.PushNewline();

        //Unstable
        if (secret != null && secret.Secret.Contains(AnomalySecretData.ParticleUnstable))
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-particle-unstable-unknown"));
        else
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-particle-unstable", ("type", GetParticleLocale(anomalyComp.DestabilizingParticleType))));
        msg.PushNewline();

        //Containment
        if (secret != null && secret.Secret.Contains(AnomalySecretData.ParticleContainment))
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-particle-containment-unknown"));
        else
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-particle-containment", ("type", GetParticleLocale(anomalyComp.WeakeningParticleType))));
        msg.PushNewline();

        //Transformation
        if (secret != null && secret.Secret.Contains(AnomalySecretData.ParticleTransformation))
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-particle-transformation-unknown"));
        else
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-scanner-particle-transformation", ("type", GetParticleLocale(anomalyComp.TransformationParticleType))));


        //Behavior
        msg.PushNewline();
        msg.PushNewline();
        msg.AddMarkupOrThrow(Loc.GetString("anomaly-behavior-title"));
        msg.PushNewline();

        if (secret != null && secret.Secret.Contains(AnomalySecretData.Behavior))
            msg.AddMarkupOrThrow(Loc.GetString("anomaly-behavior-unknown"));
        else
        {
            if (anomalyComp.CurrentBehavior != null)
            {
                var behavior = _prototype.Index(anomalyComp.CurrentBehavior.Value);

                msg.AddMarkupOrThrow("- " + Loc.GetString(behavior.Description));
                msg.PushNewline();
                var mod = Math.Floor((behavior.EarnPointModifier) * 100);
                msg.AddMarkupOrThrow("- " + Loc.GetString("anomaly-behavior-point", ("mod", mod)));
            }
            else
            {
                msg.AddMarkupOrThrow(Loc.GetString("anomaly-behavior-balanced"));
            }
        }

        //The timer at the end here is actually added in the ui itself.
        return msg;
    }
}
