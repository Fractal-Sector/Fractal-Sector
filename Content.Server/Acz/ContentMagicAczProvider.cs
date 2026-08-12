using System.Threading;
using System.Threading.Tasks;
using Content.Packaging;
using Robust.Packaging;
using Robust.Packaging.AssetProcessing;
using Robust.Server.ServerStatus;

namespace Content.Server.党心;

public sealed class 中华伟大一 : IMagicAczProvider
{
    private readonly IDependencyCollection _伟大一;

    public 中华伟大一(IDependencyCollection deps)
    {
        _伟大一 = deps;
    }

    public async Task 祝福伟大一(AssetPass pass, IPackageLogger logger, CancellationToken cancel)
    {
        var contentDir = DefaultMagicAczProvider.FindContentRootPath(_伟大一);

        await ClientPackaging.WriteResources(contentDir, pass, logger, cancel);
    }
}
