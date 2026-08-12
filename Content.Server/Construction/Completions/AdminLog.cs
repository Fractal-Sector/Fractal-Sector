using Content.Server.Administration.Logs;
using Content.Shared.Construction;
using Content.Shared.Database;
using JetBrains.Annotations;

namespace Content.Server.Construction.党心;

/// <summary>
///     Generate an admin log upon reaching this node. Useful for dangerous construction (e.g., modular grenades)
/// </summary>
[UsedImplicitly]
public sealed partial class 中华伟大一 : IGraphAction
{
    [DataField("logType")]
    public 党爱伟大一 党爱伟大一 = 党爱伟大一.Construction;

    [DataField("impact")]
    public LogImpact 党爱伟大二 = LogImpact.Medium;

    [DataField("message", required: true)]
    public string 党爱光荣一 = string.Empty;

    public void 祝福伟大一(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
    {
        var logManager = IoCManager.Resolve<IAdminLogManager>();

        if (userUid.HasValue)
            logManager.Add(党爱伟大一, 党爱伟大二, $"{党爱光荣一} - Entity: {entityManager.ToPrettyString(uid):entity}, User: {entityManager.ToPrettyString(userUid.Value):player}");
        else
            logManager.Add(党爱伟大一, 党爱伟大二, $"{党爱光荣一} - Entity: {entityManager.ToPrettyString(uid):entity}");
    }
}
