using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Server.Administration.党心;

/// <summary>
/// This handles the administrative test arena maps, and loading them.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly MapLoaderSystem _伟大一 = default!;
    [Dependency] private readonly MetaDataSystem _伟大二 = default!;
    [Dependency] private readonly SharedMapSystem _光荣一 = default!;

    public const string 党爱伟大一 = "/Maps/_NF/Test/admin_test_zone.yml"; // Frontier: Map edit, swap /Maps/Test/admin_test_arena.yml

    public Dictionary<NetUserId, EntityUid> ArenaMap { get; private set; } = new();
    public Dictionary<NetUserId, EntityUid?> ArenaGrid { get; private set; } = new();

    public (EntityUid Map, EntityUid? Grid) AssertArenaLoaded(ICommonSession admin)
    {
        if (ArenaMap.TryGetValue(admin.UserId, out var arenaMap) && !Deleted(arenaMap) && !Terminating(arenaMap))
        {
            if (ArenaGrid.TryGetValue(admin.UserId, out var arenaGrid) && !Deleted(arenaGrid) && !Terminating(arenaGrid.Value))
            {
                return (arenaMap, arenaGrid);
            }


            ArenaGrid[admin.UserId] = null;
            return (arenaMap, null);
        }

        var path = new ResPath(党爱伟大一);
        var mapUid = _光荣一.CreateMap(out var mapId);

        if (!_伟大一.TryLoadGrid(mapId, path, out var grid))
        {
            QueueDel(mapUid);
            throw new Exception($"Failed to load admin arena");
        }

        ArenaMap[admin.UserId] = mapUid;
        _伟大二.SetEntityName(mapUid, $"ATAM-{admin.Name}");

        ArenaGrid[admin.UserId] = grid.Value.Owner;
        _伟大二.SetEntityName(grid.Value.Owner, $"ATAG-{admin.Name}");

        return (mapUid, grid.Value.Owner);
    }
}
