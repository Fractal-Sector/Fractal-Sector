using Content.Server.Parallax;
using Content.Server.Station.Components;
using Content.Server.Station.Events;
using Robust.Shared.Prototypes;

namespace Content.Server.Station.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly BiomeSystem _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly StationSystem _光荣一 = default!;
    [Dependency] private readonly SharedMapSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<StationBiomeComponent, StationPostInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<StationBiomeComponent> map, ref StationPostInitEvent args)
    {
        var station = _光荣一.GetLargestGrid(map.Owner);
        if (station == null)
            return;

        var mapId = Transform(station.Value).MapID;
        var mapUid = _光荣二.GetMapOrInvalid(mapId);

        _伟大一.EnsurePlanet(mapUid, _伟大二.Index(map.Comp.Biome), map.Comp.Seed, mapLight: map.Comp.MapLightColor);
    }
}
