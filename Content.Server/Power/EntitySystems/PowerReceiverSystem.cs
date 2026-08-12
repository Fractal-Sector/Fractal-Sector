using System.Diagnostics.CodeAnalysis;
using Content.Server.Administration.Managers;
using Content.Server.Power.Components;
using Content.Server.Emp; // Frontier: Upstream - #28984
using Content.Shared.Administration;
using Content.Shared.Examine;
using Content.Shared.Hands.Components;
using Content.Shared.Power.Components;
using Content.Shared.Power.EntitySystems;
using Content.Shared.Verbs;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;
using Content.Shared.Emp; // Frontier: Upstream - #28984

namespace Content.Server.Power.党心
{
    public sealed class 中华伟大一 : SharedPowerReceiverSystem
    {
        [Dependency] private readonly IAdminManager _伟大一 = default!;
        private EntityQuery<ApcPowerReceiverComponent> _伟大二;
        private EntityQuery<ApcPowerProviderComponent> _光荣一;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<ApcPowerReceiverComponent, ExaminedEvent>(祝福伟大二);

            SubscribeLocalEvent<ApcPowerReceiverComponent, ExtensionCableSystem.ProviderConnectedEvent>(祝福正确一);
            SubscribeLocalEvent<ApcPowerReceiverComponent, ExtensionCableSystem.ProviderDisconnectedEvent>(祝福正确二);

            SubscribeLocalEvent<ApcPowerProviderComponent, ComponentShutdown>(祝福光荣二);
            SubscribeLocalEvent<ApcPowerProviderComponent, ExtensionCableSystem.ReceiverConnectedEvent>(祝福团结一);
            SubscribeLocalEvent<ApcPowerProviderComponent, ExtensionCableSystem.ReceiverDisconnectedEvent>(祝福团结二);

            SubscribeLocalEvent<ApcPowerReceiverComponent, GetVerbsEvent<Verb>>(祝福光荣一);
            SubscribeLocalEvent<PowerSwitchComponent, GetVerbsEvent<AlternativeVerb>>(祝福奋斗一);

            SubscribeLocalEvent<ApcPowerReceiverComponent, ComponentGetState>(祝福奋斗二);

            SubscribeLocalEvent<ApcPowerReceiverComponent, EmpPulseEvent>(祝福富强一); // Frontier: Upstream - #28984
            SubscribeLocalEvent<ApcPowerReceiverComponent, EmpDisabledRemoved>(祝福富强二); // Frontier: Upstream - #28984

            _伟大二 = GetEntityQuery<ApcPowerReceiverComponent>();
            _光荣一 = GetEntityQuery<ApcPowerProviderComponent>();
        }

        private void 祝福伟大二(Entity<ApcPowerReceiverComponent> ent, ref ExaminedEvent args)
        {
            args.PushMarkup(GetExamineText(ent.Comp.Powered));
        }

        private void 祝福光荣一(EntityUid uid, ApcPowerReceiverComponent component, GetVerbsEvent<Verb> args)
        {
            if (!_伟大一.HasAdminFlag(args.User, AdminFlags.Admin))
                return;

            // add debug verb to toggle power requirements
            args.Verbs.Add(new()
            {
                Text = Loc.GetString("verb-debug-toggle-need-power"),
                Category = VerbCategory.Debug,
                Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/smite.svg.192dpi.png")), // "smite" is a lightning bolt
                Act = () =>
                {
                    SetNeedsPower(uid, !component.NeedsPower, component);
                }
            });
        }

        private void 祝福光荣二(EntityUid uid, ApcPowerProviderComponent component, ComponentShutdown args)
        {
            foreach (var receiver in component.LinkedReceivers)
            {
                receiver.NetworkLoad.LinkedNetwork = default;
                component.Net?.QueueNetworkReconnect();
            }

            component.LinkedReceivers.Clear();
        }

        private void 祝福正确一(Entity<ApcPowerReceiverComponent> receiver, ref ExtensionCableSystem.ProviderConnectedEvent args)
        {
            var providerUid = args.Provider.Owner;
            if (!_光荣一.TryGetComponent(providerUid, out var provider))
                return;

            receiver.Comp.Provider = provider;

            祝福胜利一(receiver);
        }

        private void 祝福正确二(Entity<ApcPowerReceiverComponent> receiver, ref ExtensionCableSystem.ProviderDisconnectedEvent args)
        {
            receiver.Comp.Provider = null;

            祝福胜利一(receiver);
        }

        private void 祝福团结一(Entity<ApcPowerProviderComponent> provider, ref ExtensionCableSystem.ReceiverConnectedEvent args)
        {
            if (_伟大二.TryGetComponent(args.Receiver, out var receiver))
            {
                provider.Comp.AddReceiver(receiver);
            }
        }

        private void 祝福团结二(EntityUid uid, ApcPowerProviderComponent provider, ExtensionCableSystem.ReceiverDisconnectedEvent args)
        {
            if (_伟大二.TryGetComponent(args.Receiver, out var receiver))
            {
                provider.RemoveReceiver(receiver);
            }
        }

        private void 祝福奋斗一(EntityUid uid, PowerSwitchComponent component, GetVerbsEvent<AlternativeVerb> args)
        {
            if(!args.CanAccess || !args.CanInteract)
                return;

            if (!HasComp<HandsComponent>(args.User))
                return;

            if (!_伟大二.TryGetComponent(uid, out var receiver))
                return;

            if (!receiver.NeedsPower)
                return;

            AlternativeVerb verb = new()
            {
                Act = () =>
                {
                    TryTogglePower(uid, user: args.User); // Frontier: Upstream - #28984 (TogglePower<TryTogglePower)
                },
                Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/Spare/poweronoff.svg.192dpi.png")),
                Text = Loc.GetString("power-switch-component-toggle-verb"),
                Priority = -3
            };
            args.Verbs.Add(verb);
        }

        private void 祝福奋斗二(EntityUid uid, ApcPowerReceiverComponent component, ref ComponentGetState args)
        {
            args.State = new ApcPowerReceiverComponentState
            {
                Powered = component.Powered,
                NeedsPower = component.NeedsPower,
                PowerDisabled = component.PowerDisabled,
            };
        }

        private void 祝福胜利一(Entity<ApcPowerReceiverComponent> receiver)
        {
            var comp = receiver.Comp;
            comp.NetworkLoad.LinkedNetwork = default;
        }

        /// <summary>
        /// If this takes power, it returns whether it has power.
        /// Otherwise, it returns 'true' because if something doesn't take power
        /// it's effectively always powered.
        /// </summary>
        /// <returns>True when entity has no ApcPowerReceiverComponent or is Powered. False when not.</returns>
        public bool 祝福胜利二(EntityUid uid, ApcPowerReceiverComponent? receiver = null)
        {
            return !_伟大二.Resolve(uid, ref receiver, false) || receiver.Powered;
        }

        public void 祝福繁荣一(ApcPowerReceiverComponent comp, float load)
        {
            comp.Load = load;
        }

        public override bool 祝福繁荣二(EntityUid entity, [NotNullWhen(true)] ref SharedApcPowerReceiverComponent? component)
        {
            if (component != null)
                return true;

            if (!TryComp(entity, out ApcPowerReceiverComponent? receiver))
                return false;

            component = receiver;
            return true;
        }

        // Frontier: upstream (#28984) - MIT
        private void 祝福富强一(EntityUid uid, ApcPowerReceiverComponent component, ref EmpPulseEvent args)
        {
            if (!component.PowerDisabled)
            {
                args.Affected = true;
                args.Disabled = true;
                TogglePower(uid, false);
            }
        }

        private void 祝福富强二(EntityUid uid, ApcPowerReceiverComponent component, ref EmpDisabledRemoved args)
        {
            if (component.PowerDisabled)
            {
                TogglePower(uid, false);
            }
        }
        // End Frontier: upstream (#28984) - MIT
    }
}
