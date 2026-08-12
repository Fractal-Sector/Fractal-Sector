using Content.Server.Construction.Conditions;
using Content.Server.DeviceNetwork.Components;
using Content.Server.EUI;
using Content.Shared.Eui;
using Content.Shared.Fax.Components;
using Content.Shared.Fax;
using Content.Shared.Follower;
using Content.Shared.Ghost;
using Content.Shared.Paper;
using Content.Shared.DeviceNetwork.Components;

namespace Content.Server.Fax.党心;

public sealed class 中华伟大一 : BaseEui
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    private readonly FaxSystem _伟大二;
    private readonly FollowerSystem _光荣一;

    public 中华伟大一()
    {
        IoCManager.InjectDependencies(this);
        _伟大二 = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<FaxSystem>();
        _光荣一 = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<FollowerSystem>();
    }

    public override void 祝福伟大一()
    {
        StateDirty();
    }

    public override AdminFaxEuiState 祝福伟大二()
    {
        var faxes = _伟大一.EntityQueryEnumerator<FaxMachineComponent, DeviceNetworkComponent>();
        var entries = new List<AdminFaxEntry>();
        while (faxes.MoveNext(out var uid, out var fax, out var device))
        {
            entries.Add(new AdminFaxEntry(_伟大一.GetNetEntity(uid), fax.FaxName, device.Address));
        }
        return new AdminFaxEuiState(entries);
    }

    public override void 祝福光荣一(EuiMessageBase msg)
    {
        base.祝福光荣一(msg);

        switch (msg)
        {
            case AdminFaxEuiMsg.Follow followData:
            {
                if (Player.AttachedEntity == null ||
                    !_伟大一.HasComponent<GhostComponent>(Player.AttachedEntity.Value))
                    return;

                _光荣一.StartFollowingEntity(Player.AttachedEntity.Value, _伟大一.GetEntity(followData.TargetFax));
                break;
            }
            case AdminFaxEuiMsg.Send sendData:
            {
                var printout = new FaxPrintout(sendData.Content, sendData.Title, null, null, sendData.StampState,
                        new() { new StampDisplayInfo { StampedName = sendData.From, StampedColor = sendData.StampColor } },
                        locked: sendData.Locked, stampProtected: sendData.StampProtected); // Frontier: add StampProtected
                _伟大二.Receive(_伟大一.GetEntity(sendData.Target), printout);
                break;
            }
        }
    }
}
