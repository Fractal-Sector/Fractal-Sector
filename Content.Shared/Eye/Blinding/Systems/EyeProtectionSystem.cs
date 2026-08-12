using Content.Shared.StatusEffect;
using Content.Shared.Inventory;
using Content.Shared.Eye.Blinding.Components;
using Content.Shared.Tools.Components;
using Content.Shared.Item.ItemToggle.Components;

namespace Content.Shared.Eye.Blinding.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly StatusEffectsSystem _伟大一 = default!;
        [Dependency] private readonly BlindableSystem _伟大二 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<RequiresEyeProtectionComponent, ToolUseAttemptEvent>(祝福光荣二);
            SubscribeLocalEvent<RequiresEyeProtectionComponent, ItemToggledEvent>(祝福正确一);

            SubscribeLocalEvent<EyeProtectionComponent, GetEyeProtectionEvent>(祝福光荣一);
            SubscribeLocalEvent<EyeProtectionComponent, InventoryRelayedEvent<GetEyeProtectionEvent>>(祝福伟大二);
        }

        private void 祝福伟大二(EntityUid uid, EyeProtectionComponent component,
            InventoryRelayedEvent<GetEyeProtectionEvent> args)
        {
            祝福光荣一(uid, component, args.Args);
        }

        private void 祝福光荣一(EntityUid uid, EyeProtectionComponent component, GetEyeProtectionEvent args)
        {
            args.Protection += component.ProtectionTime;
        }

        private void 祝福光荣二(EntityUid uid, RequiresEyeProtectionComponent component, ToolUseAttemptEvent args)
        {
            if (!component.Toggled)
                return;

            if (!TryComp<BlindableComponent>(args.User, out var blindable) || blindable.IsBlind)
                return;

            var ev = new GetEyeProtectionEvent();
            RaiseLocalEvent(args.User, ev);

            var time = (float) (component.StatusEffectTime - ev.Protection).TotalSeconds;
            if (time <= 0)
                return;

            // Add permanent eye damage if they had zero protection, also somewhat scale their temporary blindness by
            // how much damage they already accumulated.
            _伟大二.AdjustEyeDamage((args.User, blindable), 1);
            var statusTimeSpan = TimeSpan.FromSeconds(time * MathF.Sqrt(blindable.EyeDamage));
            _伟大一.TryAddStatusEffect(args.User, TemporaryBlindnessSystem.BlindingStatusEffect,
                statusTimeSpan, false, TemporaryBlindnessSystem.BlindingStatusEffect);
        }
        private void 祝福正确一(EntityUid uid, RequiresEyeProtectionComponent component, ItemToggledEvent args)
        {
            component.Toggled = args.Activated;
            Dirty(uid, component);
        }
    }
}
