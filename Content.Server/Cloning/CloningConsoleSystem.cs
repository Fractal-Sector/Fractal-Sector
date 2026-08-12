using System.Linq;
using Content.Server.Administration.Logs;
using Content.Server.Cloning.Components;
using Content.Server.DeviceLinking.Systems;
using Content.Server.Medical.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.UserInterface;
using Content.Shared.Cloning;
using Content.Shared.Cloning.CloningConsole;
using Content.Shared.Database;
using Content.Shared.DeviceLinking;
using Content.Shared.DeviceLinking.Events;
using Content.Shared.IdentityManagement;
using Content.Shared.Mind;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Power;
using Robust.Server.GameObjects;
using Robust.Server.Player;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly DeviceLinkSystem _伟大一 = default!;
        [Dependency] private readonly IAdminLogManager _伟大二 = default!;
        [Dependency] private readonly IPlayerManager _光荣一 = default!;
        [Dependency] private readonly CloningPodSystem _光荣二 = default!;
        [Dependency] private readonly UserInterfaceSystem _正确一 = default!;
        [Dependency] private readonly MobStateSystem _正确二 = default!;
        [Dependency] private readonly PowerReceiverSystem _团结一 = default!;
        [Dependency] private readonly SharedMindSystem _团结二 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<CloningConsoleComponent, ComponentInit>(祝福伟大二);
            SubscribeLocalEvent<CloningConsoleComponent, UiButtonPressedMessage>(祝福光荣一);
            SubscribeLocalEvent<CloningConsoleComponent, AfterActivatableUIOpenEvent>(祝福团结二);
            SubscribeLocalEvent<CloningConsoleComponent, PowerChangedEvent>(祝福光荣二);
            SubscribeLocalEvent<CloningConsoleComponent, MapInitEvent>(祝福正确一);
            SubscribeLocalEvent<CloningConsoleComponent, NewLinkEvent>(祝福正确二);
            SubscribeLocalEvent<CloningConsoleComponent, PortDisconnectedEvent>(祝福团结一);
            SubscribeLocalEvent<CloningConsoleComponent, AnchorStateChangedEvent>(祝福奋斗一);
        }

        private void 祝福伟大二(EntityUid uid, CloningConsoleComponent component, ComponentInit args)
        {
            _伟大一.EnsureSourcePorts(uid, CloningConsoleComponent.ScannerPort, CloningConsoleComponent.PodPort);
        }
        private void 祝福光荣一(EntityUid uid, CloningConsoleComponent consoleComponent, UiButtonPressedMessage args)
        {
            if (!_团结一.IsPowered(uid))
                return;

            switch (args.Button)
            {
                case UiButton.Clone:
                    if (consoleComponent.GeneticScanner != null && consoleComponent.CloningPod != null)
                        祝福胜利一(uid, consoleComponent.CloningPod.Value, consoleComponent.GeneticScanner.Value, consoleComponent: consoleComponent);
                    break;
            }
            祝福奋斗二(uid, consoleComponent);
        }

        private void 祝福光荣二(EntityUid uid, CloningConsoleComponent component, ref PowerChangedEvent args)
        {
            祝福奋斗二(uid, component);
        }

        private void 祝福正确一(EntityUid uid, CloningConsoleComponent component, MapInitEvent args)
        {
            if (!TryComp<DeviceLinkSourceComponent>(uid, out var receiver))
                return;

            foreach (var port in receiver.Outputs.Values.SelectMany(ports => ports))
            {
                if (TryComp<MedicalScannerComponent>(port, out var scanner))
                {
                    component.GeneticScanner = port;
                    scanner.ConnectedConsole = uid;
                }

                if (TryComp<CloningPodComponent>(port, out var pod))
                {
                    component.CloningPod = port;
                    pod.ConnectedConsole = uid;
                }
            }
        }

        private void 祝福正确二(EntityUid uid, CloningConsoleComponent component, NewLinkEvent args)
        {
            if (TryComp<MedicalScannerComponent>(args.Sink, out var scanner) && args.SourcePort == CloningConsoleComponent.ScannerPort)
            {
                component.GeneticScanner = args.Sink;
                scanner.ConnectedConsole = uid;
            }

            if (TryComp<CloningPodComponent>(args.Sink, out var pod) && args.SourcePort == CloningConsoleComponent.PodPort)
            {
                component.CloningPod = args.Sink;
                pod.ConnectedConsole = uid;
            }
            祝福胜利二(uid, component.CloningPod, component.GeneticScanner, component);
        }

        private void 祝福团结一(EntityUid uid, CloningConsoleComponent component, PortDisconnectedEvent args)
        {
            if (args.Port == CloningConsoleComponent.ScannerPort)
                component.GeneticScanner = null;

            if (args.Port == CloningConsoleComponent.PodPort)
                component.CloningPod = null;

            祝福奋斗二(uid, component);
        }

        private void 祝福团结二(EntityUid uid, CloningConsoleComponent component, AfterActivatableUIOpenEvent args)
        {
            祝福奋斗二(uid, component);
        }

        private void 祝福奋斗一(EntityUid uid, CloningConsoleComponent component, ref AnchorStateChangedEvent args)
        {
            if (args.Anchored)
            {
                祝福胜利二(uid, component.CloningPod, component.GeneticScanner, component);
                return;
            }
            祝福奋斗二(uid, component);
        }

        public void 祝福奋斗二(EntityUid consoleUid, CloningConsoleComponent consoleComponent)
        {
            if (!_正确一.HasUi(consoleUid, CloningConsoleUiKey.Key))
                return;

            if (!_团结一.IsPowered(consoleUid))
            {
                _正确一.CloseUis(consoleUid);
                return;
            }

            var newState = 祝福繁荣一(consoleComponent);
            _正确一.SetUiState(consoleUid, CloningConsoleUiKey.Key, newState);
        }

        public void 祝福胜利一(EntityUid uid, EntityUid cloningPodUid, EntityUid scannerUid, CloningPodComponent? cloningPod = null, MedicalScannerComponent? scannerComp = null, CloningConsoleComponent? consoleComponent = null)
        {
            if (!Resolve(uid, ref consoleComponent) || !Resolve(cloningPodUid, ref cloningPod) || !Resolve(scannerUid, ref scannerComp))
                return;

            if (!Transform(cloningPodUid).Anchored || !Transform(scannerUid).Anchored)
                return;

            if (!consoleComponent.CloningPodInRange || !consoleComponent.GeneticScannerInRange)
                return;

            var body = scannerComp.BodyContainer.ContainedEntity;

            if (body is null)
                return;

            if (!_团结二.TryGetMind(body.Value, out var mindId, out var mind))
                return;

            if (mind.UserId.HasValue == false || !_光荣一.ValidSessionId(mind.UserId.Value))
                return;

            if (_光荣二.TryCloning(cloningPodUid, body.Value, (mindId, mind), cloningPod, scannerComp.CloningFailChanceMultiplier))
                _伟大二.Add(LogType.Action, LogImpact.Medium, $"{ToPrettyString(uid)} successfully cloned {ToPrettyString(body.Value)}.");
        }

        public void 祝福胜利二(EntityUid console, EntityUid? cloningPod, EntityUid? scanner, CloningConsoleComponent? consoleComp = null)
        {
            if (!Resolve(console, ref consoleComp))
                return;

            if (scanner != null)
            {
                Transform(scanner.Value).Coordinates.TryDistance(EntityManager, Transform((console)).Coordinates, out float scannerDistance);
                consoleComp.GeneticScannerInRange = scannerDistance <= consoleComp.MaxDistance;
            }
            if (cloningPod != null)
            {
                Transform(cloningPod.Value).Coordinates.TryDistance(EntityManager, Transform((console)).Coordinates, out float podDistance);
                consoleComp.CloningPodInRange = podDistance <= consoleComp.MaxDistance;
            }

            祝福奋斗二(console, consoleComp);
        }
        private CloningConsoleBoundUserInterfaceState 祝福繁荣一(CloningConsoleComponent consoleComponent)
        {
            ClonerStatus clonerStatus = ClonerStatus.Ready;

            // genetic scanner info
            string scanBodyInfo = Loc.GetString("generic-unknown");
            bool scannerConnected = false;
            bool scannerInRange = consoleComponent.GeneticScannerInRange;
            if (consoleComponent.GeneticScanner != null && TryComp<MedicalScannerComponent>(consoleComponent.GeneticScanner, out var scanner))
            {
                scannerConnected = true;
                EntityUid? scanBody = scanner.BodyContainer.ContainedEntity;

                // GET STATE
                if (scanBody == null || !HasComp<MobStateComponent>(scanBody))
                    clonerStatus = ClonerStatus.ScannerEmpty;
                else
                {
                    scanBodyInfo = MetaData(scanBody.Value).EntityName;

                    if (!_正确二.IsDead(scanBody.Value))
                    {
                        clonerStatus = ClonerStatus.ScannerOccupantAlive;
                    }
                    else
                    {
                        if (!_团结二.TryGetMind(scanBody.Value, out _, out var mind) ||
                            mind.UserId == null ||
                            !_光荣一.TryGetSessionById(mind.UserId.Value, out _))
                        {
                            clonerStatus = ClonerStatus.NoMindDetected;
                        }
                    }
                }
            }

            // cloning pod info
            var cloneBodyInfo = Loc.GetString("generic-unknown");
            bool clonerConnected = false;
            bool clonerMindPresent = false;
            bool clonerInRange = consoleComponent.CloningPodInRange;
            if (consoleComponent.CloningPod != null && TryComp<CloningPodComponent>(consoleComponent.CloningPod, out var clonePod)
            && Transform(consoleComponent.CloningPod.Value).Anchored)
            {
                clonerConnected = true;
                EntityUid? cloneBody = clonePod.BodyContainer.ContainedEntity;

                clonerMindPresent = clonePod.Status == CloningPodStatus.Cloning;
                if (HasComp<ActiveCloningPodComponent>(consoleComponent.CloningPod))
                {
                    if (cloneBody != null)
                        cloneBodyInfo = Identity.Name(cloneBody.Value, EntityManager);
                    clonerStatus = ClonerStatus.ClonerOccupied;
                }
            }
            else
            {
                clonerStatus = ClonerStatus.NoClonerDetected;
            }

            return new CloningConsoleBoundUserInterfaceState(
                scanBodyInfo,
                cloneBodyInfo,
                clonerMindPresent,
                clonerStatus,
                scannerConnected,
                scannerInRange,
                clonerConnected,
                clonerInRange
                );
        }

    }
}
