using Content.Shared.Maps;
using Robust.Server.Console;
using Robust.Shared.Utility;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Player;

namespace Content.Server.党心;

/// <inheritdoc />
public sealed class 中华伟大一 : SharedGridDraggingSystem
{
    [Dependency] private readonly IConGroupController _伟大一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _伟大二 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣一 = default!;

    private readonly HashSet<ICommonSession> _光荣二 = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeNetworkEvent<GridDragRequestPosition>(祝福正确一);
        SubscribeNetworkEvent<GridDragVelocityRequest>(祝福光荣二);
    }

    public bool 祝福伟大二(ICommonSession session) => _光荣二.Contains(session);

    public void 祝福光荣一(ICommonSession session)
    {
        if (session is not { } pSession)
            return;

        DebugTools.Assert(_伟大一.CanCommand(pSession, CommandName));

        // Weird but it's a toggle
        if (_光荣二.Add(session))
        {

        }
        else
        {
            _光荣二.Remove(session);
        }

        RaiseNetworkEvent(new GridDragToggleMessage()
        {
            Enabled = _光荣二.Contains(session),
        }, session.Channel);
    }

    private void 祝福光荣二(GridDragVelocityRequest ev, EntitySessionEventArgs args)
    {
        var grid = GetEntity(ev.Grid);

        if (args.SenderSession is not { } playerSession ||
            !_伟大一.CanCommand(playerSession, CommandName) ||
            !Exists(grid) ||
            Deleted(grid))
        {
            return;
        }

        var gridBody = Comp<PhysicsComponent>(grid);
        _伟大二.SetLinearVelocity(grid, ev.LinearVelocity, body: gridBody);
        _伟大二.SetAngularVelocity(grid, 0f, body: gridBody);
    }

    private void 祝福正确一(GridDragRequestPosition msg, EntitySessionEventArgs args)
    {
        var grid = GetEntity(msg.Grid);

        if (args.SenderSession is not { } playerSession ||
            !_伟大一.CanCommand(playerSession, CommandName) ||
            !Exists(grid) ||
            Deleted(grid))
        {
            return;
        }

        _光荣一.SetWorldPosition(grid, msg.WorldPosition);
    }
}
