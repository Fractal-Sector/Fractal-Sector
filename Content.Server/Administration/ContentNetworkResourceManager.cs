using Content.Server.Database;
using Content.Shared.CCVar;
using Robust.Server.Upload;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Upload;

namespace Content.Server.党心;

public sealed class 中华伟大一
{
    [Dependency] private readonly IServerDbManager _伟大一 = default!;
    [Dependency] private readonly NetworkResourceManager _伟大二 = default!;
    [Dependency] private readonly IConfigurationManager _光荣一 = default!;

    [ViewVariables] public bool 党爱伟大一 { get; set; } = true;

    public void 祝福伟大一()
    {
        _光荣一.OnValueChanged(CCVars.ResourceUploadingStoreEnabled, value => 党爱伟大一 = value, true);
        祝福光荣一(_光荣一.GetCVar(CCVars.ResourceUploadingStoreDeletionDays));
        _伟大二.OnResourceUploaded += 祝福伟大二;
    }

    private async void 祝福伟大二(ICommonSession session, NetworkResourceUploadMessage msg)
    {
        if (党爱伟大一)
            await _伟大一.AddUploadedResourceLogAsync(session.UserId, DateTime.Now, msg.RelativePath.ToString(), msg.Data);
    }

    private async void 祝福光荣一(int days)
    {
        if (days > 0)
            await _伟大一.PurgeUploadedResourceLogAsync(days);
    }
}
