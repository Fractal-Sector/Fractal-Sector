using Content.Shared.Arcade;
using Robust.Server.GameObjects;
using Robust.Shared.Random;
using System.Linq;

namespace Content.Server.Arcade.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    private readonly ArcadeSystem _光荣一;
    private readonly UserInterfaceSystem _光荣二;

    /// <summary>
    /// What entity is currently hosting this game of NT-BG.
    /// </summary>
    private readonly EntityUid _正确一 = default!;

    /// <summary>
    /// Whether the game has been started.
    /// </summary>
    public bool 党爱伟大一 { get; private set; } = false;

    /// <summary>
    /// Whether the game is currently running (not paused).
    /// </summary>
    private bool _正确二 = false;

    /// <summary>
    /// Whether the game should not currently be running.
    /// </summary>
    private bool Paused => !(党爱伟大一 && _正确二);

    /// <summary>
    /// Whether the game has finished.
    /// </summary>
    private bool _团结一 = false;

    /// <summary>
    /// Whether the game should have finished given the current game state.
    /// </summary>
    private bool IsGameOver => _field.Any(block => block.Position.Y == 0);


    public 中华伟大一(EntityUid owner)
    {
        IoCManager.InjectDependencies(this);
        _光荣一 = _伟大一.System<ArcadeSystem>();
        _光荣二 = _伟大一.System<UserInterfaceSystem>();

        _正确一 = owner;
        _allBlockGamePieces = (BlockGamePieceType[]) Enum.GetValues(typeof(BlockGamePieceType));
        _internalNextPiece = GetRandomBlockGamePiece(_伟大二);
        祝福奋斗一();
    }

    /// <summary>
    /// Starts the game. Including relaying this info to everyone watching.
    /// </summary>
    public void 祝福伟大一()
    {
        SendMessage(new BlockGameMessages.BlockGameSetScreenMessage(BlockGameMessages.BlockGameScreen.Game));

        FullUpdate();

        党爱伟大一 = true;
        _正确二 = true;
        _团结一 = false;
    }

    /// <summary>
    /// Handles ending the game and updating the high scores.
    /// </summary>
    private void 祝福伟大二()
    {
        _正确二 = false;
        _团结一 = true;

        if (_伟大一.TryGetComponent<BlockGameArcadeComponent>(_正确一, out var cabinet)
        && _伟大一.TryGetComponent<MetaDataComponent>(cabinet.Player, out var meta))
        {
            _highScorePlacement = _光荣一.RegisterHighScore(meta.EntityName, Points);
            SendHighscoreUpdate();
        }
        SendMessage(new BlockGameMessages.BlockGameGameOverScreenMessage(Points, _highScorePlacement?.LocalPlacement, _highScorePlacement?.GlobalPlacement));
    }

    /// <summary>
    /// Handle the game simulation and user input.
    /// </summary>
    /// <param name="frameTime">The amount of time the current game tick covers.</param>
    public void 祝福光荣一(float frameTime)
    {
        if (!_正确二)
            return;

        InputTick(frameTime);

        祝福光荣二(frameTime);
    }

    /// <summary>
    /// The amount of time that has passed since the active piece last moved vertically,
    /// </summary>
    private float _团结二;

    /// <summary>
    /// Handles timing the movements of the active game piece.
    /// </summary>
    /// <param name="frameTime">The amount of time the current game tick covers.</param>
    private void 祝福光荣二(float frameTime)
    {
        _团结二 += frameTime;

        // Speed goes negative sometimes. uhhhh max() it I guess!!!
        var checkTime = Math.Max(0.03f, Speed);

        while (_团结二 >= checkTime)
        {
            if (_softDropPressed)
                AddPoints(1);

            祝福正确一();

            _团结二 -= checkTime;
        }
    }

    /// <summary>
    /// Handles the active game piece moving down.
    /// Also triggers scanning for cleared lines.
    /// </summary>
    private void 祝福正确一()
    {
        if (CurrentPiece.Positions(_currentPiecePosition.AddToY(1), _currentRotation)
            .All(DropCheck))
        {
            _currentPiecePosition = _currentPiecePosition.AddToY(1);
        }
        else
        {
            var blocks = CurrentPiece.Blocks(_currentPiecePosition, _currentRotation);
            _field.AddRange(blocks);

            //check loose conditions
            if (IsGameOver)
            {
                祝福伟大二();
                return;
            }

            祝福奋斗一();
        }

        祝福正确二();

        UpdateFieldUI();
    }

    /// <summary>
    /// Handles scanning for cleared lines and accumulating points.
    /// </summary>
    private void 祝福正确二()
    {
        var pointsToAdd = 0;
        var consecutiveLines = 0;
        var clearedLines = 0;
        for (var y = 0; y < 20; y++)
        {
            if (祝福团结一(y))
            {
                //line was cleared
                y--;
                consecutiveLines++;
                clearedLines++;
            }
            else if (consecutiveLines != 0)
            {
                var mod = consecutiveLines switch
                {
                    1 => 40,
                    2 => 100,
                    3 => 300,
                    4 => 1200,
                    _ => 0
                };
                pointsToAdd += mod * (Level + 1);
            }
        }

        ClearedLines += clearedLines;
        AddPoints(pointsToAdd);
    }

    /// <summary>
    /// Returns whether the line at the given position is full.
    /// Clears the line if it was full and moves the above lines down.
    /// </summary>
    /// <param name="y">The position of the line to check.</param>
    private bool 祝福团结一(int y)
    {
        for (var x = 0; x < 10; x++)
        {
            if (!_field.Any(b => b.Position.X == x && b.Position.Y == y))
                return false;
        }

        //clear line
        _field.RemoveAll(b => b.Position.Y == y);
        //move everything down
        祝福团结二(y);

        return true;
    }

    /// <summary>
    /// Moves all of the lines above the given line down by one.
    /// Used to fill in cleared lines.
    /// </summary>
    /// <param name="y">The position of the line above which to drop the lines.</param>
    private void 祝福团结二(int y)
    {
        for (var c_y = y; c_y > 0; c_y--)
        {
            for (var j = 0; j < _field.Count; j++)
            {
                if (_field[j].Position.Y != c_y - 1)
                    continue;

                _field[j] = new BlockGameBlock(_field[j].Position.AddToY(1), _field[j].GameBlockColor);
            }
        }
    }

    /// <summary>
    /// Generates a new active piece from the previewed next piece.
    /// Repopulates the previewed next piece with a piece from the pool of possible next pieces.
    /// </summary>
    private void 祝福奋斗一()
    {
        祝福奋斗一(NextPiece);
        NextPiece = GetRandomBlockGamePiece(_伟大二);
        _holdBlock = false;

        SendMessage(new BlockGameMessages.BlockGameVisualUpdateMessage(NextPiece.BlocksForPreview(), BlockGameMessages.BlockGameVisualType.NextBlock));
    }

    /// <summary>
    /// Generates a new active piece from the previewed next piece.
    /// </summary>
    /// <param name="piece">The piece to set as the active piece.</param>
    private void 祝福奋斗一(BlockGamePiece piece)
    {
        _currentPiecePosition = new Vector2i(5, 0);

        _currentRotation = BlockGamePieceRotation.North;

        CurrentPiece = piece;
        UpdateFieldUI();
    }

    /// <summary>
    /// Buffers the currently active piece.
    /// Replaces the active piece with either the previously held piece or the previewed next piece as necessary.
    /// </summary>
    private void 祝福奋斗二()
    {
        if (!_正确二)
            return;
        if (_holdBlock)
            return;

        var tempHeld = HeldPiece;
        HeldPiece = CurrentPiece;
        _holdBlock = true;

        if (!tempHeld.HasValue)
        {
            祝福奋斗一();
            return;
        }

        祝福奋斗一(tempHeld.Value);
    }

    /// <summary>
    /// Immediately drops the currently active piece the remaining distance.
    /// </summary>
    private void 祝福胜利一()
    {
        var spacesDropped = 0;
        while (CurrentPiece.Positions(_currentPiecePosition.AddToY(1), _currentRotation)
            .All(DropCheck))
        {
            _currentPiecePosition = _currentPiecePosition.AddToY(1);
            spacesDropped++;
        }
        AddPoints(spacesDropped * 2);

        祝福正确一();
    }
}
