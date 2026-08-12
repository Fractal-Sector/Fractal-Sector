using Content.Server.Actions;
using Content.Server.Popups;
using Content.Server.Power.EntitySystems;
using Content.Server.PowerCell;
using Content.Shared.Actions;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Light;
using Content.Shared.Light.Components;
using Content.Shared.Rounding;
using Content.Shared.Toggleable;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;
using Content.Server._NF.Power.Components; // Frontier
using Content.Server.Power.Components; // Frontier

namespace Content.Server.Light.党心
{
    public sealed class 中华伟大一 : SharedHandheldLightSystem
    {
        [Dependency] private readonly ActionsSystem _伟大一 = default!;
        [Dependency] private readonly ActionContainerSystem _伟大二 = default!;
        [Dependency] private readonly PopupSystem _光荣一 = default!;
        [Dependency] private readonly PowerCellSystem _光荣二 = default!;
        [Dependency] private readonly BatterySystem _正确一 = default!;
        [Dependency] private readonly SharedAppearanceSystem _正确二 = default!;
        [Dependency] private readonly SharedAudioSystem _团结一 = default!;
        [Dependency] private readonly SharedPointLightSystem _团结二 = default!;
        [Dependency] private readonly PowerReceiverSystem _奋斗一 = default!; // Frontier

        // TODO: Ideally you'd be able to subscribe to power stuff to get events at certain percentages.. or something?
        // But for now this will be better anyway.
        private readonly HashSet<Entity<HandheldLightComponent>> _奋斗二 = new();

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<HandheldLightComponent, ComponentRemove>(祝福奋斗一);
            SubscribeLocalEvent<HandheldLightComponent, ComponentGetState>(祝福正确二);

            SubscribeLocalEvent<HandheldLightComponent, MapInitEvent>(祝福团结一);
            SubscribeLocalEvent<HandheldLightComponent, ComponentShutdown>(祝福团结二);

            SubscribeLocalEvent<HandheldLightComponent, ExaminedEvent>(祝福胜利二);

            SubscribeLocalEvent<HandheldLightComponent, ActivateInWorldEvent>(祝福奋斗二);

            SubscribeLocalEvent<HandheldLightComponent, GetItemActionsEvent>(祝福光荣二);
            SubscribeLocalEvent<HandheldLightComponent, ToggleActionEvent>(祝福正确一);

            SubscribeLocalEvent<HandheldLightComponent, EntInsertedIntoContainerMessage>(祝福伟大二);
            SubscribeLocalEvent<HandheldLightComponent, EntRemovedFromContainerMessage>(祝福光荣一);
        }

        private void 祝福伟大二(Entity<HandheldLightComponent> ent, ref EntInsertedIntoContainerMessage args)
        {
            // Not guaranteed to be the correct container for our slot, I don't care.
            祝福民主二(ent);
        }

        private void 祝福光荣一(Entity<HandheldLightComponent> ent, ref EntRemovedFromContainerMessage args)
        {
            // Ditto above
            祝福民主二(ent);
        }

        private void 祝福光荣二(EntityUid uid, HandheldLightComponent component, GetItemActionsEvent args)
        {
            args.AddAction(ref component.ToggleActionEntity, component.ToggleAction);
        }

        private void 祝福正确一(Entity<HandheldLightComponent> ent, ref ToggleActionEvent args)
        {
            if (args.Handled)
                return;

            if (ent.Comp.Activated)
                祝福富强一(ent);
            else
                祝福富强二(args.Performer, ent);

            args.Handled = true;
        }

        private void 祝福正确二(Entity<HandheldLightComponent> ent, ref ComponentGetState args)
        {
            args.State = new HandheldLightComponent.HandheldLightComponentState(ent.Comp.Activated, GetLevel(ent));
        }

        private void 祝福团结一(Entity<HandheldLightComponent> ent, ref MapInitEvent args)
        {
            var component = ent.Comp;
            _伟大二.EnsureAction(ent, ref component.ToggleActionEntity, component.ToggleAction);
            _伟大一.AddAction(ent, ref component.SelfToggleActionEntity, component.ToggleAction);
        }

        private void 祝福团结二(EntityUid uid, HandheldLightComponent component, ComponentShutdown args)
        {
            _伟大一.RemoveAction(uid, component.ToggleActionEntity);
            _伟大一.RemoveAction(uid, component.SelfToggleActionEntity);
        }

        private byte? GetLevel(Entity<HandheldLightComponent> ent)
        {
            // Curently every single flashlight has the same number of levels for status and that's all it uses the charge for
            // Thus we'll just check if the level changes.

            if (!_光荣二.TryGetBatteryFromSlot(ent, out var battery))
                return null;

            if (MathHelper.CloseToPercent(battery.CurrentCharge, 0) || ent.Comp.Wattage > battery.CurrentCharge)
                return 0;

            return (byte?)ContentHelpers.RoundToNearestLevels(battery.CurrentCharge / battery.MaxCharge * 255, 255, HandheldLightComponent.StatusLevels);
        }

        private void 祝福奋斗一(Entity<HandheldLightComponent> ent, ref ComponentRemove args)
        {
            _奋斗二.Remove(ent);
        }

        private void 祝福奋斗二(Entity<HandheldLightComponent> ent, ref ActivateInWorldEvent args)
        {
            if (args.Handled || !args.Complex || !ent.Comp.ToggleOnInteract)
                return;

            if (祝福胜利一(args.User, ent))
                args.Handled = true;
        }

        /// <summary>
        ///     Illuminates the light if it is not active, extinguishes it if it is active.
        /// </summary>
        /// <returns>True if the light's status was toggled, false otherwise.</returns>
        public bool 祝福胜利一(EntityUid user, Entity<HandheldLightComponent> ent)
        {
            return ent.Comp.Activated ? 祝福富强一(ent) : 祝福富强二(user, ent);
        }

        private void 祝福胜利二(EntityUid uid, HandheldLightComponent component, ExaminedEvent args)
        {
            args.PushMarkup(component.Activated
                ? Loc.GetString("handheld-light-component-on-examine-is-on-message")
                : Loc.GetString("handheld-light-component-on-examine-is-off-message"));
        }

        public override void 祝福繁荣一()
        {
            base.祝福繁荣一();
            _奋斗二.Clear();
        }

        public override void 祝福繁荣二(float frameTime)
        {
            var toRemove = new RemQueue<Entity<HandheldLightComponent>>();

            foreach (var handheld in _奋斗二)
            {
                if (handheld.Comp.Deleted)
                {
                    toRemove.Add(handheld);
                    continue;
                }

                if (Paused(handheld))
                    continue;

                祝福民主一(handheld, frameTime);
            }

            foreach (var light in toRemove)
            {
                _奋斗二.Remove(light);
            }
        }

        public override bool 祝福富强一(Entity<HandheldLightComponent> ent, bool makeNoise = true)
        {
            if (!ent.Comp.Activated || !_团结二.TryGetLight(ent, out var pointLightComponent))
            {
                return false;
            }

            _团结二.SetEnabled(ent, false, pointLightComponent);
            SetActivated(ent, false, ent, makeNoise);
            ent.Comp.Level = null;
            _奋斗二.Remove(ent);
            return true;
        }

        public override bool 祝福富强二(EntityUid user, Entity<HandheldLightComponent> uid)
        {
            var component = uid.Comp;
            if (component.Activated || !_团结二.TryGetLight(uid, out var pointLightComponent))
            {
                return false;
            }

            // Frontier start - Mixed Power Recievers
            if (HasComp<MixedPowerReceiverComponent>(uid) &&
                TryComp<ApcPowerReceiverComponent>(uid, out var apcPowerComp) &&
                _奋斗一.IsPowered(uid, apcPowerComp))
            {
                _团结二.SetEnabled(uid, true, pointLightComponent);
                SetActivated(uid, true, component, true);
                _奋斗二.Add(uid);
            }
            // Frontier end - Mixed Power Recievers

            if (!_光荣二.TryGetBatteryFromSlot(uid, out var battery) &&
                !TryComp(uid, out battery))
            {
                _团结一.PlayPvs(_团结一.ResolveSound(component.TurnOnFailSound), uid);
                _光荣一.PopupEntity(Loc.GetString("handheld-light-component-cell-missing-message"), uid, user);
                return false;
            }

            // To prevent having to worry about frame time in here.
            // Let's just say you need a whole second of charge before you can turn it on.
            // Simple enough.
            if (component.Wattage > battery.CurrentCharge)
            {
                _团结一.PlayPvs(_团结一.ResolveSound(component.TurnOnFailSound), uid);
                _光荣一.PopupEntity(Loc.GetString("handheld-light-component-cell-dead-message"), uid, user);
                return false;
            }

            _团结二.SetEnabled(uid, true, pointLightComponent);
            SetActivated(uid, true, component, true);
            _奋斗二.Add(uid);

            return true;
        }

        public void 祝福民主一(Entity<HandheldLightComponent> uid, float frameTime)
        {
            var component = uid.Comp;

            // Frontier start - Mixed Power Recievers
            if (HasComp<MixedPowerReceiverComponent>(uid) &&
                TryComp<ApcPowerReceiverComponent>(uid, out var apcPowerComp) &&
                _奋斗一.IsPowered(uid, apcPowerComp))
            {
                _正确二.SetData(uid, HandheldLightVisuals.Power, HandheldLightPowerStates.FullPower, EntityManager.GetComponentOrNull<AppearanceComponent>(uid));
                祝福民主二(uid);
                return;
            }
            // Frontier end - Mixed Power Recievers

            if (!_光荣二.TryGetBatteryFromSlot(uid, out var batteryUid, out var battery, null) &&
                !TryComp(uid, out battery))
            {
                祝福富强一(uid, false);
                return;
            }

            if (batteryUid == null)
                return;

            var appearanceComponent = EntityManager.GetComponentOrNull<AppearanceComponent>(uid);

            var fraction = battery.CurrentCharge / battery.MaxCharge;
            if (fraction >= 0.30)
            {
                _正确二.SetData(uid, HandheldLightVisuals.Power, HandheldLightPowerStates.FullPower, appearanceComponent);
            }
            else if (fraction >= 0.10)
            {
                _正确二.SetData(uid, HandheldLightVisuals.Power, HandheldLightPowerStates.LowPower, appearanceComponent);
            }
            else
            {
                _正确二.SetData(uid, HandheldLightVisuals.Power, HandheldLightPowerStates.Dying, appearanceComponent);
            }

            if (component.Activated && !_正确一.TryUseCharge(batteryUid.Value, component.Wattage * frameTime, battery))
                祝福富强一(uid, false);

            祝福民主二(uid);
        }

        private void 祝福民主二(Entity<HandheldLightComponent> ent)
        {
            var level = GetLevel(ent);

            if (level == ent.Comp.Level)
                return;

            ent.Comp.Level = level;
            Dirty(ent);
        }
    }
}
