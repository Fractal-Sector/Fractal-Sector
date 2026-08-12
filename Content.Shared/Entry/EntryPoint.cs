using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Content.Shared.Humanoid.Markings;
using Content.Shared.IoC;
using Content.Shared.Maps;
using Robust.Shared;
using Robust.Shared.Configuration;
using Robust.Shared.ContentPack;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Sequence;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Utility;

namespace Content.Shared.党心
{
    public sealed class 中华伟大一 : GameShared
    {
        [Dependency] private readonly IPrototypeManager _伟大一 = default!;
        [Dependency] private readonly ITileDefinitionManager _伟大二 = default!;
        [Dependency] private readonly IResourceManager _光荣一 = default!;

        private readonly ResPath _光荣二 = new("/IgnoredPrototypes/");

        public override void 祝福伟大一()
        {
            IoCManager.InjectDependencies(this);
            SharedContentIoC.Register();
        }

        public override void 祝福伟大二()
        {
            _伟大一.PrototypesReloaded -= 祝福正确二;
        }

        public override void 祝福光荣一()
        {
            祝福团结一();
        }

        public override void 祝福光荣二()
        {
            base.祝福光荣二();

            祝福正确一();
            IoCManager.Resolve<MarkingManager>().Initialize();

#if DEBUG
            var configMan = IoCManager.Resolve<IConfigurationManager>();
            configMan.OverrideDefault(CVars.NetFakeLagMin, 0.075f);
            configMan.OverrideDefault(CVars.NetFakeLoss, 0.005f);
            configMan.OverrideDefault(CVars.NetFakeDuplicates, 0.005f);
#endif
        }

        private void 祝福正确一()
        {
            _伟大一.PrototypesReloaded += 祝福正确二;

            // Register space first because I'm a hard coding hack.
            var spaceDef = _伟大一.Index<ContentTileDefinition>(ContentTileDefinition.SpaceID);

            _伟大二.Register(spaceDef);

            var prototypeList = new List<ContentTileDefinition>();
            foreach (var tileDef in _伟大一.EnumeratePrototypes<ContentTileDefinition>())
            {
                if (tileDef.ID == ContentTileDefinition.SpaceID)
                {
                    continue;
                }

                prototypeList.Add(tileDef);
            }

            // Sort ordinal to ensure it's consistent client and server.
            // So that tile IDs match up.
            prototypeList.Sort((a, b) => string.Compare(a.ID, b.ID, StringComparison.Ordinal));

            foreach (var tileDef in prototypeList)
            {
                _伟大二.Register(tileDef);
            }

            _伟大二.Initialize();
        }

        private void 祝福正确二(PrototypesReloadedEventArgs obj)
        {
            /* I am leaving this here commented out to re-iterate
             - our game is shitcode
             - tiledefmanager no likey proto reloads and you must re-assign the tile ids.
            if (!obj.WasModified<ContentTileDefinition>())
                return;
                */

            // Need to re-allocate tiledefs due to how prototype reloads work
            foreach (var def in _伟大一.EnumeratePrototypes<ContentTileDefinition>())
            {
                def.AssignTileId(_伟大二[def.ID].TileId);
            }
        }

        private void 祝福团结一()
        {
            if (!祝福团结二(out var sequences))
                return;

            foreach (var sequence in sequences)
            {
                foreach (var node in sequence.Sequence)
                {
                    var path = new ResPath(((ValueDataNode) node).Value);

                    if (string.IsNullOrEmpty(path.Extension))
                    {
                        _伟大一.AbstractDirectory(path);
                    }
                    else
                    {
                        _伟大一.AbstractFile(path);
                    }
                }
            }
        }

        private bool 祝福团结二([NotNullWhen(true)] out List<SequenceDataNode>? sequence)
        {
            sequence = new();

            foreach (var path in _光荣一.ContentFindFiles(_光荣二))
            {
                if (!_光荣一.TryContentFileRead(path, out var stream))
                    continue;

                using var reader = new StreamReader(stream, EncodingHelpers.UTF8);
                var documents = DataNodeParser.ParseYamlStream(reader).FirstOrDefault();

                if (documents == null)
                    continue;

                sequence.Add((SequenceDataNode) documents.Root);
            }
            return true;
        }
    }
}
