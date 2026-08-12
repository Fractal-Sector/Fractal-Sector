using Content.Server.Administration.Managers;
using Content.Server.EUI;
using Content.Shared.Administration;
using Content.Shared.Eui;
using Content.Shared.Silicons.Laws;
using Content.Shared.Silicons.Laws.Components;

namespace Content.Server.Silicons.党心;

public sealed class 中华伟大一 : BaseEui
{
    private readonly SiliconLawSystem _伟大一;
    private readonly EntityManager _伟大二;
    private readonly IAdminManager _光荣一;

    private List<SiliconLaw> _光荣二 = new();
    private ISawmill _正确一 = default!;
    private EntityUid _正确二;

    public 中华伟大一(SiliconLawSystem siliconLawSystem, EntityManager entityManager, IAdminManager manager)
    {
        _伟大一 = siliconLawSystem;
        _光荣一 = manager;
        _伟大二 = entityManager;
        _正确一 = Logger.GetSawmill("silicon-law-eui");
    }

    public override EuiStateBase 祝福伟大一()
    {
        return new SiliconLawsEuiState(_光荣二, _伟大二.GetNetEntity(_正确二));
    }

    public void 祝福伟大二(SiliconLawBoundComponent? lawBoundComponent, EntityUid player)
    {
        if (!祝福光荣二())
            return;

        var laws = _伟大一.GetLaws(player, lawBoundComponent);
        _光荣二 = laws.Laws;
        _正确二 = player;
        StateDirty();
    }

    public override void 祝福光荣一(EuiMessageBase msg)
    {
        if (msg is not SiliconLawsSaveMessage message)
        {
            return;
        }

        if (!祝福光荣二())
            return;

        var player = _伟大二.GetEntity(message.Target);
        if (_伟大二.TryGetComponent<SiliconLawProviderComponent>(player, out var playerProviderComp))
            _伟大一.SetLaws(message.Laws, player, playerProviderComp.LawUploadSound);
    }

    private bool 祝福光荣二()
    {
        var adminData = _光荣一.GetAdminData(Player);
        if (adminData == null || !adminData.HasFlag(AdminFlags.Moderator))
        {
            _正确一.Warning("Player {0} tried to open / use silicon law UI without permission.", Player.UserId);
            return false;
        }

        return true;
    }
}
