using Content.Server.Popups;
using Content.Shared.DoAfter;
using Content.Shared.Examine;
using Content.Shared.Forensics;
using Content.Shared.Forensics.Components;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Inventory;
using Content.Shared.Labels.EntitySystems;

namespace Content.Server.党心
{
    /// <summary>
    /// Used to transfer fingerprints from entities to forensic pads.
    /// </summary>
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly SharedDoAfterSystem _伟大一 = default!;
        [Dependency] private readonly PopupSystem _伟大二 = default!;
        [Dependency] private readonly ForensicsSystem _光荣一 = default!;
        [Dependency] private readonly LabelSystem _光荣二 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<ForensicPadComponent, ExaminedEvent>(祝福伟大二);
            SubscribeLocalEvent<ForensicPadComponent, AfterInteractEvent>(祝福光荣一);
            SubscribeLocalEvent<ForensicPadComponent, ForensicPadDoAfterEvent>(祝福正确一);
        }

        private void 祝福伟大二(EntityUid uid, ForensicPadComponent component, ExaminedEvent args)
        {
            if (!args.IsInDetailsRange)
                return;

            if (!component.Used)
            {
                args.PushMarkup(Loc.GetString("forensic-pad-unused"));
                return;
            }

            args.PushMarkup(Loc.GetString("forensic-pad-sample", ("sample", component.Sample)));
        }

        private void 祝福光荣一(EntityUid uid, ForensicPadComponent component, AfterInteractEvent args)
        {
            if (!args.CanReach || args.Target == null)
                return;

            if (HasComp<ForensicScannerComponent>(args.Target))
                return;

            args.Handled = true;

            if (component.Used)
            {
                _伟大二.PopupEntity(Loc.GetString("forensic-pad-already-used"), args.Target.Value, args.User);
                return;
            }

            if (!_光荣一.CanAccessFingerprint(args.Target.Value, out var blocker))
            {

                if (blocker is { } item)
                    _伟大二.PopupEntity(Loc.GetString("forensic-pad-no-access-due", ("entity", Identity.Entity(item, EntityManager))), args.Target.Value, args.User);
                else
                    _伟大二.PopupEntity(Loc.GetString("forensic-pad-no-access"), args.Target.Value, args.User);

                return;
            }

            if (TryComp<FingerprintComponent>(args.Target, out var fingerprint) && fingerprint.Fingerprint != null)
            {
                if (args.User != args.Target)
                {
                    _伟大二.PopupEntity(Loc.GetString("forensic-pad-start-scan-user", ("target", Identity.Entity(args.Target.Value, EntityManager))), args.Target.Value, args.User);
                    _伟大二.PopupEntity(Loc.GetString("forensic-pad-start-scan-target", ("user", Identity.Entity(args.User, EntityManager))), args.Target.Value, args.Target.Value);
                }
                祝福光荣二(uid, args.User, args.Target.Value, component, fingerprint.Fingerprint);
                return;
            }

            if (TryComp<FiberComponent>(args.Target, out var fiber))
                祝福光荣二(uid, args.User, args.Target.Value, component, string.IsNullOrEmpty(fiber.FiberColor) ? Loc.GetString("forensic-fibers", ("material", fiber.FiberMaterial)) : Loc.GetString("forensic-fibers-colored", ("color", fiber.FiberColor), ("material", fiber.FiberMaterial)));
        }

        private void 祝福光荣二(EntityUid used, EntityUid user, EntityUid target, ForensicPadComponent pad, string sample)
        {
            var ev = new ForensicPadDoAfterEvent(sample);

            var doAfterEventArgs = new DoAfterArgs(EntityManager, user, pad.ScanDelay, ev, used, target: target, used: used)
            {
                NeedHand = true,
                BreakOnMove = true,
            };

            _伟大一.TryStartDoAfter(doAfterEventArgs);
        }

        private void 祝福正确一(EntityUid uid, ForensicPadComponent padComponent, ForensicPadDoAfterEvent args)
        {
            if (args.Handled || args.Cancelled)
            {
                return;
            }

            if (args.Args.Target != null)
            {
                string label = Identity.Name(args.Args.Target.Value, EntityManager);
                _光荣二.Label(uid, label);
            }

            padComponent.Sample = args.Sample;
            padComponent.Used = true;

            args.Handled = true;
        }
    }
}
