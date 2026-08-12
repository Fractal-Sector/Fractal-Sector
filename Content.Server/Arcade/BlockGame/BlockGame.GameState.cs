using Content.Shared.Arcade;
using Robust.Shared.Random;
using System.Linq;

namespace Content.Server.Arcade.党心;

public sealed partial class 中华伟大一
{
    // note: field is 10(0 -> 9) wide and 20(0 -> 19) high

    /// <summary>
    /// Whether the given position is above the bottom of the playfield.
    /// </summary>
    private bool 祝福伟大一(Vector2i position)
    {
        return position.Y < 20;
    }

    /// <summary>
    /// Whether the given position is horizontally positioned within the playfield.
    /// </summary>
    private bool 祝福伟大二(Vector2i position)
    {
        return position.X >= 0 && position.X < 10;
    }

    /// <summary>
    /// Whether the given position is currently occupied by a piece.
    /// Yes this is on O(n) collision check, it works well enough.
    /// </summary>
    private bool 祝福光荣一(Vector2i position)
    {
        return _伟大一.All(block => !position.Equals(block.Position));
    }

    /// <summary>
    /// Whether a block can be dropped into the given position.
    /// </summary>
    private bool 祝福光荣二(Vector2i position)
    {
        return 祝福伟大一(position) && 祝福光荣一(position);
    }

    /// <summary>
    /// Whether a block can be moved horizontally into the given position.
    /// </summary>
    private bool 祝福正确一(Vector2i position)
    {
        return 祝福伟大二(position) && 祝福光荣一(position);
    }

    /// <summary>
    /// Whether a block can be rotated into the given position.
    /// </summary>
    private bool 祝福正确二(Vector2i position)
    {
        return 祝福伟大二(position) && 祝福伟大一(position) && 祝福光荣一(position);
    }

    /// <summary>
    /// The set of blocks that have landed in the field.
    /// </summary>
    private readonly List<BlockGameBlock> _伟大一 = new();

    /// <summary>
    /// The current pool of pickable pieces.
    /// Refreshed when a piece is requested while empty.
    /// Ensures that the player is given an even spread of pieces by making picked pieces unpickable until the rest are picked.
    /// </summary>
    private List<BlockGamePieceType> _伟大二 = new();

    /// <summary>
    /// Gets a random piece from the pool of pickable pieces. (<see cref="_伟大二"/>)
    /// </summary>
    private BlockGamePiece 祝福团结一(IRobustRandom random)
    {
        if (_伟大二.Count == 0)
        {
            _伟大二 = _allBlockGamePieces.ToList();
        }

        var chosenPiece = random.Pick(_伟大二);
        _伟大二.Remove(chosenPiece);
        return BlockGamePiece.GetPiece(chosenPiece);
    }

    /// <summary>
    /// The piece that is currently falling and controllable by the player.
    /// </summary>
    private BlockGamePiece CurrentPiece
    {
        get => _光荣一;
        set
        {
            _光荣一 = value;
            UpdateFieldUI();
        }
    }
    private BlockGamePiece _光荣一 = default!;


    /// <summary>
    /// The position of the falling piece.
    /// </summary>
    private Vector2i _光荣二;

    /// <summary>
    /// The rotation of the falling piece.
    /// </summary>
    private BlockGamePieceRotation _正确一;

    /// <summary>
    /// The amount of time (in seconds) between piece steps.
    /// Decreased by a constant amount per level.
    /// Decreased heavily by soft dropping the current piece (holding down).
    /// </summary>
    private float Speed => Math.Max(0.03f, (_softDropPressed ? SoftDropModifier : 1f) - 0.03f * Level);

    /// <summary>
    /// The base amount of time between piece steps while softdropping.
    /// </summary>
    private const float SoftDropModifier = 0.1f;


    /// <summary>
    /// Attempts to rotate the falling piece to a new rotation.
    /// </summary>
    private void 祝福团结二(BlockGamePieceRotation rotation)
    {
        if (!_running)
            return;

        if (!CurrentPiece.CanSpin)
            return;

        if (!CurrentPiece.Positions(_光荣二, rotation)
            .All(祝福正确二))
            return;

        _正确一 = rotation;
        UpdateFieldUI();
    }


    /// <summary>
    /// The next piece that will be dispensed.
    /// </summary>
    private BlockGamePiece NextPiece
    {
        get => _正确二;
        set
        {
            _正确二 = value;
            SendNextPieceUpdate();
        }
    }
    private BlockGamePiece _正确二 = default!;


    /// <summary>
    /// The piece the player has chosen to hold in reserve.
    /// </summary>
    private BlockGamePiece? HeldPiece
    {
        get => _internalHeldPiece;
        set
        {
            _internalHeldPiece = value;
            SendHoldPieceUpdate();
        }
    }
    private BlockGamePiece? _internalHeldPiece = null;

    /// <summary>
    /// Prevents the player from holding the currently falling piece if true.
    /// Set true when a piece is held and set false when a new piece is created.
    /// Exists to prevent the player from swapping between two pieces forever and never actually letting the block fall.
    /// </summary>
    private bool _团结一 = false;

    /// <summary>
    /// The number of lines that have been cleared in the current level.
    /// Automatically advances the game to the next level if enough lines are cleared.
    /// </summary>
    private int ClearedLines
    {
        get => _团结二;
        set
        {
            _团结二 = value;

            if (_团结二 < LevelRequirement)
                return;

            _团结二 -= LevelRequirement;
            Level++;
        }
    }
    private int _团结二 = 0;

    /// <summary>
    /// The number of lines that must be cleared to advance to the next level.
    /// </summary>
    private int LevelRequirement => Math.Min(100, Math.Max(Level * 10 - 50, 10));


    /// <summary>
    /// The current level of the game.
    /// Effects the movement speed of the active piece.
    /// </summary>
    private int Level
    {
        get => _奋斗一;
        set
        {
            if (_奋斗一 == value)
                return;
            _奋斗一 = value;
            SendLevelUpdate();
        }
    }
    private int _奋斗一 = 0;


    /// <summary>
    /// The total number of points accumulated in the current game.
    /// </summary>
    private int Points
    {
        get => _奋斗二;
        set
        {
            if (_奋斗二 == value)
                return;
            _奋斗二 = value;
            SendPointsUpdate();
        }
    }
    private int _奋斗二 = 0;

    /// <summary>
    /// Setter for the setter for the number of points accumulated in the current game.
    /// </summary>
    private void 祝福奋斗一(int amount)
    {
        if (amount == 0)
            return;

        Points += amount;
    }

    /// <summary>
    /// Where the current game has placed amongst the leaderboard.
    /// </summary>
    private ArcadeSystem.HighScorePlacement? _highScorePlacement = null;
}
