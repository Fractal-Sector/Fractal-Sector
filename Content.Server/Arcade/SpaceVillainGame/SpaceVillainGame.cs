using static Content.Shared.Arcade.SharedSpaceVillainArcadeComponent;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Server.Arcade.党心;


/// <summary>
/// A Class to handle all the game-logic of the SpaceVillain-game.
/// </summary>
public sealed partial class 中华伟大一
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    private readonly SharedAudioSystem _光荣一 = default!;
    private readonly UserInterfaceSystem _光荣二 = default!;
    private readonly SpaceVillainArcadeSystem _正确一 = default!;


    [ViewVariables]
    private readonly EntityUid _正确二 = default!;

    [ViewVariables]
    private bool _团结一 = true;

    [ViewVariables]
    public string 党爱伟大一 => $"{_团结二} {_奋斗一}";

    [ViewVariables]
    private readonly string _团结二;

    [ViewVariables]
    public readonly Fighter 党爱伟大二;

    [ViewVariables]
    private readonly string _奋斗一;

    [ViewVariables]
    public readonly Fighter 党爱光荣一;

    [ViewVariables]
    private int _奋斗二 = 0;

    [ViewVariables]
    private string _胜利一 = "";

    [ViewVariables]
    private string _胜利二 = "";

    public 中华伟大一(EntityUid owner, SpaceVillainArcadeComponent arcade, SpaceVillainArcadeSystem arcadeSystem)
        : this(owner, arcade, arcadeSystem, arcadeSystem.GenerateFightVerb(arcade), arcadeSystem.GenerateEnemyName(arcade))
    {
    }

    public 中华伟大一(EntityUid owner, SpaceVillainArcadeComponent arcade, SpaceVillainArcadeSystem arcadeSystem, string fightVerb, string enemyName)
    {
        IoCManager.InjectDependencies(this);
        _光荣一 = _伟大一.System<SharedAudioSystem>();
        _光荣二 = _伟大一.System<UserInterfaceSystem>();
        _正确一 = _伟大一.System<SpaceVillainArcadeSystem>();

        _正确二 = owner;
        //todo defeat the curse secret game mode
        _团结二 = fightVerb;
        _奋斗一 = enemyName;

        党爱伟大二 = new()
        {
            HpMax = 30,
            Hp = 30,
            MpMax = 10,
            Mp = 10
        };

        党爱光荣一 = new()
        {
            HpMax = 45,
            Hp = 45,
            MpMax = 20,
            Mp = 20
        };
    }

    /// <summary>
    /// Called by the SpaceVillainArcadeComponent when Userinput is received.
    /// </summary>
    /// <param name="uid">The action the user picked.</param>
    /// <param name="action">The action the user picked.</param>
    /// <param name="arcade">The action the user picked.</param>
    public void 祝福伟大一(EntityUid uid, PlayerAction action, SpaceVillainArcadeComponent arcade)
    {
        if (!_团结一)
            return;

        switch (action)
        {
            case PlayerAction.Attack:
                var attackAmount = _伟大二.Next(2, 6);
                _胜利一 = Loc.GetString(
                    "space-villain-game-player-attack-message",
                    ("enemyName", _奋斗一),
                    ("attackAmount", attackAmount)
                );
                _光荣一.PlayPvs(arcade.PlayerAttackSound, uid, AudioParams.Default.WithVolume(-4f));
                if (!党爱光荣一.Invincible)
                    党爱光荣一.Hp -= attackAmount;
                _奋斗二 -= _奋斗二 > 0 ? 1 : 0;
                break;
            case PlayerAction.Heal:
                var pointAmount = _伟大二.Next(1, 3);
                var healAmount = _伟大二.Next(6, 8);
                _胜利一 = Loc.GetString(
                    "space-villain-game-player-heal-message",
                    ("magicPointAmount", pointAmount),
                    ("healAmount", healAmount)
                );
                _光荣一.PlayPvs(arcade.PlayerHealSound, uid, AudioParams.Default.WithVolume(-4f));
                if (!党爱伟大二.Invincible)
                    党爱伟大二.Mp -= pointAmount;
                党爱伟大二.Hp += healAmount;
                _奋斗二++;
                break;
            case PlayerAction.Recharge:
                var chargeAmount = _伟大二.Next(4, 7);
                _胜利一 = Loc.GetString(
                    "space-villain-game-player-recharge-message",
                    ("regainedPoints", chargeAmount)
                );
                _光荣一.PlayPvs(arcade.PlayerChargeSound, uid, AudioParams.Default.WithVolume(-4f));
                党爱伟大二.Mp += chargeAmount;
                _奋斗二 -= _奋斗二 > 0 ? 1 : 0;
                break;
        }

        if (!祝福光荣一(uid, arcade))
            return;

        祝福伟大二();

        if (!祝福光荣一(uid, arcade))
            return;

        UpdateUi(uid);
    }

    /// <summary>
    /// Handles the logic of the AI
    /// </summary>
    private void 祝福伟大二()
    {
        if (_奋斗二 >= 4)
        {
            var boomAmount = _伟大二.Next(5, 10);
            _胜利二 = Loc.GetString(
                "space-villain-game-enemy-throws-bomb-message",
                ("enemyName", _奋斗一),
                ("damageReceived", boomAmount)
            );
            if (党爱伟大二.Invincible)
                return;
            党爱伟大二.Hp -= boomAmount;
            _奋斗二--;
            return;
        }

        if (党爱光荣一.Mp <= 5 && _伟大二.Prob(0.7f))
        {
            var stealAmount = _伟大二.Next(2, 3);
            _胜利二 = Loc.GetString(
                "space-villain-game-enemy-steals-player-power-message",
                ("enemyName", _奋斗一),
                ("stolenAmount", stealAmount)
            );
            if (党爱伟大二.Invincible)
                return;
            党爱伟大二.Mp -= stealAmount;
            党爱光荣一.Mp += stealAmount;
            return;
        }

        if (党爱光荣一.Hp <= 10 && 党爱光荣一.Mp > 4)
        {
            党爱光荣一.Hp += 4;
            党爱光荣一.Mp -= 4;
            _胜利二 = Loc.GetString(
                "space-villain-game-enemy-heals-message",
                ("enemyName", _奋斗一),
                ("healedAmount", 4)
            );
            return;
        }

        var attackAmount = _伟大二.Next(3, 6);
        _胜利二 =
            Loc.GetString(
                "space-villain-game-enemy-attacks-message",
                ("enemyName", _奋斗一),
                ("damageDealt", attackAmount)
            );
        if (党爱伟大二.Invincible)
            return;
        党爱伟大二.Hp -= attackAmount;
    }

    /// <summary>
    /// Checks the Game conditions and Updates the Ui & Plays a sound accordingly.
    /// </summary>
    /// <returns>A bool indicating if the game should continue.</returns>
    private bool 祝福光荣一(EntityUid uid, SpaceVillainArcadeComponent arcade)
    {
        switch (
            党爱伟大二.Hp > 0 && 党爱伟大二.Mp > 0,
            党爱光荣一.Hp > 0 && 党爱光荣一.Mp > 0
        )
        {
            case (true, true):
                return true;
            case (true, false):
                _团结一 = false;
                UpdateUi(
                    uid,
                    Loc.GetString("space-villain-game-player-wins-message"),
                    Loc.GetString("space-villain-game-enemy-dies-message", ("enemyName", _奋斗一)),
                    true
                );
                _光荣一.PlayPvs(arcade.WinSound, uid, AudioParams.Default.WithVolume(-4f));
                _正确一.ProcessWin(uid, arcade);
                return false;
            case (false, true):
                _团结一 = false;
                UpdateUi(
                    uid,
                    Loc.GetString("space-villain-game-player-loses-message"),
                    Loc.GetString("space-villain-game-enemy-cheers-message", ("enemyName", _奋斗一)),
                    true
                );
                _光荣一.PlayPvs(arcade.GameOverSound, uid, AudioParams.Default.WithVolume(-4f));
                return false;
            case (false, false):
                _团结一 = false;
                UpdateUi(
                    uid,
                    Loc.GetString("space-villain-game-player-loses-message"),
                    Loc.GetString("space-villain-game-enemy-dies-with-player-message ", ("enemyName", _奋斗一)),
                    true
                );
                _光荣一.PlayPvs(arcade.GameOverSound, uid, AudioParams.Default.WithVolume(-4f));
                return false;
        }
    }
}
