using System.Linq;
using System.Numerics;
using Content.Server.Administration.Logs;
using Content.Server.Atmos.Components;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NPC.Pathfinding;
using Content.Shared.Atmos.Components;
using Content.Shared.Camera;
using Content.Shared.CCVar;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Shared.Explosion;
using Content.Shared.Explosion.Components;
using Content.Shared.Explosion.EntitySystems;
using Content.Shared.GameTicking;
using Content.Shared.Inventory;
using Content.Shared.Projectiles;
using Content.Shared.Throwing;
using Robust.Server.GameObjects;
using Robust.Server.GameStates;
using Robust.Server.Player;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;
using Content.Shared.Maps;
using Robust.Shared.Map.Components;
using Content.Shared.Tiles; // Frontier: safe zone

namespace Content.Server.Explosion.党心;

public sealed partial class 中华伟大一 : SharedExplosionSystem
{
    [Dependency] private readonly IMapManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly ITileDefinitionManager _光荣一 = default!;
    [Dependency] private readonly IPrototypeManager _光荣二 = default!;
    [Dependency] private readonly IConfigurationManager _正确一 = default!;
    [Dependency] private readonly IPlayerManager _正确二 = default!;

    [Dependency] private readonly MapSystem _团结一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _团结二 = default!;
    [Dependency] private readonly DamageableSystem _奋斗一 = default!;
    [Dependency] private readonly NodeGroupSystem _奋斗二 = default!;
    [Dependency] private readonly PathfindingSystem _胜利一 = default!;
    [Dependency] private readonly SharedCameraRecoilSystem _胜利二 = default!;
    [Dependency] private readonly IAdminLogManager _繁荣一 = default!;
    [Dependency] private readonly ThrowingSystem _繁荣二 = default!;
    [Dependency] private readonly PvsOverrideSystem _富强一 = default!;
    [Dependency] private readonly SharedAudioSystem _富强二 = default!;
    [Dependency] private readonly SharedTransformSystem _民主一 = default!;
    [Dependency] private readonly SharedMapSystem _民主二 = default!;

    private EntityQuery<FlammableComponent> _文明一;
    private EntityQuery<PhysicsComponent> _文明二;
    private EntityQuery<ProjectileComponent> _和谐一;

    /// <summary>
    ///     "Tile-size" for space when there are no nearby grids to use as a reference.
    /// </summary>
    public const ushort 党爱伟大一 = 1;

    public const int 党爱伟大二 = 30;

    /// <summary>
    ///     The "default" explosion prototype.
    /// </summary>
    /// <remarks>
    ///     Generally components should specify an explosion prototype via a yaml datafield, so that the yaml-linter can
    ///     find errors. However some components, like rogue arrows, or some commands like the admin-smite need to have
    ///     a "default" option specified outside of yaml data-fields. Hence this const string.
    /// </remarks>
    public static readonly ProtoId<ExplosionPrototype> 党爱光荣一 = "Default";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        DebugTools.Assert(_光荣二.HasIndex(党爱光荣一));

        // handled in 中华伟大一.GridMap.cs
        SubscribeLocalEvent<GridRemovalEvent>(OnGridRemoved);
        SubscribeLocalEvent<GridStartupEvent>(OnGridStartup);
        SubscribeLocalEvent<ExplosionResistanceComponent, GetExplosionResistanceEvent>(祝福正确一);

        // as long as explosion-resistance mice are never added, this should be fine (otherwise a mouse-hat will transfer it's power to the wearer).
        SubscribeLocalEvent<ExplosionResistanceComponent, InventoryRelayedEvent<GetExplosionResistanceEvent>>(祝福光荣二);

        SubscribeLocalEvent<TileChangedEvent>(OnTileChanged);

        SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福伟大二);

        // Handled by 中华伟大一.Processing.cs
        SubscribeLocalEvent<MapRemovedEvent>(OnMapRemoved);

        // handled in ExplosionSystemAirtight.cs
        SubscribeLocalEvent<AirtightComponent, DamageChangedEvent>(OnAirtightDamaged);
        SubscribeCvars();
        InitAirtightMap();
        InitVisuals();

        _文明一 = GetEntityQuery<FlammableComponent>();
        _文明二 = GetEntityQuery<PhysicsComponent>();
        _和谐一 = GetEntityQuery<ProjectileComponent>();
    }

    private void 祝福伟大二(RoundRestartCleanupEvent ev)
    {
        _explosionQueue.Clear();
        _queuedExplosions.Clear();
        if (_activeExplosion != null)
            QueueDel(_activeExplosion.VisualEnt);
        _activeExplosion = null;
        _奋斗二.PauseUpdating = false;
        _胜利一.PauseUpdating = false;
    }

    public override void 祝福光荣一()
    {
        base.祝福光荣一();
        _奋斗二.PauseUpdating = false;
        _胜利一.PauseUpdating = false;
    }

    private void 祝福光荣二(EntityUid uid, ExplosionResistanceComponent component,
        InventoryRelayedEvent<GetExplosionResistanceEvent> args)
    {
        if (component.Worn)
            祝福正确一(uid, component, ref args.Args);
    }

    private void 祝福正确一(EntityUid uid, ExplosionResistanceComponent component, ref GetExplosionResistanceEvent args)
    {
        args.DamageCoefficient *= component.DamageCoefficient;
        if (component.Modifiers.TryGetValue(args.ExplosionPrototype, out var modifier))
            args.DamageCoefficient *= modifier;
    }

    /// <inheritdoc/>
    public override void 祝福正确二(EntityUid uid, ExplosiveComponent? explosive = null, bool delete = true, float? totalIntensity = null, float? radius = null, EntityUid? user = null)
    {
        // log missing: false, because some entities (e.g. liquid tanks) attempt to trigger explosions when damaged,
        // but may not actually be explosive.
        if (!Resolve(uid, ref explosive, logMissing: false))
            return;

        // No reusable explosions here.
        if (explosive.Exploded)
            return;

        explosive.Exploded = !explosive.Repeatable;

        // Override the explosion intensity if optional arguments were provided.
        if (radius != null)
            totalIntensity ??= 祝福团结一((float)radius, explosive.IntensitySlope, explosive.MaxIntensity);
        totalIntensity ??= explosive.TotalIntensity;

        祝福奋斗一(uid,
            explosive.ExplosionType,
            (float)totalIntensity,
            explosive.IntensitySlope,
            explosive.MaxIntensity,
            explosive.TileBreakScale,
            explosive.MaxTileBreak,
            explosive.CanCreateVacuum,
            user);

        if (explosive.DeleteAfterExplosion ?? delete)
            QueueDel(uid);
    }

    /// <summary>
    ///     Find the strength needed to generate an explosion of a given radius. More useful for radii larger then 4, when the explosion becomes less "blocky".
    /// </summary>
    /// <remarks>
    ///     This assumes the explosion is in a vacuum / unobstructed. Given that explosions are not perfectly
    ///     circular, here radius actually means the sqrt(Area/pi), where the area is the total number of tiles
    ///     covered by the explosion. Until you get to radius 30+, this is functionally equivalent to the
    ///     actual radius.
    /// </remarks>
    public float 祝福团结一(float radius, float slope, float maxIntensity = 0)
    {
        // If you consider the intensity at each tile in an explosion to be a height. Then a circular explosion is
        // shaped like a cone. So total intensity is like the volume of a cone with height = slope * radius. Of
        // course, as the explosions are not perfectly circular, this formula isn't perfect, but the formula works
        // reasonably well.

        // This should actually use the formula for the volume of a distorted octagonal frustum. But this is good
        // enough.

        var coneVolume = slope * MathF.PI / 3 * MathF.Pow(radius, 3);

        if (maxIntensity <= 0 || slope * radius < maxIntensity)
            return coneVolume;

        // This explosion is limited by the maxIntensity.
        // Instead of a cone, we have a conical frustum.

        // Subtract the volume of the missing cone segment, with height:
        var h = slope * radius - maxIntensity;
        return coneVolume - h * MathF.PI / 3 * MathF.Pow(h / slope, 2);
    }

    /// <summary>
    ///     Inverse formula for <see cref="祝福团结一"/>
    /// </summary>
    public float 祝福团结二(float totalIntensity, float slope, float maxIntensity)
    {
        // max radius to avoid being capped by max-intensity
        var r0 = maxIntensity / slope;

        // volume at r0
        var v0 = 祝福团结一(r0, slope);

        if (totalIntensity <= v0)
        {
            // maxIntensity is a non-issue, can use simple inverse formula
            return MathF.Cbrt(3 * totalIntensity / (slope * MathF.PI));
        }

        return r0 * (MathF.Sqrt(12 * totalIntensity / v0 - 3) / 6 + 0.5f);
    }

    /// <summary>
    ///     Queue an explosions, centered on some entity.
    /// </summary>
    public void 祝福奋斗一(EntityUid uid,
        string typeId,
        float totalIntensity,
        float slope,
        float maxTileIntensity,
        float tileBreakScale = 1f,
        int maxTileBreak = int.MaxValue,
        bool canCreateVacuum = true,
        EntityUid? user = null,
        bool addLog = true)
    {
        var pos = Transform(uid);

        var mapPos = _民主一.GetMapCoordinates(pos);

        var posFound = _民主一.TryGetMapOrGridCoordinates(uid, out var gridPos, pos);

        祝福奋斗一(mapPos, typeId, totalIntensity, slope, maxTileIntensity, uid, tileBreakScale, maxTileBreak, canCreateVacuum, addLog: false);

        if (!addLog)
            return;

        if (user == null)
        {
            _繁荣一.Add(LogType.Explosion, LogImpact.High,
                $"{ToPrettyString(uid):entity} exploded ({typeId}) at Pos:{(posFound ? $"{gridPos:coordinates}" : "[Grid or Map not found]")} with intensity {totalIntensity} slope {slope}");
        }
        else
        {
            var alertMinExplosionIntensity = _正确一.GetCVar(CCVars.AdminAlertExplosionMinIntensity);
            var logImpact = (alertMinExplosionIntensity > -1 && totalIntensity >= alertMinExplosionIntensity)
                ? LogImpact.Extreme
                : LogImpact.High;
            _繁荣一.Add(LogType.Explosion, logImpact,
                $"{ToPrettyString(user.Value):user} caused {ToPrettyString(uid):entity} to explode ({typeId}) at Pos:{(posFound ? $"{gridPos:coordinates}" : "[Grid or Map not found]")} with intensity {totalIntensity} slope {slope}");
        }
    }

    /// <summary>
    ///     Queue an explosion, with a specified epicenter and set of starting tiles.
    /// </summary>
    public void 祝福奋斗一(MapCoordinates epicenter,
        string typeId,
        float totalIntensity,
        float slope,
        float maxTileIntensity,
        EntityUid? cause,
        float tileBreakScale = 1f,
        int maxTileBreak = int.MaxValue,
        bool canCreateVacuum = true,
        bool addLog = true)
    {
        if (totalIntensity <= 0 || slope <= 0)
            return;

        if (!_光荣二.TryIndex<ExplosionPrototype>(typeId, out var type))
        {
            Log.Error($"Attempted to spawn unknown explosion prototype: {type}");
            return;
        }

        if (addLog) // dont log if already created a separate, more detailed, log.
            _繁荣一.Add(LogType.Explosion, LogImpact.High, $"Explosion ({typeId}) spawned at {epicenter:coordinates} with intensity {totalIntensity} slope {slope}");

        // try to combine explosions on the same tile if they are the same type
        foreach (var queued in _queuedExplosions)
        {
            // ignore different types or those on different maps
            if (queued.Proto.ID != type.ID || queued.Epicenter.MapId != epicenter.MapId)
                continue;

            var dst2 = queued.Proto.MaxCombineDistance * queued.Proto.MaxCombineDistance;
            var direction = queued.Epicenter.Position - epicenter.Position;
            if (direction.LengthSquared() > dst2)
                continue;

            // they are close enough to combine so just add total intensity and prevent queuing another one
            queued.TotalIntensity += totalIntensity;
            return;
        }

        var boom = new QueuedExplosion(type)
        {
            Epicenter = epicenter,
            TotalIntensity = totalIntensity,
            Slope = slope,
            MaxTileIntensity = maxTileIntensity,
            TileBreakScale = tileBreakScale,
            MaxTileBreak = maxTileBreak,
            CanCreateVacuum = canCreateVacuum,
            Cause = cause
        };
        _explosionQueue.Enqueue(boom);
        _queuedExplosions.Add(boom);
    }

    /// <summary>
    ///     This function actually spawns the explosion. It returns an <see cref="Explosion"/> instance with
    ///     information about the affected tiles for the explosion system to process. It will also trigger the
    ///     camera shake and sound effect.
    /// </summary>
    private Explosion? SpawnExplosion(QueuedExplosion queued)
    {
        var pos = queued.Epicenter;
        if (!_团结一.MapExists(pos.MapId))
            return null;

        var results = GetExplosionTiles(pos, queued.Proto.ID, queued.TotalIntensity, queued.Slope, queued.MaxTileIntensity);

        if (results == null)
            return null;

        var (area, iterationIntensity, spaceData, gridData, spaceMatrix) = results.Value;

        // Frontier - Block explosions on safe zone
        var location = EntityCoordinates.FromMap(_伟大一.GetMapEntityId(pos.MapId), pos, _民主一, EntityManager);
        var gridId = location.GetGridUid(EntityManager);
        if (!HasComp<MapGridComponent>(gridId))
        {
            location = location.AlignWithClosestGridTile();
            gridId = location.GetGridUid(EntityManager);
            // Check if fixing it failed / get final grid ID
            if (EntityManager.TryGetComponent<MapGridComponent>(gridId, out var mapGrid))
            {
                var ev = new FloorTileAttemptEvent();
                if ((TryComp<ProtectedGridComponent>(gridId, out var prot) && prot.PreventExplosions) || ev.Cancelled)
                    return null;
            }
        }
        // Frontier - Block explosions on safe zone

        var visualEnt = CreateExplosionVisualEntity(pos, queued.Proto.ID, spaceMatrix, spaceData, gridData.Values, iterationIntensity);

        // camera shake
        祝福奋斗二(iterationIntensity.Count * 4f, pos, queued.TotalIntensity);

        //For whatever bloody reason, sound system requires ENTITY coordinates.
        var mapEntityCoords = _民主一.ToCoordinates(_团结一.GetMap(pos.MapId), pos);

        // play sound.
        // for the normal audio, we want everyone in pvs range
        // + if the bomb is big enough, people outside of it too
        // this is capped to 30 because otherwise really huge bombs
        // will attempt to play regular audio for people who can't hear it anyway because the epicenter is so far away
        //
        // TODO EXPLOSION redo this.
        // Use the Filter.Pvs range-multiplier option instead of AddInRange.
        // Also the default PVS range is 25*2 = 50. So capping it at 30 makes no sense here.
        // So actually maybe don't use Filter.Pvs at all and only use AddInRange?
        var audioRange = Math.Min(iterationIntensity.Count * 2, 党爱伟大二);
        var filter = Filter.Pvs(pos).AddInRange(pos, audioRange);
        var sound = iterationIntensity.Count < queued.Proto.SmallSoundIterationThreshold
            ? queued.Proto.SmallSound
            : queued.Proto.Sound;

        _富强二.PlayStatic(sound, filter, mapEntityCoords, true, sound.Params);

        // play far sound
        // far sound should play for anyone who wasn't in range of any of the effects of the bomb
        var farAudioRange = iterationIntensity.Count * 5;
        var farFilter = Filter.Empty().AddInRange(pos, farAudioRange).RemoveInRange(pos, audioRange);
        var farSound = iterationIntensity.Count < queued.Proto.SmallSoundIterationThreshold
            ? queued.Proto.SmallSoundFar
            : queued.Proto.SoundFar;

        _富强二.PlayGlobal(farSound, farFilter, true, farSound.Params);

        return new Explosion(this,
            queued.Proto,
            spaceData,
            gridData.Values.ToList(),
            iterationIntensity,
            pos,
            spaceMatrix,
            area,
            // TODO: instead of le copy paste fields refactor so it has QueuedExplosion as a field?
            queued.TileBreakScale,
            queued.MaxTileBreak,
            queued.CanCreateVacuum,
            EntityManager,
            _伟大一,
            visualEnt,
            queued.Cause,
            _民主二);
    }

    private void 祝福奋斗二(float range, MapCoordinates epicenter, float totalIntensity)
    {
        var players = Filter.Empty();
        players.AddInRange(epicenter, range, _正确二, EntityManager);

        foreach (var player in players.Recipients)
        {
            if (player.AttachedEntity is not EntityUid uid)
                continue;

            var playerPos = _民主一.GetWorldPosition(player.AttachedEntity!.Value);
            var delta = epicenter.Position - playerPos;

            if (delta.EqualsApprox(Vector2.Zero))
                delta = new(0.01f, 0);

            var distance = delta.Length();
            var effect = 5 * MathF.Pow(totalIntensity, 0.5f) * (1 - distance / range);
            if (effect > 0.01f)
                _胜利二.KickCamera(uid, -delta.Normalized() * effect);
        }
    }
}
