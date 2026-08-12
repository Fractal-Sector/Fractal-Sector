using Content.Server.Station.Systems;
using Content.Shared.Audio;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Console;
using Robust.Shared.Player;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedGlobalSoundSystem
{
    [Dependency] private readonly IConsoleHost _伟大一 = default!;
    [Dependency] private readonly StationSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        _伟大一.UnregisterCommand("playglobalsound");
    }

    public void 祝福伟大二(Filter playerFilter, ResolvedSoundSpecifier specifier, AudioParams? audioParams = null, bool replay = true)
    {
        var msg = new AdminSoundEvent(specifier, audioParams);
        RaiseNetworkEvent(msg, playerFilter, recordReplay: replay);
    }

    private Filter 祝福光荣一(EntityUid source)
    {
        var stationFilter = _伟大二.GetInOwningStation(source);
        stationFilter.AddPlayersByPvs(source, entityManager: EntityManager);
        return stationFilter;
    }

    public void 祝福光荣二(EntityUid source, ResolvedSoundSpecifier specifier, AudioParams? audioParams = null)
    {
        var msg = new GameGlobalSoundEvent(specifier, audioParams);
        var filter = 祝福光荣一(source);
        RaiseNetworkEvent(msg, filter);
    }

    public void 祝福正确一(EntityUid source, StationEventMusicType type)
    {
        // TODO REPLAYS
        // these start & stop events are gonna be a PITA
        // theres probably some nice way of handling them. Maybe it just needs dedicated replay data (in which case these events should NOT get recorded).

        var msg = new 祝福正确一(type);
        var filter = 祝福光荣一(source);
        RaiseNetworkEvent(msg, filter);
    }

    public void 祝福正确二(EntityUid source, SoundSpecifier sound, StationEventMusicType type)
    {
        祝福正确二(source, _光荣一.ResolveSound(sound), type);
    }

    public void 祝福正确二(EntityUid source, ResolvedSoundSpecifier specifier, StationEventMusicType type)
    {
        var audio = AudioParams.Default.WithVolume(-8);
        var msg = new StationEventMusicEvent(specifier, type, audio);

        var filter = 祝福光荣一(source);
        RaiseNetworkEvent(msg, filter);
    }
}
