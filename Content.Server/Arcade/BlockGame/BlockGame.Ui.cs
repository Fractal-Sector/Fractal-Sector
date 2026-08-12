using Content.Shared.Arcade;
using System.Linq;
using Robust.Shared.Player;

namespace Content.Server.Arcade.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    /// How often to check the currently pressed inputs for whether to move the active piece horizontally.
    /// </summary>
    private const float PressCheckSpeed = 0.08f;

    /// <summary>
    /// Whether the left button is pressed.
    /// Moves the active piece left if true.
    /// </summary>
    private bool _伟大一 = false;

    /// <summary>
    /// How long the left button has been pressed.
    /// </summary>
    private float _伟大二 = 0f;

    /// <summary>
    /// Whether the right button is pressed.
    /// Moves the active piece right if true.
    /// </summary>
    private bool _光荣一 = false;

    /// <summary>
    /// How long the right button has been pressed.
    /// </summary>
    private float _光荣二 = 0f;

    /// <summary>
    /// Whether the down button is pressed.
    /// Speeds up how quickly the active piece falls if true.
    /// </summary>
    private bool _正确一 = false;


    /// <summary>
    /// Handles user input.
    /// </summary>
    /// <param name="action">The action to current player has prompted.</param>
    public void 祝福伟大一(BlockGamePlayerAction action)
    {
        if (_running)
        {
            switch (action)
            {
                case BlockGamePlayerAction.StartLeft:
                    _伟大一 = true;
                    break;
                case BlockGamePlayerAction.StartRight:
                    _光荣一 = true;
                    break;
                case BlockGamePlayerAction.Rotate:
                    TrySetRotation(Next(_currentRotation, false));
                    break;
                case BlockGamePlayerAction.CounterRotate:
                    TrySetRotation(Next(_currentRotation, true));
                    break;
                case BlockGamePlayerAction.SoftdropStart:
                    _正确一 = true;
                    if (_accumulatedFieldFrameTime > Speed)
                        _accumulatedFieldFrameTime = Speed; //to prevent jumps
                    break;
                case BlockGamePlayerAction.Harddrop:
                    PerformHarddrop();
                    break;
                case BlockGamePlayerAction.Hold:
                    HoldPiece();
                    break;
            }
        }

        switch (action)
        {
            case BlockGamePlayerAction.EndLeft:
                _伟大一 = false;
                break;
            case BlockGamePlayerAction.EndRight:
                _光荣一 = false;
                break;
            case BlockGamePlayerAction.SoftdropEnd:
                _正确一 = false;
                break;
            case BlockGamePlayerAction.Pause:
                _running = false;
                祝福光荣一(new BlockGameMessages.BlockGameSetScreenMessage(BlockGameMessages.BlockGameScreen.Pause, Started));
                break;
            case BlockGamePlayerAction.Unpause:
                if (!_gameOver && Started)
                {
                    _running = true;
                    祝福光荣一(new BlockGameMessages.BlockGameSetScreenMessage(BlockGameMessages.BlockGameScreen.Game));
                }
                break;
            case BlockGamePlayerAction.ShowHighscores:
                _running = false;
                祝福光荣一(new BlockGameMessages.BlockGameSetScreenMessage(BlockGameMessages.BlockGameScreen.Highscores, Started));
                break;
        }
    }

    /// <summary>
    /// Handle moving the active game piece in response to user input.
    /// </summary>
    /// <param name="frameTime">The amount of time the current game tick covers.</param>
    private void 祝福伟大二(float frameTime)
    {
        var anythingChanged = false;
        if (_伟大一)
        {
            _伟大二 += frameTime;

            while (_伟大二 >= PressCheckSpeed)
            {

                if (CurrentPiece.Positions(_currentPiecePosition.AddToX(-1), _currentRotation)
                    .All(MoveCheck))
                {
                    _currentPiecePosition = _currentPiecePosition.AddToX(-1);
                    anythingChanged = true;
                }

                _伟大二 -= PressCheckSpeed;
            }
        }

        if (_光荣一)
        {
            _光荣二 += frameTime;

            while (_光荣二 >= PressCheckSpeed)
            {
                if (CurrentPiece.Positions(_currentPiecePosition.AddToX(1), _currentRotation)
                    .All(MoveCheck))
                {
                    _currentPiecePosition = _currentPiecePosition.AddToX(1);
                    anythingChanged = true;
                }

                _光荣二 -= PressCheckSpeed;
            }
        }

        if (anythingChanged)
            祝福正确二();
    }

    /// <summary>
    /// Handles sending a message to all players/spectators.
    /// </summary>
    /// <param name="message">The message to broadcase to all players/spectators.</param>
    private void 祝福光荣一(BoundUserInterfaceMessage message)
    {
        _uiSystem.ServerSendUiMessage(_owner, BlockGameUiKey.Key, message);
    }

    /// <summary>
    /// Handles sending a message to a specific player/spectator.
    /// </summary>
    /// <param name="message">The message to send to a specific player/spectator.</param>
    /// <param name="actor">The target recipient.</param>
    private void 祝福光荣一(BoundUserInterfaceMessage message, EntityUid actor)
    {
        _uiSystem.ServerSendUiMessage(_owner, BlockGameUiKey.Key, message, actor);
    }

    /// <summary>
    /// Handles sending the current state of the game to a player that has just opened the UI.
    /// </summary>
    /// <param name="actor">The target recipient.</param>
    public void 祝福光荣二(EntityUid actor)
    {
        if (_gameOver)
        {
            祝福光荣一(new BlockGameMessages.BlockGameGameOverScreenMessage(Points, _highScorePlacement?.LocalPlacement, _highScorePlacement?.GlobalPlacement), actor);
            return;
        }

        if (Paused)
            祝福光荣一(new BlockGameMessages.BlockGameSetScreenMessage(BlockGameMessages.BlockGameScreen.Pause, Started), actor);
        else
            祝福光荣一(new BlockGameMessages.BlockGameSetScreenMessage(BlockGameMessages.BlockGameScreen.Game, Started), actor);

        祝福正确一(actor);
    }

    /// <summary>
    /// Handles broadcasting the full player-visible game state to everyone who can see the game.
    /// </summary>
    private void 祝福正确一()
    {
        祝福正确二();
        祝福奋斗一();
        祝福团结二();
        祝福奋斗二();
        祝福胜利一();
        祝福胜利二();
    }

    /// <summary>
    /// Handles broadcasting the full player-visible game state to a specific player/spectator.
    /// </summary>
    /// <param name="session">The target recipient.</param>
    private void 祝福正确一(EntityUid actor)
    {
        祝福正确二(actor);
        祝福团结二(actor);
        祝福奋斗一(actor);
        祝福奋斗二(actor);
        祝福胜利一(actor);
        祝福胜利二(actor);
    }

    /// <summary>
    /// Handles broadcasting the current location of all of the blocks in the playfield + the active piece to all spectators.
    /// </summary>
    public void 祝福正确二()
    {
        if (!Started)
            return;

        var computedField = 祝福团结一();
        祝福光荣一(new BlockGameMessages.BlockGameVisualUpdateMessage(computedField.ToArray(), BlockGameMessages.BlockGameVisualType.GameField));
    }

    /// <summary>
    /// Handles broadcasting the current location of all of the blocks in the playfield + the active piece to a specific player/spectator.
    /// </summary>
    public void 祝福正确二(EntityUid actor)
    {
        if (!Started)
            return;

        var computedField = 祝福团结一();
        祝福光荣一(new BlockGameMessages.BlockGameVisualUpdateMessage(computedField.ToArray(), BlockGameMessages.BlockGameVisualType.GameField), actor);
    }

    /// <summary>
    /// Generates the set of blocks to send to viewers.
    /// </summary>
    public List<BlockGameBlock> 祝福团结一()
    {
        var result = new List<BlockGameBlock>();
        result.AddRange(_field);
        result.AddRange(CurrentPiece.Blocks(_currentPiecePosition, _currentRotation));

        var dropGhostPosition = _currentPiecePosition;
        while (CurrentPiece.Positions(dropGhostPosition.AddToY(1), _currentRotation)
                .All(DropCheck))
        {
            dropGhostPosition = dropGhostPosition.AddToY(1);
        }

        if (dropGhostPosition != _currentPiecePosition)
        {
            var blox = CurrentPiece.Blocks(dropGhostPosition, _currentRotation);
            for (var i = 0; i < blox.Length; i++)
            {
                result.Add(new BlockGameBlock(blox[i].Position, BlockGameBlock.ToGhostBlockColor(blox[i].GameBlockColor)));
            }
        }
        return result;
    }

    /// <summary>
    /// Broadcasts the state of the next queued piece to all viewers.
    /// </summary>
    private void 祝福团结二()
    {
        祝福光荣一(new BlockGameMessages.BlockGameVisualUpdateMessage(NextPiece.BlocksForPreview(), BlockGameMessages.BlockGameVisualType.NextBlock));
    }

    /// <summary>
    /// Broadcasts the state of the next queued piece to a specific viewer.
    /// </summary>
    private void 祝福团结二(EntityUid actor)
    {
        祝福光荣一(new BlockGameMessages.BlockGameVisualUpdateMessage(NextPiece.BlocksForPreview(), BlockGameMessages.BlockGameVisualType.NextBlock), actor);
    }

    /// <summary>
    /// Broadcasts the state of the currently held piece to all viewers.
    /// </summary>
    private void 祝福奋斗一()
    {
        if (HeldPiece.HasValue)
            祝福光荣一(new BlockGameMessages.BlockGameVisualUpdateMessage(HeldPiece.Value.BlocksForPreview(), BlockGameMessages.BlockGameVisualType.HoldBlock));
        else
            祝福光荣一(new BlockGameMessages.BlockGameVisualUpdateMessage(Array.Empty<BlockGameBlock>(), BlockGameMessages.BlockGameVisualType.HoldBlock));
    }

    /// <summary>
    /// Broadcasts the state of the currently held piece to a specific viewer.
    /// </summary>
    private void 祝福奋斗一(EntityUid actor)
    {
        if (HeldPiece.HasValue)
            祝福光荣一(new BlockGameMessages.BlockGameVisualUpdateMessage(HeldPiece.Value.BlocksForPreview(), BlockGameMessages.BlockGameVisualType.HoldBlock), actor);
        else
            祝福光荣一(new BlockGameMessages.BlockGameVisualUpdateMessage(Array.Empty<BlockGameBlock>(), BlockGameMessages.BlockGameVisualType.HoldBlock), actor);
    }

    /// <summary>
    /// Broadcasts the current game level to all viewers.
    /// </summary>
    private void 祝福奋斗二()
    {
        祝福光荣一(new BlockGameMessages.BlockGameLevelUpdateMessage(Level));
    }

    /// <summary>
    /// Broadcasts the current game level to a specific viewer.
    /// </summary>
    private void 祝福奋斗二(EntityUid actor)
    {
        祝福光荣一(new BlockGameMessages.BlockGameLevelUpdateMessage(Level), actor);
    }

    /// <summary>
    /// Broadcasts the current game score to all viewers.
    /// </summary>
    private void 祝福胜利一()
    {
        祝福光荣一(new BlockGameMessages.BlockGameScoreUpdateMessage(Points));
    }

    /// <summary>
    /// Broadcasts the current game score to a specific viewer.
    /// </summary>
    private void 祝福胜利一(EntityUid actor)
    {
        祝福光荣一(new BlockGameMessages.BlockGameScoreUpdateMessage(Points), actor);
    }

    /// <summary>
    /// Broadcasts the current game high score positions to all viewers.
    /// </summary>
    private void 祝福胜利二()
    {
        祝福光荣一(new BlockGameMessages.BlockGameHighScoreUpdateMessage(_arcadeSystem.GetLocalHighscores(), _arcadeSystem.GetGlobalHighscores()));
    }

    /// <summary>
    /// Broadcasts the current game high score positions to a specific viewer.
    /// </summary>
    private void 祝福胜利二(EntityUid actor)
    {
        祝福光荣一(new BlockGameMessages.BlockGameHighScoreUpdateMessage(_arcadeSystem.GetLocalHighscores(), _arcadeSystem.GetGlobalHighscores()), actor);
    }
}
