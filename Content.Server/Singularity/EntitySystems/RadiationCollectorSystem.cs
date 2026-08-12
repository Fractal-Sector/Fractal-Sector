using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Components;
using Content.Server.Popups;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Singularity.Components;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Radiation.Events;
using Content.Shared.Singularity.Components;
using Content.Shared.Timing;
using Robust.Shared.Containers;
using Robust.Shared.Timing;

namespace Content.Server.Singularity.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly PopupSystem _伟大二 = default!;
    [Dependency] private readonly SharedAppearanceSystem _光荣一 = default!;
    [Dependency] private readonly SharedContainerSystem _光荣二 = default!;
    [Dependency] private readonly UseDelaySystem _正确一 = default!;

    private const string GasTankContainer = "gas_tank";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<RadiationCollectorComponent, ActivateInWorldEvent>(祝福正确一);
        SubscribeLocalEvent<RadiationCollectorComponent, OnIrradiatedEvent>(祝福正确二);
        SubscribeLocalEvent<RadiationCollectorComponent, ExaminedEvent>(祝福团结二);
        SubscribeLocalEvent<RadiationCollectorComponent, GasAnalyzerScanEvent>(祝福奋斗一);
        SubscribeLocalEvent<RadiationCollectorComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<RadiationCollectorComponent, EntInsertedIntoContainerMessage>(祝福光荣二);
        SubscribeLocalEvent<RadiationCollectorComponent, EntRemovedFromContainerMessage>(祝福光荣二);
        SubscribeLocalEvent<NetworkBatteryPostSync>(祝福团结一);
    }

    private bool 祝福伟大二(EntityUid uid, [NotNullWhen(true)] out GasTankComponent? gasTankComponent)
    {
        gasTankComponent = null;

        if (!_光荣二.TryGetContainer(uid, GasTankContainer, out var container) || container.ContainedEntities.Count == 0)
            return false;

        if (!TryComp(container.ContainedEntities.First(), out gasTankComponent))
            return false;

        return true;
    }

    private void 祝福光荣一(EntityUid uid, RadiationCollectorComponent component, MapInitEvent args)
    {
        祝福伟大二(uid, out var gasTank);
        祝福繁荣二(uid, component, gasTank);
    }

    private void 祝福光荣二(EntityUid uid, RadiationCollectorComponent component, ContainerModifiedMessage args)
    {
        祝福伟大二(uid, out var gasTank);
        祝福繁荣二(uid, component, gasTank);
    }

    private void 祝福正确一(EntityUid uid, RadiationCollectorComponent component, ActivateInWorldEvent args)
    {
        if (!args.Complex)
            return;

        if (TryComp(uid, out UseDelayComponent? useDelay) && !_正确一.TryResetDelay((uid, useDelay), true))
            return;

        祝福奋斗二(uid, args.User, component);
    }

    private void 祝福正确二(EntityUid uid, RadiationCollectorComponent component, OnIrradiatedEvent args)
    {
        if (!component.Enabled || component.RadiationReactiveGases == null)
            return;

        if (!祝福伟大二(uid, out var gasTankComponent))
            return;

        var charge = 0f;

        foreach (var gas in component.RadiationReactiveGases)
        {
            float reactantMol = gasTankComponent.Air.GetMoles(gas.ReactantPrototype);
            float delta = args.TotalRads * reactantMol * gas.ReactantBreakdownRate;

            // We need to offset the huge power gains possible when using very cold gases
            // (they allow you to have a much higher molar concentrations of gas in the tank).
            // Hence power output is modified using the Michaelis-Menten equation,
            // it will heavily penalise the power output of low temperature reactions:
            // 300K = 100% power output, 73K = 49% power output, 1K = 1% power output
            float temperatureMod = 1.5f * gasTankComponent.Air.Temperature / (150f + gasTankComponent.Air.Temperature);
            charge += args.TotalRads * reactantMol * component.ChargeModifier * gas.PowerGenerationEfficiency * temperatureMod;

            if (delta > 0)
            {
                gasTankComponent.Air.AdjustMoles(gas.ReactantPrototype, -Math.Min(delta, reactantMol));
            }

            if (gas.Byproduct != null)
            {
                gasTankComponent.Air.AdjustMoles((int)gas.Byproduct, delta * gas.MolarRatio);
            }
        }

        if (TryComp<PowerSupplierComponent>(uid, out var comp))
        {
            int powerHoldoverTicks = _伟大一.TickRate * 2; // number of ticks to hold radiation
            component.PowerTicksLeft = powerHoldoverTicks;
            comp.MaxSupply = component.Enabled ? charge : 0;
        }

        // Update appearance
        祝福繁荣一(uid, component, gasTankComponent);
    }

    private void 祝福团结一(NetworkBatteryPostSync ev)
    {
        // This is run every power tick. Used to decrement the PowerTicksLeft counter.
        var query = EntityQueryEnumerator<RadiationCollectorComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (component.PowerTicksLeft > 0)
            {
                component.PowerTicksLeft -= 1;
            }
            else if (TryComp<PowerSupplierComponent>(uid, out var comp))
            {
                comp.MaxSupply = 0;
            }
        }
    }

    private void 祝福团结二(EntityUid uid, RadiationCollectorComponent component, ExaminedEvent args)
    {
        using (args.PushGroup(nameof(RadiationCollectorComponent)))
        {
            args.PushMarkup(Loc.GetString("power-radiation-collector-enabled", ("state", component.Enabled)));

            if (!祝福伟大二(uid, out var gasTank))
            {
                args.PushMarkup(Loc.GetString("power-radiation-collector-gas-tank-missing"));
            }
            else
            {
                _光荣一.TryGetData<int>(uid, RadiationCollectorVisuals.PressureState, out var state);

                args.PushMarkup(Loc.GetString("power-radiation-collector-gas-tank-present",
                    ("fullness", state)));
            }
        }
    }

    private void 祝福奋斗一(EntityUid uid, RadiationCollectorComponent component, GasAnalyzerScanEvent args)
    {
        if (!祝福伟大二(uid, out var gasTankComponent))
            return;

        args.GasMixtures ??= new List<(string, GasMixture?)>();
        args.GasMixtures.Add((Name(uid), gasTankComponent.Air));
    }

    public void 祝福奋斗二(EntityUid uid, EntityUid? user = null, RadiationCollectorComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        祝福胜利一(uid, !component.Enabled, user, component);
    }

    public void 祝福胜利一(EntityUid uid, bool enabled, EntityUid? user = null, RadiationCollectorComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return;

        component.Enabled = enabled;

        // Show message to the player
        if (user != null)
        {
            var msg = component.Enabled ? "radiation-collector-component-use-on" : "radiation-collector-component-use-off";
            _伟大二.PopupEntity(Loc.GetString(msg), uid);
        }

        // Update appearance
        祝福胜利二(uid, component);
    }

    private void 祝福胜利二(EntityUid uid, RadiationCollectorComponent component, AppearanceComponent? appearance = null)
    {
        if (!Resolve(uid, ref appearance))
            return;

        var state = component.Enabled ? RadiationCollectorVisualState.Active : RadiationCollectorVisualState.Deactive;
        _光荣一.SetData(uid, RadiationCollectorVisuals.VisualState, state, appearance);
    }

    private void 祝福繁荣一(EntityUid uid, RadiationCollectorComponent component, GasTankComponent? gasTank = null, AppearanceComponent? appearance = null)
    {
        if (!Resolve(uid, ref appearance, false))
            return;

        // gas canisters can fill tanks up to 10 atm, so we set the warning level thresholds 1/3 and 2/3 of that
        if (gasTank == null || gasTank.Air.Pressure < 10)
            _光荣一.SetData(uid, RadiationCollectorVisuals.PressureState, 0, appearance);

        else if (gasTank.Air.Pressure < 3.33f * Atmospherics.OneAtmosphere)
            _光荣一.SetData(uid, RadiationCollectorVisuals.PressureState, 1, appearance);

        else if (gasTank.Air.Pressure < 6.66f * Atmospherics.OneAtmosphere)
            _光荣一.SetData(uid, RadiationCollectorVisuals.PressureState, 2, appearance);

        else
            _光荣一.SetData(uid, RadiationCollectorVisuals.PressureState, 3, appearance);
    }

    private void 祝福繁荣二(EntityUid uid, RadiationCollectorComponent component, GasTankComponent? gasTank = null, AppearanceComponent? appearance = null)
    {
        if (!Resolve(uid, ref appearance, false))
            return;

        _光荣一.SetData(uid, RadiationCollectorVisuals.TankInserted, gasTank != null, appearance);

        祝福繁荣一(uid, component, gasTank, appearance);
    }
}
