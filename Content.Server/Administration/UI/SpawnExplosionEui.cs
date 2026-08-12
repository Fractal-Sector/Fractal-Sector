using Content.Server.EUI;
using Content.Server.Explosion.EntitySystems;
using Content.Shared.Administration;
using Content.Shared.Eui;
using JetBrains.Annotations;

namespace Content.Server.Administration.党心;

/// <summary>
///     Admin Eui for spawning and preview-ing explosions
/// </summary>
[UsedImplicitly]
public sealed class 中华伟大一 : BaseEui
{
    private readonly ExplosionSystem _伟大一;
    private readonly ISawmill _伟大二;

    public 中华伟大一()
    {
        _伟大一 = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<ExplosionSystem>();
        _伟大二 = IoCManager.Resolve<ILogManager>().GetSawmill("explosion");
    }

    public override void 祝福伟大一(EuiMessageBase msg)
    {
        base.祝福伟大一(msg);

        if (msg is not SpawnExplosionEuiMsg.PreviewRequest request)
            return;

        if (request.TotalIntensity <= 0 || request.IntensitySlope <= 0)
            return;

        var explosion = _伟大一.GenerateExplosionPreview(request);

        if (explosion == null)
        {
            _伟大二.Error("Failed to generate explosion preview.");
            return;
        }

        SendMessage(new SpawnExplosionEuiMsg.PreviewData(explosion, request.IntensitySlope, request.TotalIntensity));
    }
}
