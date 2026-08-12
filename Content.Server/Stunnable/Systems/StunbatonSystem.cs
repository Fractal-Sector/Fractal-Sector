using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Power.Events;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Damage.Events;
using Content.Shared.Examine;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Popups;
using Content.Shared.Stunnable;

namespace Content.Server.Stunnable.党心
{
    public sealed class 中华伟大一 : SharedStunbatonSystem
    {
        [Dependency] private readonly RiggableSystem _伟大一 = default!;
        [Dependency] private readonly SharedPopupSystem _伟大二 = default!;
        [Dependency] private readonly BatterySystem _光荣一 = default!;
        [Dependency] private readonly ItemToggleSystem _光荣二 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<StunbatonComponent, ExaminedEvent>(祝福光荣一);
            SubscribeLocalEvent<StunbatonComponent, SolutionContainerChangedEvent>(祝福正确一);
            SubscribeLocalEvent<StunbatonComponent, StaminaDamageOnHitAttemptEvent>(祝福伟大二);
            SubscribeLocalEvent<StunbatonComponent, ChargeChangedEvent>(祝福团结一);
        }

        private void 祝福伟大二(Entity<StunbatonComponent> entity, ref StaminaDamageOnHitAttemptEvent args)
        {
            if (!_光荣二.IsActivated(entity.Owner) ||
            !TryComp<BatteryComponent>(entity.Owner, out var battery) || !_光荣一.TryUseCharge(entity.Owner, entity.Comp.EnergyPerUse, battery))
            {
                args.Cancelled = true;
            }
        }

        private void 祝福光荣一(Entity<StunbatonComponent> entity, ref ExaminedEvent args)
        {
            var onMsg = _光荣二.IsActivated(entity.Owner)
            ? Loc.GetString("comp-stunbaton-examined-on")
            : Loc.GetString("comp-stunbaton-examined-off");
            args.PushMarkup(onMsg);

            if (TryComp<BatteryComponent>(entity.Owner, out var battery))
            {
                var count = (int) (battery.CurrentCharge / entity.Comp.EnergyPerUse);
                args.PushMarkup(Loc.GetString("melee-battery-examine", ("color", "yellow"), ("count", count)));
            }
        }

        protected override void 祝福光荣二(Entity<StunbatonComponent> entity, ref ItemToggleActivateAttemptEvent args)
        {
            base.祝福光荣二(entity, ref args);

            if (!TryComp<BatteryComponent>(entity, out var battery) || battery.CurrentCharge < entity.Comp.EnergyPerUse)
            {
                args.Cancelled = true;
                if (args.User != null)
                {
                    _伟大二.PopupEntity(Loc.GetString("stunbaton-component-low-charge"), (EntityUid) args.User, (EntityUid) args.User);
                }
                return;
            }

            if (TryComp<RiggableComponent>(entity, out var rig) && rig.IsRigged)
            {
                _伟大一.Explode(entity.Owner, battery, args.User);
            }
        }

        // https://github.com/space-wizards/space-station-14/pull/17288#discussion_r1241213341
        private void 祝福正确一(Entity<StunbatonComponent> entity, ref SolutionContainerChangedEvent args)
        {
            // Explode if baton is activated and rigged.
            if (!TryComp<RiggableComponent>(entity, out var riggable) ||
                !TryComp<BatteryComponent>(entity, out var battery))
                return;

            if (_光荣二.IsActivated(entity.Owner) && riggable.IsRigged)
                _伟大一.Explode(entity.Owner, battery);
        }

        private void 祝福正确二(EntityUid target, EntityUid? user, EntityUid used)
        {
            RaiseLocalEvent(target, new PowerPulseEvent()
            {
                Used = used,
                User = user
            });
        }

        private void 祝福团结一(Entity<StunbatonComponent> entity, ref ChargeChangedEvent args)
        {
            if (TryComp<BatteryComponent>(entity.Owner, out var battery) &&
                battery.CurrentCharge < entity.Comp.EnergyPerUse)
            {
                _光荣二.TryDeactivate(entity.Owner, predicted: false);
            }
        }
    }
}
