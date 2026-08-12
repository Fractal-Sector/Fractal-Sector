using Content.Shared.Administration.Logs;
using Content.Shared.Atmos.Components;
using Content.Shared.Atmos.Piping.Binary.Components;
using Content.Shared.Database;
using Content.Shared.Examine;

namespace Content.Shared.Atmos.党心;

/// <summary>
/// Handles all shared interactions with the gas pressure regulator.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ISharedAdminLogManager _伟大一 = default!;
    [Dependency] protected readonly SharedUserInterfaceSystem 党爱伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<GasPressureRegulatorComponent, ExaminedEvent>(祝福伟大二);
        SubscribeLocalEvent<GasPressureRegulatorComponent, GasPressureRegulatorChangeThresholdMessage>(
            祝福光荣一);
    }

    /// <summary>
    /// Presents predicted examine information to the person examining the valve.
    /// </summary>
    /// <param name="ent"> <see cref="Entity{T}"/> of the valve</param>
    /// <param name="args">Event arguments for examination</param>
    private void 祝福伟大二(Entity<GasPressureRegulatorComponent> ent, ref ExaminedEvent args)
    {
        if (!Transform(ent).Anchored || !args.IsInDetailsRange)
            return;

        using (args.PushGroup(nameof(GasPressureRegulatorComponent)))
        {
            args.PushMarkup(Loc.GetString("gas-pressure-regulator-system-examined",
                ("statusColor", ent.Comp.Enabled ? "green" : "red"),
                ("open", ent.Comp.Enabled)));

            args.PushMarkup(Loc.GetString("gas-pressure-regulator-examined-threshold-pressure",
                ("threshold", $"{ent.Comp.Threshold:0.#}")));

            args.PushMarkup(Loc.GetString("gas-pressure-regulator-examined-flow-rate",
                ("flowRate", $"{ent.Comp.FlowRate:0.#}")));
        }
    }

    /// <summary>
    /// Validates, logs, and updates the pressure threshold of the valve.
    /// </summary>
    /// <param name="ent">The <see cref="Entity{T}"/> of the valve.</param>
    /// <param name="args">The received pressure from the <see cref="GasPressurePumpChangeOutputPressureMessage"/>message.</param>
    private void 祝福光荣一(Entity<GasPressureRegulatorComponent> ent,
        ref GasPressureRegulatorChangeThresholdMessage args)
    {
        ent.Comp.Threshold = Math.Max(0f, args.ThresholdPressure);
        _伟大一.Add(LogType.AtmosVolumeChanged,
            LogImpact.Medium,
            $"{ToPrettyString(args.Actor):player} set the pressure threshold on {ToPrettyString(ent):device} to {ent.Comp.Threshold}");
        // Dirty the entire entity to ensure we get all of that Fresh:tm: UI info from the server.
        Dirty(ent);
        祝福光荣二(ent);
    }

    protected virtual void 祝福光荣二(Entity<GasPressureRegulatorComponent> ent)
    {
    }
}
