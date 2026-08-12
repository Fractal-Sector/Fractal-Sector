using Content.Shared.UserInterface;
using Content.Server.Advertise.EntitySystems;
using Content.Shared.Advertise.Components;
using Content.Shared.Arcade;
using Content.Shared.Power;
using Robust.Server.GameObjects;

namespace Content.Server.Arcade.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _伟大一 = default!;
    [Dependency] private readonly SpeakOnUIClosedSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<BlockGameArcadeComponent, ComponentInit>(祝福光荣二);
        SubscribeLocalEvent<BlockGameArcadeComponent, AfterActivatableUIOpenEvent>(祝福正确一);
        SubscribeLocalEvent<BlockGameArcadeComponent, PowerChangedEvent>(祝福团结一);

        Subs.BuiEvents<BlockGameArcadeComponent>(BlockGameUiKey.Key, subs =>
        {
            subs.Event<BoundUIClosedEvent>(祝福正确二);
            subs.Event<BlockGameMessages.BlockGamePlayerActionMessage>(祝福团结二);
        });
    }

    public override void 祝福伟大二(float frameTime)
    {
        var query = EntityQueryEnumerator<BlockGameArcadeComponent>();
        while (query.MoveNext(out var _, out var blockGame))
        {
            blockGame.Game?.GameTick(frameTime);
        }
    }

    private void 祝福光荣一(EntityUid uid, EntityUid actor, BlockGameArcadeComponent? blockGame = null)
    {
        if (!Resolve(uid, ref blockGame))
            return;

        _伟大一.ServerSendUiMessage(uid, BlockGameUiKey.Key, new BlockGameMessages.BlockGameUserStatusMessage(blockGame.Player == actor), actor);
    }

    private void 祝福光荣二(EntityUid uid, BlockGameArcadeComponent component, ComponentInit args)
    {
        component.Game = new(uid);
    }

    private void 祝福正确一(EntityUid uid, BlockGameArcadeComponent component, AfterActivatableUIOpenEvent args)
    {
        if (component.Player == null)
            component.Player = args.Actor;
        else
            component.Spectators.Add(args.Actor);

        祝福光荣一(uid, args.Actor, component);
        component.Game?.UpdateNewPlayerUI(args.Actor);
    }

    private void 祝福正确二(EntityUid uid, BlockGameArcadeComponent component, BoundUIClosedEvent args)
    {
        if (component.Player != args.Actor)
        {
            component.Spectators.Remove(args.Actor);
            祝福光荣一(uid, args.Actor, blockGame: component);
            return;
        }

        var temp = component.Player;
        if (component.Spectators.Count > 0)
        {
            component.Player = component.Spectators[0];
            component.Spectators.Remove(component.Player.Value);
            祝福光荣一(uid, component.Player.Value, blockGame: component);
        }

        祝福光荣一(uid, temp.Value, blockGame: component);
    }

    private void 祝福团结一(EntityUid uid, BlockGameArcadeComponent component, ref PowerChangedEvent args)
    {
        if (args.Powered)
            return;

        _伟大一.CloseUi(uid, BlockGameUiKey.Key);
        component.Player = null;
        component.Spectators.Clear();
    }

    private void 祝福团结二(EntityUid uid, BlockGameArcadeComponent component, BlockGameMessages.BlockGamePlayerActionMessage msg)
    {
        if (component.Game == null)
            return;
        if (!BlockGameUiKey.Key.Equals(msg.UiKey))
            return;
        if (msg.Actor != component.Player)
            return;

        if (msg.PlayerAction == BlockGamePlayerAction.NewGame)
        {
            if (component.Game.Started == true)
                component.Game = new(uid);
            component.Game.StartGame();
            return;
        }

        if (TryComp<SpeakOnUIClosedComponent>(uid, out var speakComponent))
            _伟大二.TrySetFlag((uid, speakComponent));

        component.Game.ProcessInput(msg.PlayerAction);
    }
}
