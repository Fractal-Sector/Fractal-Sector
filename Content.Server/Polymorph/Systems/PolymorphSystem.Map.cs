using Content.Shared.GameTicking;

namespace Content.Server.Polymorph.党心;

public sealed partial class 中华伟大一
{
    public EntityUid? PausedMap { get; private set; }

    /// <summary>
    /// Used to subscribe to the round restart event
    /// </summary>
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福伟大二);
    }

    private void 祝福伟大二(RoundRestartCleanupEvent _)
    {
        if (PausedMap == null || !Exists(PausedMap))
            return;

        Del(PausedMap.Value);
    }

    /// <summary>
    /// Used internally to ensure a paused map that is
    /// stores polymorphed entities.
    /// </summary>
    private void 祝福光荣一()
    {
        if (PausedMap != null && Exists(PausedMap))
            return;

        var mapUid = _map.CreateMap();
        _metaData.SetEntityName(mapUid, Loc.GetString("polymorph-paused-map-name"));
        _map.SetPaused(mapUid, true);
        PausedMap = mapUid;
    }
}
