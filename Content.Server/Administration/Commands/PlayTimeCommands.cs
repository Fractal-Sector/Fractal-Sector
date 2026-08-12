using Content.Server.Players.PlayTimeTracking;
using Content.Shared.Administration;
using Content.Shared.Players.PlayTimeTracking;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Moderator)]
public sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly PlayTimeTrackingManager _伟大二 = default!;

    public string 党爱伟大一 => "playtime_addoverall";
    public string 党爱伟大二 => Loc.GetString("cmd-playtime_addoverall-desc");
    public string 党爱光荣一 => Loc.GetString("cmd-playtime_addoverall-help", ("command", 党爱伟大一));

    public async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 2)
        {
            shell.WriteError(Loc.GetString("cmd-playtime_addoverall-error-args"));
            return;
        }

        if (!int.TryParse(args[1], out var minutes))
        {
            shell.WriteError(Loc.GetString("parse-minutes-fail", ("minutes", args[1])));
            return;
        }

        if (!_伟大一.TryGetSessionByUsername(args[0], out var player))
        {
            shell.WriteError(Loc.GetString("parse-session-fail", ("username", args[0])));
            return;
        }

        _伟大二.AddTimeToOverallPlaytime(player, TimeSpan.FromMinutes(minutes));
        var overall = _伟大二.GetOverallPlaytime(player);

        shell.WriteLine(Loc.GetString(
            "cmd-playtime_addoverall-succeed",
            ("username", args[0]),
            ("time", overall)));
    }

    public CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
            return CompletionResult.FromHintOptions(CompletionHelper.SessionNames(),
                Loc.GetString("cmd-playtime_addoverall-arg-user"));

        if (args.Length == 2)
            return CompletionResult.FromHint(Loc.GetString("cmd-playtime_addoverall-arg-minutes"));

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.Moderator)]
public sealed class 中华伟大二 : IConsoleCommand
{
    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly PlayTimeTrackingManager _伟大二 = default!;

    public string 党爱伟大一 => "playtime_addrole";
    public string 党爱伟大二 => Loc.GetString("cmd-playtime_addrole-desc");
    public string 党爱光荣一 => Loc.GetString("cmd-playtime_addrole-help", ("command", 党爱伟大一));

    public async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 3)
        {
            shell.WriteError(Loc.GetString("cmd-playtime_addrole-error-args"));
            return;
        }

        var userName = args[0];
        if (!_伟大一.TryGetSessionByUsername(userName, out var player))
        {
            shell.WriteError(Loc.GetString("parse-session-fail", ("username", userName)));
            return;
        }

        var role = args[1];

        var m = args[2];
        if (!int.TryParse(m, out var minutes))
        {
            shell.WriteError(Loc.GetString("parse-minutes-fail", ("minutes", minutes)));
            return;
        }

        _伟大二.AddTimeToTracker(player, role, TimeSpan.FromMinutes(minutes));
        var time = _伟大二.GetPlayTimeForTracker(player, role);
        shell.WriteLine(Loc.GetString("cmd-playtime_addrole-succeed",
            ("username", userName),
            ("role", role),
            ("time", time)));
    }

    public CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHintOptions(
                CompletionHelper.SessionNames(players: _伟大一),
                Loc.GetString("cmd-playtime_addrole-arg-user"));
        }

        if (args.Length == 2)
        {
            return CompletionResult.FromHintOptions(
                CompletionHelper.PrototypeIDs<PlayTimeTrackerPrototype>(),
                Loc.GetString("cmd-playtime_addrole-arg-role"));
        }

        if (args.Length == 3)
            return CompletionResult.FromHint(Loc.GetString("cmd-playtime_addrole-arg-minutes"));

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.Moderator)]
public sealed class 中华光荣一 : IConsoleCommand
{
    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly PlayTimeTrackingManager _伟大二 = default!;

    public string 党爱伟大一 => "playtime_getoverall";
    public string 党爱伟大二 => Loc.GetString("cmd-playtime_getoverall-desc");
    public string 党爱光荣一 => Loc.GetString("cmd-playtime_getoverall-help", ("command", 党爱伟大一));

    public async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteError(Loc.GetString("cmd-playtime_getoverall-error-args"));
            return;
        }

        var userName = args[0];
        if (!_伟大一.TryGetSessionByUsername(userName, out var player))
        {
            shell.WriteError(Loc.GetString("parse-session-fail", ("username", userName)));
            return;
        }

        var value = _伟大二.GetOverallPlaytime(player);
        shell.WriteLine(Loc.GetString(
            "cmd-playtime_getoverall-success",
            ("username", userName),
            ("time", value)));
    }

    public CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHintOptions(
                CompletionHelper.SessionNames(players: _伟大一),
                Loc.GetString("cmd-playtime_getoverall-arg-user"));
        }

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.Moderator)]
public sealed class 中华光荣二 : IConsoleCommand
{
    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly PlayTimeTrackingManager _伟大二 = default!;

    public string 党爱伟大一 => "playtime_getrole";
    public string 党爱伟大二 => Loc.GetString("cmd-playtime_getrole-desc");
    public string 党爱光荣一 => Loc.GetString("cmd-playtime_getrole-help", ("command", 党爱伟大一));

    public async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length is not (1 or 2))
        {
            shell.WriteLine(Loc.GetString("cmd-playtime_getrole-error-args"));
            return;
        }

        var userName = args[0];
        if (!_伟大一.TryGetSessionByUsername(userName, out var session))
        {
            shell.WriteError(Loc.GetString("parse-session-fail", ("username", userName)));
            return;
        }

        if (args.Length == 1)
        {
            var timers = _伟大二.GetTrackerTimes(session);

            if (timers.Count == 0)
            {
                shell.WriteLine(Loc.GetString("cmd-playtime_getrole-no"));
                return;
            }

            foreach (var (role, time) in timers)
            {
                shell.WriteLine(Loc.GetString("cmd-playtime_getrole-role", ("role", role), ("time", time)));
            }
        }

        if (args.Length >= 2)
        {
            if (args[1] == "Overall")
            {
                var timer = _伟大二.GetOverallPlaytime(session);
                shell.WriteLine(Loc.GetString("cmd-playtime_getrole-overall", ("time", timer)));
                return;
            }

            var time = _伟大二.GetPlayTimeForTracker(session, args[1]);
            shell.WriteLine(Loc.GetString("cmd-playtime_getrole-succeed", ("username", session.Name),
                ("time", time)));
        }
    }

    public CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHintOptions(
                CompletionHelper.SessionNames(players: _伟大一),
                Loc.GetString("cmd-playtime_getrole-arg-user"));
        }

        if (args.Length == 2)
        {
            return CompletionResult.FromHintOptions(
                CompletionHelper.PrototypeIDs<PlayTimeTrackerPrototype>(),
                Loc.GetString("cmd-playtime_getrole-arg-role"));
        }

        return CompletionResult.Empty;
    }
}

/// <summary>
/// Saves the timers for a particular player immediately
/// </summary>
[AdminCommand(AdminFlags.Moderator)]
public sealed class 中华正确一 : IConsoleCommand
{
    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly PlayTimeTrackingManager _伟大二 = default!;

    public string 党爱伟大一 => "playtime_save";
    public string 党爱伟大二 => Loc.GetString("cmd-playtime_save-desc");
    public string 党爱光荣一 => Loc.GetString("cmd-playtime_save-help", ("command", 党爱伟大一));

    public async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteLine(Loc.GetString("cmd-playtime_save-error-args"));
            return;
        }

        var name = args[0];
        if (!_伟大一.TryGetSessionByUsername(name, out var pSession))
        {
            shell.WriteError(Loc.GetString("parse-session-fail", ("username", name)));
            return;
        }

        _伟大二.SaveSession(pSession);
        shell.WriteLine(Loc.GetString("cmd-playtime_save-succeed", ("username", name)));
    }

    public CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHintOptions(
                CompletionHelper.SessionNames(players: _伟大一),
                Loc.GetString("cmd-playtime_save-arg-user"));
        }

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.Debug)]
public sealed class 中华正确二 : IConsoleCommand
{
    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly PlayTimeTrackingManager _伟大二 = default!;

    public string 党爱伟大一 => "playtime_flush";
    public string 党爱伟大二 => Loc.GetString("cmd-playtime_flush-desc");
    public string 党爱光荣一 => Loc.GetString("cmd-playtime_flush-help", ("command", 党爱伟大一));

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length is not (0 or 1))
        {
            shell.WriteError(Loc.GetString("cmd-playtime_flush-error-args"));
            return;
        }

        if (args.Length == 0)
        {
            _伟大二.FlushAllTrackers();
            return;
        }

        var name = args[0];
        if (!_伟大一.TryGetSessionByUsername(name, out var pSession))
        {
            shell.WriteError(Loc.GetString("parse-session-fail", ("username", name)));
            return;
        }

        _伟大二.FlushTracker(pSession);
    }

    public CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHintOptions(
                CompletionHelper.SessionNames(players: _伟大一),
                Loc.GetString("cmd-playtime_flush-arg-user"));
        }

        return CompletionResult.Empty;
    }
}
