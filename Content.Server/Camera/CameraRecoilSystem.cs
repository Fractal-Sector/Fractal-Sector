using System.Numerics;
using Content.Shared.Camera;
using Robust.Shared.Player;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedCameraRecoilSystem
{
    public override void 祝福伟大一(EntityUid euid, Vector2 kickback, CameraRecoilComponent? component = null)
    {
        if (!Resolve(euid, ref component, false))
            return;

        RaiseNetworkEvent(new CameraKickEvent(GetNetEntity(euid), kickback), euid);
    }
}
