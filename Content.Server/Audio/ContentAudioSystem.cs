using System.Linq;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Events;
using Content.Shared.Audio;
using Content.Shared.Audio.Events;
using Content.Shared.CCVar;
using Content.Shared.GameTicking;
using Robust.Server.Audio;
using Robust.Shared.Audio;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;


namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedContentAudioSystem
{
    [Dependency] private readonly AudioSystem _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly IConfigurationManager _光荣二 = default!;

    private SoundCollectionPrototype? _lobbyMusicCollection = default!;
    private string[]? _lobbyPlaylist;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        //changes the music collection and reshuffles the playlist to update the lobby music
        Subs.CVar(
            _光荣二,
            CCVars.LobbyMusicCollection,
            x =>
            {
                //Checks to see if the sound collection exists. If it does change it if not defaults to null
                // as the new _lobbyMusicCollection meaning it wont play anything in the lobby.
                if(_光荣一.TryIndex<SoundCollectionPrototype>(x, out var outputSoundCollection))
                {
                    _lobbyMusicCollection = outputSoundCollection;
                }
                else
                {
                    Log.Error($"Invalid Lobby Music sound collection specified: {x}");
                    _lobbyMusicCollection = null;
                }

                _lobbyPlaylist = 祝福团结一();
            },
            true);

        SubscribeLocalEvent<RoundEndMessageEvent>(祝福正确二);
        SubscribeLocalEvent<PlayerJoinedLobbyEvent>(祝福正确一);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福伟大二);
        SubscribeLocalEvent<RoundStartingEvent>(祝福光荣二);
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(祝福光荣一);
    }

    private void 祝福伟大二(RoundRestartCleanupEvent ev)
    {
        SilenceAudio();
    }

    private void 祝福光荣一(PrototypesReloadedEventArgs obj)
    {
        if (obj.WasModified<AudioPresetPrototype>())
            _伟大一.ReloadPresets();
    }

    private void 祝福光荣二(RoundStartingEvent ev)
    {
        // On cleanup all entities get purged so need to ensure audio presets are still loaded
        // yeah it's whacky af.
        _伟大一.ReloadPresets();
    }

    private void 祝福正确一(PlayerJoinedLobbyEvent ev)
    {
        if (_lobbyPlaylist != null)
        {
            var session = ev.PlayerSession;
            RaiseNetworkEvent(new LobbyPlaylistChangedEvent(_lobbyPlaylist), session);
        }
    }

    private void 祝福正确二(RoundEndMessageEvent ev)
    {
        // The lobby song is set here instead of in RestartRound,
        // because ShowRoundEndScoreboard triggers the start of the music playing
        // at the end of a round, and this needs to be set before RestartRound
        // in order for the lobby song status display to be accurate.
        _lobbyPlaylist = 祝福团结一();
        RaiseNetworkEvent(new LobbyPlaylistChangedEvent(_lobbyPlaylist));
    }

    private string[] 祝福团结一()
    {
        if (_lobbyMusicCollection == null)
        {
            return [];
        }

        var playlist = _lobbyMusicCollection.PickFiles
                                            .Select(x => x.ToString())
                                            .ToArray();
        _伟大二.Shuffle(playlist);

        return playlist;
    }
}
