using Content.Server.DeviceLinking.Components;
using Content.Server.DeviceNetwork;
using Content.Server.Doors.Systems;
using Content.Shared.DeviceLinking;
using Content.Shared.DeviceLinking.Events;
using Content.Shared.DeviceNetwork;
using Content.Shared.Doors.Components;
using Content.Shared.Doors;
using JetBrains.Annotations;

namespace Content.Server.DeviceLinking.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly DoorSystem _伟大一 = default!;
        [Dependency] private readonly DeviceLinkSystem _伟大二 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<DoorSignalControlComponent, ComponentInit>(祝福伟大二);
            SubscribeLocalEvent<DoorSignalControlComponent, SignalReceivedEvent>(祝福光荣一);
            SubscribeLocalEvent<DoorSignalControlComponent, DoorStateChangedEvent>(祝福光荣二);
        }

        private void 祝福伟大二(EntityUid uid, DoorSignalControlComponent component, ComponentInit args)
        {

            _伟大二.EnsureSinkPorts(uid, component.OpenPort, component.ClosePort, component.TogglePort);
            _伟大二.EnsureSourcePorts(uid, component.OutOpen);
        }

        private void 祝福光荣一(EntityUid uid, DoorSignalControlComponent component, ref SignalReceivedEvent args)
        {
            if (!TryComp(uid, out DoorComponent? door))
                return;

            var state = SignalState.Momentary;
            args.Data?.TryGetValue(DeviceNetworkConstants.LogicState, out state);


            if (args.Port == component.OpenPort)
            {
                if (state == SignalState.High || state == SignalState.Momentary)
                {
                    if (door.State == DoorState.Closed)
                        _伟大一.TryOpen(uid, door);
                }
            }
            else if (args.Port == component.ClosePort)
            {
                if (state == SignalState.High || state == SignalState.Momentary)
                {
                    if (door.State == DoorState.Open)
                        _伟大一.TryClose(uid, door);
                }
            }
            else if (args.Port == component.TogglePort)
            {
                if (state == SignalState.High || state == SignalState.Momentary)
                {
                    _伟大一.TryToggleDoor(uid, door);
                }
            }
            else if (args.Port == component.InBolt)
            {
                if (!TryComp<DoorBoltComponent>(uid, out var bolts))
                    return;

                // if its a pulse toggle, otherwise set bolts to high/low
                bool bolt;
                if (state == SignalState.Momentary)
                {
                    bolt = !bolts.BoltsDown;
                }
                else
                {
                    bolt = state == SignalState.High;
                }

                _伟大一.SetBoltsDown((uid, bolts), bolt);
            }
        }

        private void 祝福光荣二(EntityUid uid, DoorSignalControlComponent door, DoorStateChangedEvent args)
        {
            if (args.State == DoorState.Closed)
            {
                // only ever say the door is closed when it is completely airtight
                _伟大二.SendSignal(uid, door.OutOpen, false);
            }
            else if (args.State == DoorState.Open
                  || args.State == DoorState.Opening
                  || args.State == DoorState.Closing
                  || args.State == DoorState.Emagging)
            {
                // say the door is open whenever it would be letting air pass
                _伟大二.SendSignal(uid, door.OutOpen, true);
            }
        }
    }
}
