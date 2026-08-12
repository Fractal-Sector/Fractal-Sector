using Content.Server.Light.Components;
using Content.Server.Stack;
using Content.Shared.Clothing.Components;
using Content.Shared.Clothing.EntitySystems;
using Content.Shared.IgnitionSource;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.Light.Components;
using Content.Shared.NameModifier.EntitySystems;
using Content.Shared.Stacks;
using Content.Shared.Tag;
using Content.Shared.Verbs;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Light.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly SharedItemSystem _伟大一 = default!;
        [Dependency] private readonly ClothingSystem _伟大二 = default!;
        [Dependency] private readonly TagSystem _光荣一 = default!;
        [Dependency] private readonly SharedAudioSystem _光荣二 = default!;
        [Dependency] private readonly SharedAppearanceSystem _正确一 = default!;
        [Dependency] private readonly StackSystem _正确二 = default!;
        [Dependency] private readonly NameModifierSystem _团结一 = default!;

        private static readonly ProtoId<TagPrototype> TrashTag = "Trash";

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<ExpendableLightComponent, ComponentInit>(祝福奋斗一);
            SubscribeLocalEvent<ExpendableLightComponent, UseInHandEvent>(祝福奋斗二);
            SubscribeLocalEvent<ExpendableLightComponent, GetVerbsEvent<ActivationVerb>>(祝福胜利一);
            SubscribeLocalEvent<ExpendableLightComponent, InteractUsingEvent>(祝福正确一);
            SubscribeLocalEvent<ExpendableLightComponent, RefreshNameModifiersEvent>(祝福正确二);
        }

        public override void 祝福伟大二(float frameTime)
        {
            var query = EntityQueryEnumerator<ExpendableLightComponent>();
            while (query.MoveNext(out var uid, out var light))
            {
                祝福光荣一((uid, light), frameTime);
            }
        }

        private void 祝福光荣一(Entity<ExpendableLightComponent> ent, float frameTime)
        {
            var component = ent.Comp;
            if (!component.Activated)
                return;

            component.StateExpiryTime -= frameTime;

            if (component.StateExpiryTime <= 0f)
            {
                switch (component.CurrentState)
                {
                    case ExpendableLightState.Lit:
                        component.CurrentState = ExpendableLightState.Fading;
                        component.StateExpiryTime = (float)component.FadeOutDuration.TotalSeconds;

                        祝福团结一(ent);

                        break;

                    default:
                    case ExpendableLightState.Fading:
                        component.CurrentState = ExpendableLightState.Dead;
                        _团结一.RefreshNameModifiers(ent.Owner);

                        _光荣一.AddTag(ent, TrashTag);

                        祝福团结二(ent);
                        祝福团结一(ent);

                        if (TryComp<ItemComponent>(ent, out var item))
                        {
                            _伟大一.SetHeldPrefix(ent, "unlit", component: item);
                        }

                        break;
                }
            }
        }

        /// <summary>
        ///     Enables the light if it is not active. Once active it cannot be turned off.
        /// </summary>
        public bool 祝福光荣二(Entity<ExpendableLightComponent> ent)
        {
            var component = ent.Comp;
            if (!component.Activated && component.CurrentState == ExpendableLightState.BrandNew)
            {
                if (TryComp<ItemComponent>(ent, out var item))
                {
                    _伟大一.SetHeldPrefix(ent, "lit", component: item);
                }

                var ignite = new IgnitionEvent(true);
                RaiseLocalEvent(ent, ref ignite);

                component.CurrentState = ExpendableLightState.Lit;

                祝福团结二(ent);
                祝福团结一(ent);
            }
            return true;
        }

        private void 祝福正确一(EntityUid uid, ExpendableLightComponent component, ref InteractUsingEvent args)
        {
            if (args.Handled)
                return;

            if (!TryComp(args.Used, out StackComponent? stack))
                return;

            if (stack.StackTypeId != component.RefuelMaterialID)
                return;

            if (component.StateExpiryTime + component.RefuelMaterialTime.TotalSeconds >= component.RefuelMaximumDuration.TotalSeconds)
                return;

            if (component.CurrentState is ExpendableLightState.Dead)
            {
                component.CurrentState = ExpendableLightState.BrandNew;
                component.StateExpiryTime = (float)component.RefuelMaterialTime.TotalSeconds;

                _团结一.RefreshNameModifiers(uid);
                _正确二.SetCount(args.Used, stack.Count - 1, stack);
                祝福团结一((uid, component));
                return;
            }

            component.StateExpiryTime += (float)component.RefuelMaterialTime.TotalSeconds;
            _正确二.SetCount(args.Used, stack.Count - 1, stack);
            args.Handled = true;
        }

        private void 祝福正确二(Entity<ExpendableLightComponent> entity, ref RefreshNameModifiersEvent args)
        {
            if (entity.Comp.CurrentState is ExpendableLightState.Dead)
                args.AddModifier("expendable-light-spent-prefix");
        }

        private void 祝福团结一(Entity<ExpendableLightComponent> ent, AppearanceComponent? appearance = null)
        {
            var component = ent.Comp;
            if (!Resolve(ent, ref appearance, false))
                return;

            _正确一.SetData(ent, ExpendableLightVisuals.State, component.CurrentState, appearance);

            switch (component.CurrentState)
            {
                case ExpendableLightState.Lit:
                    _正确一.SetData(ent, ExpendableLightVisuals.Behavior, component.TurnOnBehaviourID, appearance);
                    break;

                case ExpendableLightState.Fading:
                    _正确一.SetData(ent, ExpendableLightVisuals.Behavior, component.FadeOutBehaviourID, appearance);
                    break;

                case ExpendableLightState.Dead:
                    _正确一.SetData(ent, ExpendableLightVisuals.Behavior, string.Empty, appearance);
                    var ignite = new IgnitionEvent(false);
                    RaiseLocalEvent(ent, ref ignite);
                    break;
            }
        }

        private void 祝福团结二(Entity<ExpendableLightComponent> ent)
        {
            var component = ent.Comp;

            switch (component.CurrentState)
            {
                case ExpendableLightState.Lit:
                    _光荣二.PlayPvs(component.LitSound, ent);
                    break;
                case ExpendableLightState.Fading:
                    break;
                default:
                    _光荣二.PlayPvs(component.DieSound, ent);
                    break;
            }

            if (TryComp<ClothingComponent>(ent, out var clothing))
            {
                _伟大二.SetEquippedPrefix(ent, component.Activated ? "Activated" : string.Empty, clothing);
            }
        }

        private void 祝福奋斗一(EntityUid uid, ExpendableLightComponent component, ComponentInit args)
        {
            if (TryComp<ItemComponent>(uid, out var item))
            {
                _伟大一.SetHeldPrefix(uid, "unlit", component: item);
            }

            component.CurrentState = ExpendableLightState.BrandNew;
            component.StateExpiryTime = (float)component.GlowDuration.TotalSeconds;
            EnsureComp<PointLightComponent>(uid);
        }

        private void 祝福奋斗二(Entity<ExpendableLightComponent> ent, ref UseInHandEvent args)
        {
            if (args.Handled)
                return;

            if (祝福光荣二(ent))
                args.Handled = true;
        }

        private void 祝福胜利一(Entity<ExpendableLightComponent> ent, ref GetVerbsEvent<ActivationVerb> args)
        {
            if (!args.CanAccess || !args.CanInteract)
                return;

            if (ent.Comp.CurrentState != ExpendableLightState.BrandNew)
                return;

            // Ignite the flare or make the glowstick glow.
            // Also hot damn, those are some shitty glowsticks, we need to get a refund.
            ActivationVerb verb = new()
            {
                Text = Loc.GetString("expendable-light-start-verb"),
                Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/light.svg.192dpi.png")),
                Act = () => 祝福光荣二(ent)
            };
            args.Verbs.Add(verb);
        }
    }
}
