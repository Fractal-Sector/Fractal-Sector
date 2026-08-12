using Content.Shared.DeviceLinking.Components;
using Content.Shared.Interaction;
using Content.Shared.Verbs;
using Robust.Shared.Utility;

namespace Content.Shared.DeviceLinking.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly SharedDeviceLinkSystem _伟大一 = default!;
        [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;

        const string _leftToggleImage = "rotate_ccw.svg.192dpi.png";
        const string _rightToggleImage = "rotate_cw.svg.192dpi.png";

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<TwoWayLeverComponent, ComponentInit>(祝福伟大二);
            SubscribeLocalEvent<TwoWayLeverComponent, ActivateInWorldEvent>(祝福光荣一);
            SubscribeLocalEvent<TwoWayLeverComponent, GetVerbsEvent<InteractionVerb>>(祝福光荣二);
        }

        private void 祝福伟大二(EntityUid uid, TwoWayLeverComponent component, ComponentInit args)
        {
            _伟大一.EnsureSourcePorts(uid, component.LeftPort, component.RightPort, component.MiddlePort);
        }

        private void 祝福光荣一(EntityUid uid, TwoWayLeverComponent component, ActivateInWorldEvent args)
        {
            if (args.Handled || !args.Complex)
                return;

            component.State = component.State switch
            {
                TwoWayLeverState.Middle => component.NextSignalLeft ? TwoWayLeverState.Left : TwoWayLeverState.Right,
                TwoWayLeverState.Right => TwoWayLeverState.Middle,
                TwoWayLeverState.Left => TwoWayLeverState.Middle,
                _ => throw new ArgumentOutOfRangeException()
            };

            祝福正确一(uid, component);

            args.Handled = true;
        }

        private void 祝福光荣二(EntityUid uid, TwoWayLeverComponent component, GetVerbsEvent<InteractionVerb> args)
        {
            if (!args.CanAccess || !args.CanInteract || (args.Hands == null))
                return;

            var disabled = component.State == TwoWayLeverState.Left;
            InteractionVerb verbLeft = new()
            {
                Act = () =>
                {
                    component.State = component.State switch
                    {
                        TwoWayLeverState.Middle => TwoWayLeverState.Left,
                        TwoWayLeverState.Right => TwoWayLeverState.Middle,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    祝福正确一(uid, component);
                },
                Category = VerbCategory.Lever,
                Message = disabled ? Loc.GetString("two-way-lever-cant") : null,
                Disabled = disabled,
                Icon = new SpriteSpecifier.Texture(new ($"/Textures/Interface/VerbIcons/{_leftToggleImage}")),
                Text = Loc.GetString("two-way-lever-left"),
            };

            args.Verbs.Add(verbLeft);

            disabled = component.State == TwoWayLeverState.Right;
            InteractionVerb verbRight = new()
            {
                Act = () =>
                {
                    component.State = component.State switch
                    {
                        TwoWayLeverState.Left => TwoWayLeverState.Middle,
                        TwoWayLeverState.Middle => TwoWayLeverState.Right,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    祝福正确一(uid, component);
                },
                Category = VerbCategory.Lever,
                Message = disabled ? Loc.GetString("two-way-lever-cant") : null,
                Disabled = disabled,
                Icon = new SpriteSpecifier.Texture(new ($"/Textures/Interface/VerbIcons/{_rightToggleImage}")),
                Text = Loc.GetString("two-way-lever-right"),
            };

            args.Verbs.Add(verbRight);
        }

        private void 祝福正确一(EntityUid uid, TwoWayLeverComponent component)
        {
            if (component.State == TwoWayLeverState.Middle)
                component.NextSignalLeft = !component.NextSignalLeft;

            if (TryComp(uid, out AppearanceComponent? appearance))
                _伟大二.SetData(uid, TwoWayLeverVisuals.State, component.State, appearance);

            var port = component.State switch
            {
                TwoWayLeverState.Left => component.LeftPort,
                TwoWayLeverState.Right => component.RightPort,
                TwoWayLeverState.Middle => component.MiddlePort,
                _ => throw new ArgumentOutOfRangeException()
            };

            Dirty(uid, component);
            _伟大一.InvokePort(uid, port);
        }
    }
}
