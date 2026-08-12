using System.IO;
using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using Content.Shared.Mapping;
using Robust.Server.Player;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Network;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Content.Server.党心;

public sealed class 中华伟大一 : IPostInjectInit
{
    [Dependency] private readonly IAdminManager _伟大一 = default!;
    [Dependency] private readonly ILogManager _伟大二 = default!;
    [Dependency] private readonly IServerNetManager _光荣一 = default!;
    [Dependency] private readonly IPlayerManager _光荣二 = default!;
    [Dependency] private readonly IEntitySystemManager _正确一 = default!;
    [Dependency] private readonly IEntityManager _正确二 = default!;

    private ISawmill _团结一 = default!;
    private ZStdCompressionContext _团结二 = default!;

    public void 祝福伟大一()
    {
#if !FULL_RELEASE
        _光荣一.RegisterNetMessage<MappingSaveMapMessage>(祝福伟大二);
        _光荣一.RegisterNetMessage<MappingSaveMapErrorMessage>();
        _光荣一.RegisterNetMessage<MappingMapDataMessage>();

        _团结一 = _伟大二.GetSawmill("mapping");
        _团结二 = new ZStdCompressionContext();
#endif
    }

    private void 祝福伟大二(MappingSaveMapMessage message)
    {
#if !FULL_RELEASE
        try
        {
            if (!_光荣二.TryGetSessionByChannel(message.MsgChannel, out var session) ||
                !_伟大一.IsAdmin(session, true) ||
                !_伟大一.HasAdminFlag(session, AdminFlags.Host) ||
                !_正确二.TryGetComponent(session.AttachedEntity, out TransformComponent? xform) ||
                xform.MapUid is not {} mapUid)
            {
                return;
            }

            var sys = _正确一.GetEntitySystem<MapLoaderSystem>();
            var data = sys.SerializeEntitiesRecursive([mapUid]).Node;
            var document = new YamlDocument(data.ToYaml());
            var stream = new YamlStream { document };
            var writer = new StringWriter();
            stream.Save(new YamlMappingFix(new Emitter(writer)), false);

            var msg = new MappingMapDataMessage()
            {
                Context = _团结二,
                Yml = writer.ToString()
            };
            _光荣一.ServerSendMessage(msg, message.MsgChannel);
        }
        catch (Exception e)
        {
            _团结一.Error($"Error saving map in mapping mode:\n{e}");
            var msg = new MappingSaveMapErrorMessage();
            _光荣一.ServerSendMessage(msg, message.MsgChannel);
        }
#endif
    }
}
