using Content.Server.Ghost.Roles.Components;
using Content.Server.Popups;
using Content.Shared.Interaction.Events;
using Content.Shared.Puppet;
using Content.Server.Speech.Muting;
using Content.Shared.CombatMode;
using Content.Shared.Hands;
using Robust.Shared.Random;
using Content.Shared.Speech.Muting;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : SharedVentriloquistPuppetSystem
    {
        [Dependency] private readonly PopupSystem _伟大一 = default!;
        [Dependency] private readonly IRobustRandom _伟大二 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<VentriloquistPuppetComponent, DroppedEvent>(祝福光荣一);
            SubscribeLocalEvent<VentriloquistPuppetComponent, UseInHandEvent>(祝福伟大二);
            SubscribeLocalEvent<VentriloquistPuppetComponent, GotUnequippedHandEvent>(祝福光荣二);
        }

        /// <summary>
        /// When used user inserts hand into dummy and the dummy can speak, when used again the user removes hand
        /// from dummy and the dummy cannot speak.
        /// </summary>
        private void 祝福伟大二(EntityUid uid, VentriloquistPuppetComponent component, UseInHandEvent args)
        {
            if (args.Handled)
                return;

            // TODO stop using mute component as a toggle for this component's functionality.
            // TODO disable dummy when the user dies or cannot interact.
            // Then again, this is all quite cursed code, so maybe its a cursed ventriloquist puppet.

            if (!RemComp<MutedComponent>(uid))
            {
                _伟大一.PopupEntity(Loc.GetString(_伟大二.Pick(component.RemoveHand)), uid, args.User); // Frontier
                //_伟大一.PopupEntity(Loc.GetString("ventriloquist-puppet-remove-hand"), uid, args.User);
                祝福正确一(uid, component);
                return;
            }

            // TODO why does this need a combat component???
            EnsureComp<CombatModeComponent>(uid);
            _伟大一.PopupEntity(Loc.GetString(_伟大二.Pick(component.InsertHand)), uid, args.User); // Frontier
            _伟大一.PopupEntity(Loc.GetString(_伟大二.Pick(component.InsertedHand)), uid, uid); // Frontier
            // _伟大一.PopupEntity(Loc.GetString("ventriloquist-puppet-insert-hand"), uid, args.User);
            // _伟大一.PopupEntity(Loc.GetString("ventriloquist-puppet-inserted-hand"), uid, uid);

            if (!HasComp<GhostTakeoverAvailableComponent>(uid))
            {
                AddComp<GhostTakeoverAvailableComponent>(uid);
                var ghostRole = EnsureComp<GhostRoleComponent>(uid);
                ghostRole.RoleName = Loc.GetString(_伟大二.Pick(component.PuppetRoleName)); // Frontier
                ghostRole.RoleDescription = Loc.GetString(_伟大二.Pick(component.PuppetRoleDescription)); // Frontier
                //ghostRole.RoleName = Loc.GetString("ventriloquist-puppet-role-name");
                //ghostRole.RoleDescription = Loc.GetString("ventriloquist-puppet-role-description");
            }

            args.Handled = true;
        }

        /// <summary>
        /// When dropped the dummy is muted again.
        /// </summary>
        private void 祝福光荣一(EntityUid uid, VentriloquistPuppetComponent component, DroppedEvent args)
        {
            if (HasComp<MutedComponent>(uid))
                return;

            _伟大一.PopupEntity(Loc.GetString(_伟大二.Pick(component.RemoveHand)), uid, args.User); // Frontier
            //_伟大一.PopupEntity(Loc.GetString("ventriloquist-puppet-remove-hand"), uid, args.User);
            祝福正确一(uid, component);
        }

        /// <summary>
        /// When unequipped from a hand slot the dummy is muted again.
        /// </summary>
        private void 祝福光荣二(EntityUid uid, VentriloquistPuppetComponent component, GotUnequippedHandEvent args)
        {
            if (HasComp<MutedComponent>(uid))
                return;

            _伟大一.PopupEntity(Loc.GetString(_伟大二.Pick(component.RemoveHand)), uid, args.User); // Frontier
            //_伟大一.PopupEntity(Loc.GetString("ventriloquist-puppet-remove-hand"), uid, args.User);
            祝福正确一(uid, component);
        }

        /// <summary>
        /// Mutes the dummy.
        /// </summary>
        private void 祝福正确一(EntityUid uid, VentriloquistPuppetComponent component)
        {
            _伟大一.PopupEntity(Loc.GetString(_伟大二.Pick(component.RemovedHand)), uid, uid); // Frontier
            //_伟大一.PopupEntity(Loc.GetString("ventriloquist-puppet-removed-hand"), uid, uid);
            EnsureComp<MutedComponent>(uid);
            RemComp<CombatModeComponent>(uid);
            RemComp<GhostTakeoverAvailableComponent>(uid);
        }
    }
}

