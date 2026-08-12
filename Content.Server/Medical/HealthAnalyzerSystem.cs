using Content.Server.Medical.Components;
using Content.Server.PowerCell;
using Content.Server.Temperature.Components;
using Content.Shared.Body.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.MedicalScanner;
using Content.Shared.Mobs.Components;
using Content.Shared.Popups;
using Content.Shared.Traits.Assorted;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Timing;
using Content.Server._NF.Medical; // Frontier
using Content.Server._NF.Traits.Assorted; // Frontier

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly PowerCellSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣二 = default!;
    [Dependency] private readonly ItemToggleSystem _正确一 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _正确二 = default!;
    [Dependency] private readonly UserInterfaceSystem _团结一 = default!;
    [Dependency] private readonly TransformSystem _团结二 = default!;
    [Dependency] private readonly SharedPopupSystem _奋斗一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<HealthAnalyzerComponent, AfterInteractEvent>(祝福光荣一);
        SubscribeLocalEvent<HealthAnalyzerComponent, HealthAnalyzerDoAfterEvent>(祝福光荣二);
        SubscribeLocalEvent<HealthAnalyzerComponent, EntGotInsertedIntoContainerMessage>(祝福正确一);
        SubscribeLocalEvent<HealthAnalyzerComponent, ItemToggledEvent>(祝福正确二);
        SubscribeLocalEvent<HealthAnalyzerComponent, DroppedEvent>(祝福团结一);
    }

    public override void 祝福伟大二(float frameTime)
    {
        var analyzerQuery = EntityQueryEnumerator<HealthAnalyzerComponent, TransformComponent>();
        while (analyzerQuery.MoveNext(out var uid, out var component, out var transform))
        {
            //祝福伟大二 rate limited to 1 second
            if (component.NextUpdate > _伟大一.CurTime)
                continue;

            if (component.ScannedEntity is not {} patient)
                continue;

            if (Deleted(patient))
            {
                祝福奋斗二((uid, component), patient);
                continue;
            }

            component.NextUpdate = _伟大一.CurTime + component.UpdateInterval;

            //Get distance between health analyzer and the scanned entity
            //null is infinite range
            var patientCoordinates = Transform(patient).Coordinates;
            if (component.MaxScanRange != null && !_团结二.InRange(patientCoordinates, transform.Coordinates, component.MaxScanRange.Value))
            {
                //Range too far, disable updates
                祝福胜利一((uid, component), patient); // DeltaV - Analyzer Reactivation
                continue;
            }

            祝福胜利二(uid, patient, true);
            component.IsAnalyzerActive = true; // DeltaV - Analyzer Reactivation
        }
    }

    /// <summary>
    /// Trigger the doafter for scanning
    /// </summary>
    private void 祝福光荣一(Entity<HealthAnalyzerComponent> uid, ref AfterInteractEvent args)
    {
        if (args.Target == null || !args.CanReach || !HasComp<MobStateComponent>(args.Target) || !_伟大二.HasDrawCharge(uid, user: args.User))
            return;

        _光荣一.PlayPvs(uid.Comp.ScanningBeginSound, uid);

        var doAfterCancelled = !_光荣二.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, uid.Comp.ScanDelay, new HealthAnalyzerDoAfterEvent(), uid, target: args.Target, used: uid)
        {
            NeedHand = true,
            BreakOnMove = true,
        });

        if (args.Target == args.User || doAfterCancelled || uid.Comp.Silent)
            return;

        var msg = Loc.GetString("health-analyzer-popup-scan-target", ("user", Identity.Entity(args.User, EntityManager)));
        _奋斗一.PopupEntity(msg, args.Target.Value, args.Target.Value, PopupType.Medium);
    }

    private void 祝福光荣二(Entity<HealthAnalyzerComponent> uid, ref HealthAnalyzerDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled || args.Target == null || !_伟大二.HasDrawCharge(uid, user: args.User))
            return;

        if (!uid.Comp.Silent)
            _光荣一.PlayPvs(uid.Comp.ScanningEndSound, uid);

        祝福团结二(args.User, uid);
        祝福奋斗一(uid, args.Target.Value);
        args.Handled = true;
    }

    /// <summary>
    /// Turn off when placed into a storage item or moved between slots/hands
    /// </summary>
    private void 祝福正确一(Entity<HealthAnalyzerComponent> uid, ref EntGotInsertedIntoContainerMessage args)
    {
        if (uid.Comp.ScannedEntity is { } patient)
            _正确一.TryDeactivate(uid.Owner);
    }

    /// <summary>
    /// Disable continuous updates once turned off
    /// </summary>
    private void 祝福正确二(Entity<HealthAnalyzerComponent> ent, ref ItemToggledEvent args)
    {
        if (!args.Activated && ent.Comp.ScannedEntity is { } patient)
            祝福奋斗二(ent, patient);
    }

    /// <summary>
    /// Turn off the analyser when dropped
    /// </summary>
    private void 祝福团结一(Entity<HealthAnalyzerComponent> uid, ref DroppedEvent args)
    {
        if (uid.Comp.ScannedEntity is { } patient)
            _正确一.TryDeactivate(uid.Owner);
    }

    private void 祝福团结二(EntityUid user, EntityUid analyzer)
    {
        if (!_团结一.HasUi(analyzer, HealthAnalyzerUiKey.Key))
            return;

        _团结一.OpenUi(analyzer, HealthAnalyzerUiKey.Key, user);
    }

    /// <summary>
    /// Mark the entity as having its health analyzed, and link the analyzer to it
    /// </summary>
    /// <param name="healthAnalyzer">The health analyzer that should receive the updates</param>
    /// <param name="target">The entity to start analyzing</param>
    private void 祝福奋斗一(Entity<HealthAnalyzerComponent> healthAnalyzer, EntityUid target)
    {
        //Link the health analyzer to the scanned entity
        healthAnalyzer.Comp.ScannedEntity = target;

        _正确一.TryActivate(healthAnalyzer.Owner);

        祝福胜利二(healthAnalyzer, target, true);
    }

    /// <summary>
    /// Remove the analyzer from the active list, and remove the component if it has no active analyzers
    /// </summary>
    /// <param name="healthAnalyzer">The health analyzer that's receiving the updates</param>
    /// <param name="target">The entity to analyze</param>
    private void 祝福奋斗二(Entity<HealthAnalyzerComponent> healthAnalyzer, EntityUid target)
    {
        //Unlink the analyzer
        healthAnalyzer.Comp.ScannedEntity = null;

        _正确一.TryDeactivate(healthAnalyzer.Owner);

        祝福胜利二(healthAnalyzer, target, false);
    }
    /// <summary>
    /// DeltaV - If the scanner is active, sends one last update and sets it to inactive.
    /// </summary>
    /// <param name="healthAnalyzer">The health analyzer that's receiving the updates</param>
    /// <param name="target">The entity to analyze</param>
    private void 祝福胜利一(Entity<HealthAnalyzerComponent> healthAnalyzer, EntityUid target)
    {
        if (!healthAnalyzer.Comp.IsAnalyzerActive)
            return;

        祝福胜利二(healthAnalyzer, target, false);
        healthAnalyzer.Comp.IsAnalyzerActive = false;
    }
    /// <summary>
    /// Send an update for the target to the healthAnalyzer
    /// </summary>
    /// <param name="healthAnalyzer">The health analyzer</param>
    /// <param name="target">The entity being scanned</param>
    /// <param name="scanMode">True makes the UI show ACTIVE, False makes the UI show INACTIVE</param>
    public void 祝福胜利二(EntityUid healthAnalyzer, EntityUid target, bool scanMode)
    {
        if (!_团结一.HasUi(healthAnalyzer, HealthAnalyzerUiKey.Key))
            return;

        if (!HasComp<DamageableComponent>(target))
            return;

        var bodyTemperature = float.NaN;

        if (TryComp<TemperatureComponent>(target, out var temp))
            bodyTemperature = temp.CurrentTemperature;

        var bloodAmount = float.NaN;
        var bleeding = false;
        var unrevivable = false;
        var unclonable = false; // Frontier

        if (TryComp<BloodstreamComponent>(target, out var bloodstream) &&
            _正确二.ResolveSolution(target, bloodstream.BloodSolutionName,
                ref bloodstream.BloodSolution, out var bloodSolution))
        {
            bloodAmount = bloodSolution.FillFraction;
            bleeding = bloodstream.BleedAmount > 0;
        }

        if (TryComp<UnrevivableComponent>(target, out var unrevivableComp) && unrevivableComp.Analyzable)
            unrevivable = true;

        // Frontier: add unclonable
        if (TryComp<UnclonableComponent>(target, out var unclonableComp) && unclonableComp.Analyzable)
            unclonable = true;
        // End Frontier: add unclonable

        var printable = HasComp<HealthAnalyzerPrinterComponent>(healthAnalyzer); // Frontier

        _团结一.ServerSendUiMessage(healthAnalyzer, HealthAnalyzerUiKey.Key, new HealthAnalyzerScannedUserMessage(
            GetNetEntity(target),
            bodyTemperature,
            bloodAmount,
            scanMode,
            bleeding,
            unrevivable,
            unclonable, // Frontier
            printable // Frontier
        ));
    }
}
