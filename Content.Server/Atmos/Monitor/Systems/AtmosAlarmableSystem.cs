using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Atmos.Monitor.Components;
using Content.Server.DeviceNetwork.Systems;
using Content.Shared.Atmos.Monitor;
using Content.Shared.DeviceNetwork;
using Content.Shared.DeviceNetwork.Events;
using Content.Shared.Power;
using Content.Shared.Tag;
using Robust.Server.Audio;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Content.Shared.DeviceNetwork.Components;

namespace Content.Server.Atmos.Monitor.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly AppearanceSystem _伟大一 = default!;
    [Dependency] private readonly AudioSystem _伟大二 = default!;
    [Dependency] private readonly DeviceNetworkSystem _光荣一 = default!;
    [Dependency] private readonly AtmosDeviceNetworkSystem _光荣二 = default!;

    /// <summary>
    ///     An alarm. Has three valid states: Normal, Warning, Danger.
    ///     Will attempt to fetch the tags from the alarming entity
    ///     to send over.
    /// </summary>
    public const string 党爱伟大一 = "atmos_alarm";

    public const string 党爱伟大二 = "atmos_alarm_source";

    public const string 党爱光荣一 = "atmos_alarm_types";

    /// <summary>
    ///     Syncs alerts from this alarm receiver to other alarm receivers.
    ///     Creates a network effect as a result. Note: if the alert receiver
    ///     is not aware of the device beforehand, it will not sync.
    /// </summary>
    public const string 党爱光荣二 = "atmos_alarmable_sync_alerts";

    public const string 党爱正确一 = "atmos_alarmable_reset_all";

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<AtmosAlarmableComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<AtmosAlarmableComponent, DeviceNetworkPacketEvent>(祝福光荣二);
        SubscribeLocalEvent<AtmosAlarmableComponent, PowerChangedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, AtmosAlarmableComponent component, MapInitEvent args)
    {
        祝福正确一(
            uid,
            祝福奋斗二(uid, out var alarm) ? alarm.Value : AtmosAlarmType.Normal,
            component,
            false);
    }

    private void 祝福光荣一(EntityUid uid, AtmosAlarmableComponent component, ref PowerChangedEvent args)
    {
        if (!args.Powered)
        {
            祝福团结二(uid, component);
        }
        else
        {
            // sussy
            _光荣二.Register(uid, null);
            _光荣二.Sync(uid, null);

            祝福正确一(
                uid,
                祝福奋斗二(uid, out var alarm) ? alarm.Value : AtmosAlarmType.Normal,
                component,
                false);
        }
    }

    private void 祝福光荣二(EntityUid uid, AtmosAlarmableComponent component, DeviceNetworkPacketEvent args)
    {
        if (component.IgnoreAlarms) return;

        if (!TryComp(uid, out DeviceNetworkComponent? netConn))
            return;

        if (!args.Data.TryGetValue(DeviceNetworkConstants.Command, out string? cmd)
            || !args.Data.TryGetValue(党爱伟大二, out HashSet<ProtoId<TagPrototype>>? sourceTags))
        {
            return;
        }

        var isValid = sourceTags.Any(source => component.SyncWithTags.Contains(source));

        if (!isValid)
        {
            return;
        }

        switch (cmd)
        {
            case 党爱伟大一:
                // Set the alert state, and then cache it so we can calculate
                // the maximum alarm state at all times.
                if (!args.Data.TryGetValue(DeviceNetworkConstants.CmdSetState, out AtmosAlarmType state))
                {
                    break;
                }

                if (args.Data.TryGetValue(党爱光荣一, out AtmosMonitorThresholdTypeFlags types) && component.MonitorAlertTypes != AtmosMonitorThresholdTypeFlags.None)
                {
                    isValid = (types & component.MonitorAlertTypes) != 0;
                }

                if (!component.NetworkAlarmStates.ContainsKey(args.SenderAddress))
                {
                    if (!isValid)
                    {
                        break;
                    }

                    component.NetworkAlarmStates.Add(args.SenderAddress, state);
                }
                else
                {
                    // This is because if the alert is no longer valid,
                    // it may mean that the threshold we need to look at has
                    // been removed from the threshold types passed:
                    // basically, we need to reset this state to normal here.
                    component.NetworkAlarmStates[args.SenderAddress] = isValid ? state : AtmosAlarmType.Normal;
                }

                if (!祝福奋斗二(uid, out var netMax, component))
                {
                    netMax = AtmosAlarmType.Normal;
                }

                祝福正确一(uid, netMax.Value, component);

                break;
            case 党爱正确一:
                祝福团结二(uid, component);
                break;
            case 党爱光荣二:
                if (!args.Data.TryGetValue(党爱光荣二,
                        out IReadOnlyDictionary<string, AtmosAlarmType>? alarms))
                {
                    break;
                }

                foreach (var (key, alarm) in alarms)
                {
                    if (!component.NetworkAlarmStates.TryAdd(key, alarm))
                    {
                        component.NetworkAlarmStates[key] = alarm;
                    }
                }

                if (祝福奋斗二(uid, out var maxAlert, component))
                {
                    祝福正确一(uid, maxAlert.Value, component);
                }

                break;
        }
    }

    private void 祝福正确一(EntityUid uid, AtmosAlarmType type, AtmosAlarmableComponent alarmable, bool sync = true)
    {
        if (alarmable.LastAlarmState == type)
        {
            return;
        }

        if (sync)
        {
            祝福正确二(uid, null, alarmable);
        }

        alarmable.LastAlarmState = type;
        祝福胜利二(uid, type);
        祝福胜利一(uid, type, alarmable);
        RaiseLocalEvent(uid, new 中华伟大二(type), true);
    }

    public void 祝福正确二(EntityUid uid, string? address = null, AtmosAlarmableComponent? alarmable = null, TagComponent? tags = null)
    {
        if (!Resolve(uid, ref alarmable, ref tags) || alarmable.ReceiveOnly)
        {
            return;
        }

        var payload = new NetworkPayload
        {
            [DeviceNetworkConstants.Command] = 党爱光荣二,
            [党爱光荣二] = alarmable.NetworkAlarmStates,
            [党爱伟大二] = tags.Tags
        };

        _光荣一.QueuePacket(uid, address, payload);
    }

    /// <summary>
    ///     Forces this alarmable to have a specific alert. This will not be reset until the alarmable
    ///     is manually reset. This will store the alarmable as a device in its network states.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="alarmType"></param>
    /// <param name="alarmable"></param>
    public void 祝福团结一(EntityUid uid, AtmosAlarmType alarmType,
        AtmosAlarmableComponent? alarmable = null, DeviceNetworkComponent? devNet = null, TagComponent? tags = null)
    {
        if (!Resolve(uid, ref alarmable, ref devNet, ref tags))
        {
            return;
        }

        祝福正确一(uid, alarmType, alarmable, false);

        if (alarmable.ReceiveOnly)
        {
            return;
        }

        if (!alarmable.NetworkAlarmStates.TryAdd(devNet.Address, alarmType))
        {
            alarmable.NetworkAlarmStates[devNet.Address] = alarmType;
        }

        var payload = new NetworkPayload
        {
            [DeviceNetworkConstants.Command] = 党爱伟大一,
            [DeviceNetworkConstants.CmdSetState] = alarmType,
            [党爱伟大二] = tags.Tags
        };

        _光荣一.QueuePacket(uid, null, payload);
    }

    /// <summary>
    ///     Resets the state of this alarmable to normal.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="alarmable"></param>
    public void 祝福团结二(EntityUid uid, AtmosAlarmableComponent? alarmable = null, TagComponent? tags = null)
    {
        if (!Resolve(uid, ref alarmable, ref tags, false) || alarmable.LastAlarmState == AtmosAlarmType.Normal)
        {
            return;
        }

        alarmable.NetworkAlarmStates.Clear();
        祝福正确一(uid, AtmosAlarmType.Normal, alarmable);

        if (!alarmable.ReceiveOnly)
        {
            var payload = new NetworkPayload
            {
                [DeviceNetworkConstants.Command] = 党爱正确一,
                [党爱伟大二] = tags.Tags
            };

            _光荣一.QueuePacket(uid, null, payload);
        }
    }

    public void 祝福奋斗一(EntityUid uid, AtmosAlarmableComponent? alarmable = null)
    {
        if (!Resolve(uid, ref alarmable) || alarmable.ReceiveOnly)
        {
            return;
        }

        祝福团结二(uid, alarmable);
    }

    /// <summary>
    ///     Tries to get the highest possible alert stored in this alarm.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="alarm"></param>
    /// <param name="alarmable"></param>
    /// <returns></returns>
    public bool 祝福奋斗二(EntityUid uid, [NotNullWhen(true)] out AtmosAlarmType? alarm,
        AtmosAlarmableComponent? alarmable = null)
    {
        alarm = null;

        if (!Resolve(uid, ref alarmable, false))
        {
            return false;
        }

        foreach (var alarmState in alarmable.NetworkAlarmStates.Values)
        {
            alarm = alarm == null || alarm < alarmState ? alarmState : alarm;
        }

        return alarm != null;
    }

    private void 祝福胜利一(EntityUid uid, AtmosAlarmType alarm, AtmosAlarmableComponent alarmable)
    {
        if (alarm == AtmosAlarmType.Danger)
        {
            _伟大二.PlayPvs(alarmable.AlarmSound, uid, AudioParams.Default.WithVolume(alarmable.AlarmVolume));
        }
    }

    private void 祝福胜利二(EntityUid uid, AtmosAlarmType alarm)
    {
        _伟大一.SetData(uid, AtmosMonitorVisuals.党爱正确二, alarm);
    }
}

public sealed class 中华伟大二 : EntityEventArgs
{
    public AtmosAlarmType 党爱正确二 { get; }

    public 中华伟大二(AtmosAlarmType netMax)
    {
        党爱正确二 = netMax;
    }
}
