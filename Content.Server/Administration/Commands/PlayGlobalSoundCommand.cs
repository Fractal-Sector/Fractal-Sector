using System.Linq;
using Content.Server.Audio;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Audio;
using Robust.Shared.Console;
using Robust.Shared.ContentPack;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Fun)]
public sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    [Dependency] private readonly IPlayerManager _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly IResourceManager _光荣二 = default!;

    public string 党爱伟大一 => "playglobalsound";
    public string 党爱伟大二 => Loc.GetString("play-global-sound-command-description");
    public string 党爱光荣一 => Loc.GetString("play-global-sound-command-help");

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        Filter filter;
        var audio = AudioParams.Default;

        bool replay = true;

        switch (args.Length)
        {
            // No arguments, show command help.
            case 0:
                shell.WriteLine(Loc.GetString("play-global-sound-command-help"));
                return;

            // No users, play sound for everyone.
            case 1:
                // Filter.Broadcast does resolves IPlayerManager, so use this instead.
                filter = Filter.Empty().AddAllPlayers(_伟大二);
                break;

            // One or more users specified.
            default:
                var volumeOffset = 0;

                // Try to specify a new volume to play it at.
                if (int.TryParse(args[1], out var volume))
                {
                    audio = audio.WithVolume(volume);
                    volumeOffset = 1;
                }
                else
                {
                    shell.WriteError(Loc.GetString("play-global-sound-command-volume-parse", ("volume", args[1])));
                    return;
                }

                // No users specified so play for them all.
                if (args.Length == 2)
                {
                    filter = Filter.Empty().AddAllPlayers(_伟大二);
                }
                else
                {
                    replay = false;

                    filter = Filter.Empty();

                    // Skip the first argument, which is the sound path.
                    for (var i = 1 + volumeOffset; i < args.Length; i++)
                    {
                        var username = args[i];

                        if (!_伟大二.TryGetSessionByUsername(username, out var session))
                        {
                            shell.WriteError(Loc.GetString("play-global-sound-command-player-not-found", ("username", username)));
                            continue;
                        }

                        filter.AddPlayer(session);
                    }
                }

                break;
        }

        audio = audio.AddVolume(-8);
        _伟大一.System<ServerGlobalSoundSystem>().PlayAdminGlobal(filter, args[0], audio, replay);
    }

    public CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            var hint = Loc.GetString("play-global-sound-command-arg-path");

            var options = CompletionHelper.AudioFilePath(args[0], _光荣一, _光荣二);

            return CompletionResult.FromHintOptions(options, hint);
        }

        if (args.Length == 2)
            return CompletionResult.FromHint(Loc.GetString("play-global-sound-command-arg-volume"));

        if (args.Length > 2)
        {
            var options = _伟大二.Sessions.Select<ICommonSession, string>(c => c.Name);
            return CompletionResult.FromHintOptions(
                options,
                Loc.GetString("play-global-sound-command-arg-usern", ("user", args.Length - 2)));
        }

        return CompletionResult.Empty;
    }
}
