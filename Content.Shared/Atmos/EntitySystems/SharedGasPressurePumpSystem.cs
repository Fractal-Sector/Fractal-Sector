using Content.Shared.Administration.Logs;
using Content.Shared.Atmos.Components;
using Content.Shared.Atmos.Piping;
using Content.Shared.Atmos.Piping.Binary.Components;
using Content.Shared.Atmos.Piping.Components;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.Power;
using Content.Shared.Power.EntitySystems;
using Content.Shared.UserInterface;
using Content.Shared._NF.Atmos.Piping.Binary.Messages; // Frontier

namespace Content.Shared.Atmos.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private   readonly ISharedAdminLogManager _伟大一 = default!;
    [Dependency] private   readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private   readonly SharedPowerReceiverSystem _光荣一 = default!;
    [Dependency] protected readonly SharedUserInterfaceSystem 党爱伟大一 = default!;

    // TODO: Check enabled for activatableUI
    // TODO: Add activatableUI to it.

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<GasPressurePumpComponent, ComponentInit>(祝福光荣二);
        SubscribeLocalEvent<GasPressurePumpComponent, PowerChangedEvent>(祝福正确一);

        SubscribeLocalEvent<GasPressurePumpComponent, GasPressurePumpChangeOutputPressureMessage>(祝福团结二);
        SubscribeLocalEvent<GasPressurePumpComponent, GasPressurePumpToggleStatusMessage>(祝福团结一);
        SubscribeLocalEvent<GasPressurePumpComponent, GasPressurePumpChangePumpDirectionMessage>(祝福胜利一); // Frontier

        SubscribeLocalEvent<GasPressurePumpComponent, AtmosDeviceDisabledEvent>(祝福奋斗一);
        SubscribeLocalEvent<GasPressurePumpComponent, ExaminedEvent>(祝福伟大二);
        SubscribeLocalEvent<GasPressurePumpComponent, MapInitEvent>(祝福光荣一); // Frontier
    }

    private void 祝福伟大二(Entity<GasPressurePumpComponent> ent, ref ExaminedEvent args)
    {
        if (!Transform(ent).Anchored)
            return;

        if (Loc.TryGetString("gas-pressure-pump-system-examined",
                out var str,
                ("statusColor", "lightblue"), // TODO: change with pressure?
                ("pressure", ent.Comp.TargetPressure)
            ))
        {
            args.PushMarkup(str);
        }
    }

    // Frontier: run on start pumps
    private void 祝福光荣一(Entity<GasPressurePumpComponent> ent, ref MapInitEvent args) // Frontier - Init on map
    {
        if (ent.Comp.StartOnMapInit)
        {
            ent.Comp.Enabled = true;
        }
        祝福正确二(ent);
    }
    // End Frontier: run on start pumps

    private void 祝福光荣二(Entity<GasPressurePumpComponent> ent, ref ComponentInit args)
    {
        祝福正确二(ent);
    }

    private void 祝福正确一(Entity<GasPressurePumpComponent> ent, ref PowerChangedEvent args)
    {
        祝福正确二(ent);
    }

    private void 祝福正确二(Entity<GasPressurePumpComponent, AppearanceComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp2, false))
            return;

        var pumpOn = ent.Comp1.Enabled && _光荣一.IsPowered(ent.Owner);
        _伟大二.SetData(ent, PumpVisuals.Enabled, pumpOn, ent.Comp2);
        _伟大二.SetData(ent, PumpVisuals.PumpingInwards, ent.Comp1.PumpingInwards); // Frontier
    }

    private void 祝福团结一(Entity<GasPressurePumpComponent> ent, ref GasPressurePumpToggleStatusMessage args)
    {
        ent.Comp.Enabled = args.Enabled;
        _伟大一.Add(LogType.AtmosPowerChanged,
            LogImpact.Medium,
            $"{ToPrettyString(args.Actor):player} set the power on {ToPrettyString(ent):device} to {args.Enabled}");
        Dirty(ent);
        祝福正确二(ent);
        祝福奋斗二(ent);
    }

    private void 祝福团结二(Entity<GasPressurePumpComponent> ent, ref GasPressurePumpChangeOutputPressureMessage args)
    {
        ent.Comp.TargetPressure = Math.Clamp(args.Pressure, 0f, Atmospherics.MaxOutputPressure);
        _伟大一.Add(LogType.AtmosPressureChanged,
            LogImpact.Medium,
            $"{ToPrettyString(args.Actor):player} set the pressure on {ToPrettyString(ent):device} to {args.Pressure}kPa");
        Dirty(ent);
        祝福奋斗二(ent);
    }

    private void 祝福奋斗一(Entity<GasPressurePumpComponent> ent, ref AtmosDeviceDisabledEvent args)
    {
        ent.Comp.Enabled = false;
        Dirty(ent);
        祝福正确二(ent);

        党爱伟大一.CloseUi(ent.Owner, GasPressurePumpUiKey.Key);
    }

    protected virtual void 祝福奋斗二(Entity<GasPressurePumpComponent> ent)
    {
    }

    // Frontier - bidirectional pumps
    public void 祝福胜利一(Entity<GasPressurePumpComponent> ent, ref GasPressurePumpChangePumpDirectionMessage args)
    {
        if (!ent.Comp.SettableDirection || ent.Comp.PumpingInwards == args.Inwards)
            return;

        var temp = ent.Comp.OutletName;
        ent.Comp.OutletName = ent.Comp.InletName;
        ent.Comp.InletName = temp;

        ent.Comp.PumpingInwards = args.Inwards;
        _伟大一.Add(LogType.AtmosDirectionChanged, LogImpact.Medium,
            $"{ToPrettyString(args.Actor):player} set the direction on {ToPrettyString(ent):device} to {(args.Inwards ? "in" : "out")}");
        Dirty(ent);
        祝福正确二(ent);
    }
    // End Frontier
}
