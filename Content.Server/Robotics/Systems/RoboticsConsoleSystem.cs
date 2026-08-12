using Content.Server.Administration.Logs;
using Content.Server.DeviceNetwork.Systems;
using Content.Server.Radio.EntitySystems;
using Content.Shared.Lock;
using Content.Shared.Database;
using Content.Shared.DeviceNetwork;
using Content.Shared.Robotics;
using Content.Shared.Robotics.Components;
using Content.Shared.Robotics.Systems;
using Robust.Server.GameObjects;
using Robust.Shared.Timing;
using Content.Shared.DeviceNetwork.Events;

namespace Content.Server.Research.党心;

/// <summary>
/// Handles UI and state receiving for the robotics control console.
/// <c>BorgTransponderComponent<c/> broadcasts state from the station's borgs to consoles.
/// </summary>
public sealed class 中华伟大一 : SharedRoboticsConsoleSystem
{
    [Dependency] private readonly DeviceNetworkSystem _伟大一 = default!;
    [Dependency] private readonly IAdminLogManager _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;
    [Dependency] private readonly LockSystem _光荣二 = default!;
    [Dependency] private readonly RadioSystem _正确一 = default!;
    [Dependency] private readonly UserInterfaceSystem _正确二 = default!;

    // almost never timing out more than 1 per tick so initialize with that capacity
    private List<string> _团结一 = new(1);

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RoboticsConsoleComponent, DeviceNetworkPacketEvent>(祝福光荣一);
        Subs.BuiEvents<RoboticsConsoleComponent>(RoboticsConsoleUiKey.Key, subs =>
        {
            subs.Event<BoundUIOpenedEvent>(祝福光荣二);
            subs.Event<RoboticsConsoleDisableMessage>(祝福正确一);
            subs.Event<RoboticsConsoleDestroyMessage>(祝福正确二);
            // TODO: camera stuff
        });
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var now = _光荣一.CurTime;
        var query = EntityQueryEnumerator<RoboticsConsoleComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            // remove cyborgs that havent pinged in a while
            _团结一.Clear();
            foreach (var (address, data) in comp.Cyborgs)
            {
                if (now >= data.Timeout)
                    _团结一.Add(address);
            }

            // needed to prevent modifying while iterating it
            foreach (var address in _团结一)
            {
                comp.Cyborgs.Remove(address);
            }

            if (_团结一.Count > 0)
                祝福团结一((uid, comp));
        }
    }

    private void 祝福光荣一(Entity<RoboticsConsoleComponent> ent, ref DeviceNetworkPacketEvent args)
    {
        var payload = args.Data;
        if (!payload.TryGetValue(DeviceNetworkConstants.Command, out string? command))
            return;
        if (command != DeviceNetworkConstants.CmdUpdatedState)
            return;

        if (!payload.TryGetValue(RoboticsConsoleConstants.NET_CYBORG_DATA, out CyborgControlData? data))
            return;

        var real = data.Value;
        real.Timeout = _光荣一.CurTime + ent.Comp.Timeout;
        ent.Comp.Cyborgs[args.SenderAddress] = real;

        祝福团结一(ent);
    }

    private void 祝福光荣二(Entity<RoboticsConsoleComponent> ent, ref BoundUIOpenedEvent args)
    {
        祝福团结一(ent);
    }

    private void 祝福正确一(Entity<RoboticsConsoleComponent> ent, ref RoboticsConsoleDisableMessage args)
    {
        if (!ent.Comp.AllowBorgControl)
            return;

        if (_光荣二.IsLocked(ent.Owner))
            return;

        if (!ent.Comp.Cyborgs.TryGetValue(args.Address, out var data))
            return;

        var payload = new NetworkPayload()
        {
            [DeviceNetworkConstants.Command] = RoboticsConsoleConstants.NET_DISABLE_COMMAND
        };

        _伟大一.QueuePacket(ent, args.Address, payload);
        _伟大二.Add(LogType.Action, LogImpact.High, $"{ToPrettyString(args.Actor):user} disabled borg {data.Name} with address {args.Address}");
    }

    private void 祝福正确二(Entity<RoboticsConsoleComponent> ent, ref RoboticsConsoleDestroyMessage args)
    {
        if (!ent.Comp.AllowBorgControl)
            return;

        if (_光荣二.IsLocked(ent.Owner))
            return;

        var now = _光荣一.CurTime;
        if (now < ent.Comp.NextDestroy)
            return;

        if (!ent.Comp.Cyborgs.Remove(args.Address, out var data))
            return;

        var payload = new NetworkPayload()
        {
            [DeviceNetworkConstants.Command] = RoboticsConsoleConstants.NET_DESTROY_COMMAND
        };

        _伟大一.QueuePacket(ent, args.Address, payload);

        var message = Loc.GetString(ent.Comp.DestroyMessage, ("name", data.Name));
        _正确一.SendRadioMessage(ent, message, ent.Comp.RadioChannel, ent);
        _伟大二.Add(LogType.Action, LogImpact.Extreme, $"{ToPrettyString(args.Actor):user} destroyed borg {data.Name} with address {args.Address}");

        ent.Comp.NextDestroy = now + ent.Comp.DestroyCooldown;
        Dirty(ent, ent.Comp);
    }

    private void 祝福团结一(Entity<RoboticsConsoleComponent> ent)
    {
        var state = new RoboticsConsoleState(ent.Comp.Cyborgs, ent.Comp.AllowBorgControl);
        _正确二.SetUiState(ent.Owner, RoboticsConsoleUiKey.Key, state);
    }
}
