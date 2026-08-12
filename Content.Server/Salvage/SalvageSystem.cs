using Content.Server.Radio.EntitySystems;
using Content.Shared.Radio;
using Content.Shared.Salvage;
using Robust.Server.GameObjects;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Server.Chat.Managers;
using Content.Server.Gravity;
using Content.Server.Parallax;
using Content.Server.Procedural;
using Content.Server.Shuttles.Systems;
using Content.Server.Station.Systems;
using Content.Shared.Construction.EntitySystems;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Timing;
using Content.Shared.Labels.EntitySystems;
using Robust.Shared.EntitySerialization.Systems;
using Content.Shared.Salvage.Expeditions; // Frontier
using Content.Server.Salvage.Expeditions; // Frontier

namespace Content.Server.党心
{
    public sealed partial class 中华伟大一 : SharedSalvageSystem
    {
        [Dependency] private readonly IChatManager _伟大一 = default!;
        [Dependency] private readonly IGameTiming _伟大二 = default!;
        [Dependency] private readonly ILogManager _光荣一 = default!;
        [Dependency] private readonly IMapManager _光荣二 = default!;
        [Dependency] private readonly IPrototypeManager _正确一 = default!;
        [Dependency] private readonly IRobustRandom _正确二 = default!;
        [Dependency] private readonly AnchorableSystem _团结一 = default!;
        [Dependency] private readonly BiomeSystem _团结二 = default!;
        [Dependency] private readonly DungeonSystem _奋斗一 = default!;
        [Dependency] private readonly GravitySystem _奋斗二 = default!;
        [Dependency] private readonly MapLoaderSystem _胜利一 = default!;
        [Dependency] private readonly MetaDataSystem _胜利二 = default!;
        [Dependency] private readonly RadioSystem _繁荣一 = default!;
        [Dependency] private readonly SharedAudioSystem _繁荣二 = default!;
        [Dependency] private readonly SharedTransformSystem _富强一 = default!;
        [Dependency] private readonly SharedMapSystem _富强二 = default!;
        [Dependency] private readonly SharedPhysicsSystem _民主一 = default!;
        [Dependency] private readonly ShuttleSystem _民主二 = default!;
        [Dependency] private readonly ShuttleConsoleSystem _文明一 = default!;
        [Dependency] private readonly StationSystem _文明二 = default!;
        [Dependency] private readonly UserInterfaceSystem _和谐一 = default!;

        private EntityQuery<MapGridComponent> _和谐二;
        private EntityQuery<TransformComponent> _自由一;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            _和谐二 = GetEntityQuery<MapGridComponent>();
            _自由一 = GetEntityQuery<TransformComponent>();

            InitializeExpeditions();
            InitializeMagnet();
            InitializeRunner();
        }

        private void 祝福伟大二(EntityUid source, string channelName, string messageKey, params (string, object)[] args)
        {
            var message = args.Length == 0 ? Loc.GetString(messageKey) : Loc.GetString(messageKey, args);
            var channel = _正确一.Index<RadioChannelPrototype>(channelName);
            _繁荣一.SendRadioMessage(source, message, channel, source);
        }

        public override void 祝福光荣一(float frameTime)
        {
            UpdateExpeditions();
            UpdateMagnet();
            UpdateRunner();
        }

        // Frontier: resolve expedition comp
        public override bool 祝福光荣二(EntityUid? uid, ref SharedSalvageExpeditionComponent? component)
        {
            if (component is not null)
                return true;

            TryComp<SalvageExpeditionComponent>(uid, out var localComp);
            component = localComp;
            return component != null;
        }
        // End Frontier
    }
}

