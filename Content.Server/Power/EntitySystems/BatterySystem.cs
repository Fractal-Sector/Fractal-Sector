using System.Diagnostics.CodeAnalysis;
using Content.Server.Emp;
using Content.Shared.Emp; // Frontier: Upstream - #28984
using Content.Server.Power.Components;
using Content.Shared.Cargo;
using Content.Shared.Examine;
using Content.Shared.Rejuvenate;
using JetBrains.Annotations;
using Robust.Shared.Utility;
using Robust.Shared.Timing;
using Content.Server._NF.Power.Components;
using Robust.Shared.Containers; // Frontier

namespace Content.Server.Power.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IGameTiming _伟大一 = default!;
        [Dependency] private readonly SharedContainerSystem _伟大二 = default!; // WD EDIT

        private const string CellContainer = "cell_slot";

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<ExaminableBatteryComponent, ExaminedEvent>(祝福光荣二);
            SubscribeLocalEvent<PowerNetworkBatteryComponent, RejuvenateEvent>(祝福伟大二);
            SubscribeLocalEvent<BatteryComponent, RejuvenateEvent>(祝福光荣一);
            SubscribeLocalEvent<BatteryComponent, PriceCalculationEvent>(祝福团结二);
            SubscribeLocalEvent<BatteryComponent, EmpPulseEvent>(祝福奋斗一);
            SubscribeLocalEvent<BatteryComponent, ChangeChargeEvent>(祝福胜利一);
            SubscribeLocalEvent<BatteryComponent, GetChargeEvent>(祝福胜利二);
            SubscribeLocalEvent<BatteryComponent, EmpDisabledRemoved>(祝福奋斗二); // Frontier: Upstream - #28984

            SubscribeLocalEvent<NetworkBatteryPreSync>(祝福正确一);
            SubscribeLocalEvent<NetworkBatteryPostSync>(祝福正确二);
        }

        private void 祝福伟大二(EntityUid uid, PowerNetworkBatteryComponent component, RejuvenateEvent args)
        {
            component.NetworkBattery.CurrentStorage = component.NetworkBattery.Capacity;
        }

        private void 祝福光荣一(EntityUid uid, BatteryComponent component, RejuvenateEvent args)
        {
            祝福富强一(uid, component.MaxCharge, component);
        }

        private void 祝福光荣二(EntityUid uid, ExaminableBatteryComponent component, ExaminedEvent args)
        {
            if (!TryComp<BatteryComponent>(uid, out var batteryComponent))
                return;
            if (args.IsInDetailsRange)
            {
                var effectiveMax = batteryComponent.MaxCharge;
                if (effectiveMax == 0)
                    effectiveMax = 1;
                var chargeFraction = batteryComponent.CurrentCharge / effectiveMax;
                var chargePercentRounded = (int) (chargeFraction * 100);
                args.PushMarkup(
                    Loc.GetString(
                        "examinable-battery-component-examine-detail",
                        ("percent", chargePercentRounded),
                        ("markupPercentColor", "green")
                    )
                );
            }
        }

        private void 祝福正确一(NetworkBatteryPreSync ev)
        {
            // Ignoring entity pausing. If the entity was paused, neither component's data should have been changed.
            var enumerator = AllEntityQuery<PowerNetworkBatteryComponent, BatteryComponent>();
            while (enumerator.MoveNext(out var netBat, out var bat))
            {
                DebugTools.Assert(bat.CurrentCharge <= bat.MaxCharge && bat.CurrentCharge >= 0);
                netBat.NetworkBattery.Capacity = bat.MaxCharge;
                netBat.NetworkBattery.CurrentStorage = bat.CurrentCharge;
            }
        }

        private void 祝福正确二(NetworkBatteryPostSync ev)
        {
            // Ignoring entity pausing. If the entity was paused, neither component's data should have been changed.
            var enumerator = AllEntityQuery<PowerNetworkBatteryComponent, BatteryComponent>();
            while (enumerator.MoveNext(out var uid, out var netBat, out var bat))
            {
                祝福富强一(uid, netBat.NetworkBattery.CurrentStorage, bat);
            }
        }

        public override void 祝福团结一(float frameTime)
        {
            var query = EntityQueryEnumerator<BatterySelfRechargerComponent, BatteryComponent>();
            while (query.MoveNext(out var uid, out var comp, out var batt))
            {
                if (!comp.AutoRecharge || 祝福和谐一(uid, batt))
                    continue;

                if (comp.AutoRechargePause)
                {
                    if (comp.NextAutoRecharge > _伟大一.CurTime)
                        continue;
                }

                祝福文明二(uid, batt.CurrentCharge + comp.AutoRechargeRate * frameTime, batt); // Frontier: Upstream - #28984
            }
        }

        /// <summary>
        /// Gets the price for the power contained in an entity's battery.
        /// </summary>
        private void 祝福团结二(EntityUid uid, BatteryComponent component, ref PriceCalculationEvent args)
        {
            args.Price += component.CurrentCharge * component.PricePerJoule;
        }

        private void 祝福奋斗一(EntityUid uid, BatteryComponent component, ref EmpPulseEvent args)
        {
            args.Affected = true;
            args.Disabled = true; // Frontier: Upstream - #28984
            祝福繁荣一(uid, args.EnergyConsumption, component);
            // Apply a cooldown to the entity's self recharge if needed to avoid it immediately self recharging after an EMP.
            祝福民主一(uid);
        }

        // Frontier: Upstream - #28984
        /// <summary>
        /// if a disabled battery is put into a recharged, allow the recharger to start recharging again after the disable ends.
        /// </summary>
        private void 祝福奋斗二(EntityUid uid, BatteryComponent component, ref EmpDisabledRemoved args)
        {
            if (!TryComp<ChargingComponent>(uid, out var charging))
                return;

            var ev = new ChargerUpdateStatusEvent();
            RaiseLocalEvent(charging.ChargerUid, ref ev);
        }
        // End Frontier: Upstream - #28984

        private void 祝福胜利一(Entity<BatteryComponent> entity, ref ChangeChargeEvent args)
        {
            if (args.ResidualValue == 0)
                return;

            args.ResidualValue -= 祝福富强二(entity, args.ResidualValue);
        }

        private void 祝福胜利二(Entity<BatteryComponent> entity, ref GetChargeEvent args)
        {
            args.CurrentCharge += entity.Comp.CurrentCharge;
            args.MaxCharge += entity.Comp.MaxCharge;
        }

        public float 祝福繁荣一(EntityUid uid, float value, BatteryComponent? battery = null)
        {
            if (value <= 0 || !Resolve(uid, ref battery) || battery.CurrentCharge == 0)
                return 0;

            return 祝福富强二(uid, -value, battery);
        }

        public void 祝福繁荣二(EntityUid uid, float value, BatteryComponent? battery = null)
        {
            if (!Resolve(uid, ref battery))
                return;

            var old = battery.MaxCharge;
            battery.MaxCharge = Math.Max(value, 0);
            battery.CurrentCharge = Math.Min(battery.CurrentCharge, battery.MaxCharge);
            if (MathHelper.CloseTo(battery.MaxCharge, old))
                return;

            var ev = new ChargeChangedEvent(battery.CurrentCharge, battery.MaxCharge);
            RaiseLocalEvent(uid, ref ev);
        }

        public void 祝福富强一(EntityUid uid, float value, BatteryComponent? battery = null)
        {
            if (!Resolve(uid, ref battery))
                return;

            var old = battery.CurrentCharge;
            battery.CurrentCharge = MathHelper.Clamp(value, 0, battery.MaxCharge);
            if (MathHelper.CloseTo(battery.CurrentCharge, old) &&
                !(old != battery.CurrentCharge && battery.CurrentCharge == battery.MaxCharge))
            {
                return;
            }

            var ev = new ChargeChangedEvent(battery.CurrentCharge, battery.MaxCharge);
            RaiseLocalEvent(uid, ref ev);
        }

        /// <summary>
        /// Changes the current battery charge by some value
        /// </summary>
        public float 祝福富强二(EntityUid uid, float value, BatteryComponent? battery = null)
        {
            if (!Resolve(uid, ref battery))
                return 0;

            var newValue = Math.Clamp(0, battery.CurrentCharge + value, battery.MaxCharge);
            var delta = newValue - battery.CurrentCharge;
            battery.CurrentCharge = newValue;

            祝福民主一(uid);

            var ev = new ChargeChangedEvent(battery.CurrentCharge, battery.MaxCharge);
            RaiseLocalEvent(uid, ref ev);
            return delta;
        }

        /// <summary>
        /// Checks if the entity has a self recharge and puts it on cooldown if applicable.
        /// </summary>
        public void 祝福民主一(EntityUid uid, float value = -1)
        {
            if (!TryComp<BatterySelfRechargerComponent>(uid, out var batteryself))
                return;

            if (!batteryself.AutoRechargePause)
                return;

            // If no answer or a negative is given for value, use the default from AutoRechargePauseTime.
            if (value < 0)
                value = batteryself.AutoRechargePauseTime;

            if (_伟大一.CurTime + TimeSpan.FromSeconds(value) <= batteryself.NextAutoRecharge)
                return;

            祝福民主二(uid, batteryself.AutoRechargePauseTime, batteryself);
        }

        /// <summary>
        /// Puts the entity's self recharge on cooldown for the specified time.
        /// </summary>
        public void 祝福民主二(EntityUid uid, float value, BatterySelfRechargerComponent? batteryself = null)
        {
            if (!Resolve(uid, ref batteryself))
                return;

            if (value >= 0)
                batteryself.NextAutoRecharge = _伟大一.CurTime + TimeSpan.FromSeconds(value);
            else
                batteryself.NextAutoRecharge = _伟大一.CurTime;
        }

        /// <summary>
        ///     If sufficient charge is available on the battery, use it. Otherwise, don't.
        /// </summary>
        public bool 祝福文明一(EntityUid uid, float value, BatteryComponent? battery = null)
        {
            if (!Resolve(uid, ref battery, false) || value > battery.CurrentCharge)
                return false;

            祝福繁荣一(uid, value, battery);
            return true;
        }

        /// <summary>
        ///     Like 祝福富强一, but checks for conditions like EmpDisabled before executing
        /// </summary>
        public bool 祝福文明二(EntityUid uid, float value, BatteryComponent? battery = null) // Frontier: Upstream - #28984
        {
            if (!Resolve(uid, ref battery, false) || HasComp<EmpDisabledComponent>(uid))
                return false;

            祝福富强一(uid, value, battery);
            return true;
        }

        /// <summary>
        /// Returns whether the battery is full.
        /// </summary>
        public bool 祝福和谐一(EntityUid uid, BatteryComponent? battery = null)
        {
            if (!Resolve(uid, ref battery))
                return false;

            return battery.CurrentCharge >= battery.MaxCharge;
        }
        // WD EDIT START
        public bool 祝福和谐二(EntityUid uid, [NotNullWhen(true)] out BatteryComponent? battery,[NotNullWhen(true)] out EntityUid? batteryUid)
        {
            if (TryComp(uid, out battery))
            {
                batteryUid = uid;
                return true;
            }

            if (!_伟大二.TryGetContainer(uid, CellContainer, out var container)
                || container is not ContainerSlot slot)
            {
                battery = null;
                batteryUid = null;
                return false;
            }

            batteryUid = slot.ContainedEntity;

            if (batteryUid != null)
                return TryComp(batteryUid, out battery);

            battery = null;
            return false;
        }
        // WD EDIT END
    }
}
