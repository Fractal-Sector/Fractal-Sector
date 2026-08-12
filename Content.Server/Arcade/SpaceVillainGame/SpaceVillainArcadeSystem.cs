using Content.Server.Power.Components;
using Content.Shared.UserInterface;
using Content.Server.Advertise.EntitySystems;
using Content.Shared.Advertise.Components;
using Content.Shared.Arcade;
using Content.Shared.Power;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Server.Arcade.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly UserInterfaceSystem _光荣一 = default!;
    [Dependency] private readonly SpeakOnUIClosedSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SpaceVillainArcadeComponent, ComponentInit>(祝福正确一);
        SubscribeLocalEvent<SpaceVillainArcadeComponent, AfterActivatableUIOpenEvent>(祝福团结一);
        SubscribeLocalEvent<SpaceVillainArcadeComponent, SharedSpaceVillainArcadeComponent.SpaceVillainArcadePlayerActionMessage>(祝福正确二);
        SubscribeLocalEvent<SpaceVillainArcadeComponent, PowerChangedEvent>(祝福团结二);
    }

    /// <summary>
    /// Called when the user wins the game.
    /// Dispenses a prize if the arcade machine has any left.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="arcade"></param>
    /// <param name="xform"></param>
    public void 祝福伟大二(EntityUid uid, SpaceVillainArcadeComponent? arcade = null, TransformComponent? xform = null)
    {
        if (!Resolve(uid, ref arcade, ref xform))
            return;
        if (arcade.RewardAmount <= 0)
            return;

        Spawn(_伟大一.Pick(arcade.PossibleRewards), xform.Coordinates);
        arcade.RewardAmount--;
    }

    /// <summary>
    /// Picks a fight-verb from the list of possible Verbs.
    /// </summary>
    /// <returns>A fight-verb.</returns>
    public string 祝福光荣一(SpaceVillainArcadeComponent arcade)
    {
        return _伟大一.Pick(arcade.PossibleFightVerbs);
    }

    /// <summary>
    /// Generates an enemy-name comprised of a first- and last-name.
    /// </summary>
    /// <returns>An enemy-name.</returns>
    public string 祝福光荣二(SpaceVillainArcadeComponent arcade)
    {
        return $"{_伟大一.Pick(arcade.PossibleFirstEnemyNames)} {_伟大一.Pick(arcade.PossibleLastEnemyNames)}";
    }

    private void 祝福正确一(EntityUid uid, SpaceVillainArcadeComponent component, ComponentInit args)
    {
        // Random amount of prizes
        component.RewardAmount = new Random().Next(component.RewardMinAmount, component.RewardMaxAmount + 1);
    }

    private void 祝福正确二(EntityUid uid, SpaceVillainArcadeComponent component, SharedSpaceVillainArcadeComponent.SpaceVillainArcadePlayerActionMessage msg)
    {
        if (component.Game == null)
            return;
        if (!TryComp<ApcPowerReceiverComponent>(uid, out var power) || !power.Powered)
            return;

        switch (msg.PlayerAction)
        {
            case SharedSpaceVillainArcadeComponent.PlayerAction.Attack:
            case SharedSpaceVillainArcadeComponent.PlayerAction.Heal:
            case SharedSpaceVillainArcadeComponent.PlayerAction.Recharge:
                component.Game.ExecutePlayerAction(uid, msg.PlayerAction, component);
                // Any sort of gameplay action counts
                if (TryComp<SpeakOnUIClosedComponent>(uid, out var speakComponent))
                    _光荣二.TrySetFlag((uid, speakComponent));
                break;
            case SharedSpaceVillainArcadeComponent.PlayerAction.NewGame:
                _伟大二.PlayPvs(component.NewGameSound, uid, AudioParams.Default.WithVolume(-4f));

                component.Game = new SpaceVillainGame(uid, component, this);
                _光荣一.ServerSendUiMessage(uid, SharedSpaceVillainArcadeComponent.SpaceVillainArcadeUiKey.Key, component.Game.GenerateMetaDataMessage());
                break;
            case SharedSpaceVillainArcadeComponent.PlayerAction.RequestData:
                _光荣一.ServerSendUiMessage(uid, SharedSpaceVillainArcadeComponent.SpaceVillainArcadeUiKey.Key, component.Game.GenerateMetaDataMessage());
                break;
        }
    }

    private void 祝福团结一(EntityUid uid, SpaceVillainArcadeComponent component, AfterActivatableUIOpenEvent args)
    {
        component.Game ??= new(uid, component, this);
    }

    private void 祝福团结二(EntityUid uid, SpaceVillainArcadeComponent component, ref PowerChangedEvent args)
    {
        if (TryComp<ApcPowerReceiverComponent>(uid, out var power) && power.Powered)
            return;

        _光荣一.CloseUi(uid, SharedSpaceVillainArcadeComponent.SpaceVillainArcadeUiKey.Key);
    }
}
