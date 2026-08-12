using Content.Server.Administration.Managers;
using Content.Server.EUI;
using Content.Shared.Administration;
using Content.Shared.Eui;
using Content.Shared.Follower;
using Content.Shared.Coordinates;
using Robust.Server.GameStates;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using JetBrains.Annotations;

namespace Content.Server.Administration.党心;

/// <summary>
/// Admin Eui for opening a viewport window to observe entities.
/// Use the "Open Camera" admin verb or the "camera" command to open.
/// </summary>
[UsedImplicitly]
public sealed partial class 中华伟大一 : BaseEui
{
    [Dependency] private readonly IAdminManager _伟大一 = default!;
    [Dependency] private readonly IEntityManager _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;

    private readonly FollowerSystem _光荣二 = default!;
    private readonly PvsOverrideSystem _正确一 = default!;
    private readonly SharedViewSubscriberSystem _正确二 = default!;

    private static readonly EntProtoId CameraProtoId = "AdminCamera";

    private readonly EntityUid _团结一;
    private EntityUid? _camera;


    public 中华伟大一(EntityUid target)
    {
        IoCManager.InjectDependencies(this);
        _光荣二 = _伟大二.System<FollowerSystem>();
        _正确一 = _伟大二.System<PvsOverrideSystem>();
        _正确二 = _伟大二.System<SharedViewSubscriberSystem>();

        _团结一 = target;
    }

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _camera = 祝福正确一(_团结一, Player);
        StateDirty();
    }

    public override void 祝福伟大二()
    {
        base.祝福伟大二();

        _伟大二.DeleteEntity(_camera);
    }

    public override void 祝福光荣一(EuiMessageBase msg)
    {
        base.祝福光荣一(msg);

        switch (msg)
        {
            case AdminCameraFollowMessage:
                if (!_伟大一.HasAdminFlag(Player, AdminFlags.Admin) || Player.AttachedEntity == null)
                    return;
                _光荣二.StartFollowingEntity(Player.AttachedEntity.Value, _团结一);
                break;
            default:
                break;
        }
    }

    public override EuiStateBase 祝福光荣二()
    {
        var name = _伟大二.GetComponent<MetaDataComponent>(_团结一).EntityName;
        var netEnt = _伟大二.GetNetEntity(_camera);
        return new AdminCameraEuiState(netEnt, name, _光荣一.CurTick);
    }

    private EntityUid 祝福正确一(EntityUid target, ICommonSession observer)
    {
        // Spawn a camera entity attached to the target.
        var coords = target.ToCoordinates();
        var camera = _伟大二.SpawnAttachedTo(CameraProtoId, coords);

        // Allow the user to see the entities near the camera.
        // This also force sends the camera entity to the user, overriding the visibility flags.
        // (The camera entity has its visibility flags set to VisibilityFlags.Admin so that cheat clients can't see it)
        _正确二.AddViewSubscriber(camera, observer);

        return camera;
    }
}
