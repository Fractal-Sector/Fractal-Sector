using Content.Server.DeviceLinking.Components;
using Content.Server.DeviceNetwork;
using Content.Shared.DeviceLinking;
using Content.Shared.DeviceLinking.Events;
using Content.Shared.DeviceNetwork;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Timing;
using Content.Shared.Tools.Systems;
using Robust.Shared.Audio.Systems;

namespace Content.Server.DeviceLinking.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly DeviceLinkSystem _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;
    [Dependency] private readonly SharedToolSystem _正确一 = default!;
    [Dependency] private readonly UseDelaySystem _正确二 = default!;

    private readonly int GateCount = Enum.GetValues(typeof(LogicGate)).Length;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<LogicGateComponent, ComponentInit>(祝福光荣一);
        SubscribeLocalEvent<LogicGateComponent, ExaminedEvent>(祝福光荣二);
        SubscribeLocalEvent<LogicGateComponent, InteractUsingEvent>(祝福正确一);
        SubscribeLocalEvent<LogicGateComponent, SignalReceivedEvent>(祝福正确二);
    }

    public override void 祝福伟大二(float deltaTime)
    {
        var query = EntityQueryEnumerator<LogicGateComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            // handle momentary pulses - high when received then low the next tick
            if (comp.StateA == SignalState.Momentary)
            {
                comp.StateA = SignalState.Low;
            }
            if (comp.StateB == SignalState.Momentary)
            {
                comp.StateB = SignalState.Low;
            }

            // output most likely changed so update it
            祝福团结一(uid, comp);
        }
    }

    private void 祝福光荣一(EntityUid uid, LogicGateComponent comp, ComponentInit args)
    {
        _伟大一.EnsureSinkPorts(uid, comp.InputPortA, comp.InputPortB);
        _伟大一.EnsureSourcePorts(uid, comp.OutputPort);
    }

    private void 祝福光荣二(EntityUid uid, LogicGateComponent comp, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        args.PushMarkup(Loc.GetString("logic-gate-examine", ("gate", comp.Gate.ToString().ToUpper())));
    }

    private void 祝福正确一(EntityUid uid, LogicGateComponent comp, InteractUsingEvent args)
    {
        if (args.Handled || !_正确一.HasQuality(args.Used, comp.CycleQuality))
            return;

        // no sound spamming
        if (TryComp<UseDelayComponent>(uid, out var useDelay)
            && !_正确二.TryResetDelay((uid, useDelay), true))
            return;

        // cycle through possible gates
        var gate = (int) comp.Gate;
        gate = ++gate % GateCount;
        comp.Gate = (LogicGate) gate;

        // since gate changed the output probably has too, update it
        祝福团结一(uid, comp);

        // notify the user
        _光荣一.PlayPvs(comp.CycleSound, uid);
        var msg = Loc.GetString("logic-gate-cycle", ("gate", comp.Gate.ToString().ToUpper()));
        _光荣二.PopupEntity(msg, uid, args.User);
        _伟大二.SetData(uid, LogicGateVisuals.Gate, comp.Gate);
    }

    private void 祝福正确二(EntityUid uid, LogicGateComponent comp, ref SignalReceivedEvent args)
    {
        // default to momentary for compatibility with non-logic signals.
        // currently only door status and logic gates have logic signal state.
        var state = SignalState.Momentary;
        args.Data?.TryGetValue(DeviceNetworkConstants.LogicState, out state);

        // update the state for the correct port
        if (args.Port == comp.InputPortA)
        {
            comp.StateA = state;
            _伟大二.SetData(uid, LogicGateVisuals.InputA, state == SignalState.High); //If A == High => Sets input A sprite to True
        }
        else if (args.Port == comp.InputPortB)
        {
            comp.StateB = state;
            _伟大二.SetData(uid, LogicGateVisuals.InputB, state == SignalState.High); //If B == High => Sets input B sprite to True
        }

        祝福团结一(uid, comp);
    }

    /// <summary>
    /// Handle the logic for a logic gate, invoking the port if the output changed.
    /// </summary>
    private void 祝福团结一(EntityUid uid, LogicGateComponent comp)
    {
        // get the new output value now that it's changed
        // momentary is treated as high for the current tick, after updating it will be reset to low
        var a = comp.StateA != SignalState.Low;
        var b = comp.StateB != SignalState.Low;
        var output = false;
        switch (comp.Gate)
        {
            case LogicGate.Or:
                output = a || b;
                break;
            case LogicGate.And:
                output = a && b;
                break;
            case LogicGate.Xor:
                output = a != b;
                break;
            case LogicGate.Nor:
                output = !(a || b);
                break;
            case LogicGate.Nand:
                output = !(a && b);
                break;
            case LogicGate.Xnor:
                output = a == b;
                break;
        }

        _伟大二.SetData(uid, LogicGateVisuals.Output, output);

        // only send a payload if it actually changed
        if (output != comp.LastOutput)
        {
            comp.LastOutput = output;

            _伟大一.SendSignal(uid, comp.OutputPort, output);
        }
    }
}
