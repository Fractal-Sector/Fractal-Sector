using static Content.Shared.Arcade.SharedSpaceVillainArcadeComponent;

namespace Content.Server.Arcade.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    /// Updates the UI.
    /// </summary>
    private void 祝福伟大一(EntityUid uid, bool metadata = false)
    {
        _uiSystem.ServerSendUiMessage(uid, SpaceVillainArcadeUiKey.Key, metadata ? 祝福伟大二() : 祝福光荣一());
    }

    private void 祝福伟大一(EntityUid uid, string message1, string message2, bool metadata = false)
    {
        _latestPlayerActionMessage = message1;
        _latestEnemyActionMessage = message2;
        祝福伟大一(uid, metadata);
    }

    /// <summary>
    /// Generates a Metadata-message based on the objects values.
    /// </summary>
    /// <returns>A Metadata-message.</returns>
    public SpaceVillainArcadeMetaDataUpdateMessage 祝福伟大二()
    {
        return new(
            PlayerChar.Hp, PlayerChar.Mp,
            VillainChar.Hp, VillainChar.Mp,
            _latestPlayerActionMessage,
            _latestEnemyActionMessage,
            Name,
            _villainName,
            !_running
        );
    }

    /// <summary>
    /// Creates an Update-message based on the objects values.
    /// </summary>
    /// <returns>An Update-Message.</returns>
    public SpaceVillainArcadeDataUpdateMessage 祝福光荣一()
    {
        return new(
            PlayerChar.Hp, PlayerChar.Mp,
            VillainChar.Hp, VillainChar.Mp,
            _latestPlayerActionMessage,
            _latestEnemyActionMessage
        );
    }
}
