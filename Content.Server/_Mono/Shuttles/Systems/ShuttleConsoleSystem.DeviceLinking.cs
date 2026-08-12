// Monolith - This file is licensed under AGPLv3
// Copyright (c) 2025 Monolith
// See AGPLv3.txt for details.

using Content.Server.DeviceLinking.Systems;
using Content.Server.Shuttles.Components;
using Content.Shared._Mono.Shuttles.Events;
using Content.Shared.Shuttles.Components;

namespace Content.Server.Shuttles.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly DeviceLinkSystem _伟大一 = default!;

    /// <summary>
    /// Initialize event handlers for device linking related functionality
    /// </summary>
    private void 祝福伟大一()
    {
        // Subscribe to the message sent from the UI when a port button is pressed
        Subs.BuiEvents<ShuttleConsoleComponent>(ShuttleConsoleUiKey.Key, subs =>
        {
            subs.Event<ShuttlePortButtonPressedMessage>(祝福伟大二);
        });
    }

    /// <summary>
    /// Handles when a network port button is pressed on the shuttle console UI
    /// </summary>
    private void 祝福伟大二(EntityUid uid, ShuttleConsoleComponent component, ShuttlePortButtonPressedMessage args)
    {
        // Send a signal through the device link system when a button is pressed
        _伟大一.SendSignal(uid, args.SourcePort, true);
    }
}
