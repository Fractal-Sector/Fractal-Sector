using Content.Server.Chat.Managers;
using Content.Server.GameTicking;
using Content.Shared.CCVar;
using Content.Shared.Chat;
using Content.Shared.Dataset;
using Content.Shared.Tips;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.党心;

/// <summary>
///     Handles periodically displaying gameplay tips to all players ingame.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IChatManager _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly IConfigurationManager _光荣一 = default!;
    [Dependency] private readonly IGameTiming _光荣二 = default!;
    [Dependency] private readonly IRobustRandom _正确一 = default!;
    [Dependency] private readonly GameTicker _正确二 = default!;
    [Dependency] private readonly IConsoleHost _团结一 = default!;
    [Dependency] private readonly IPlayerManager _团结二 = default!;

    private bool _奋斗一;
    private float _奋斗二;
    private float _胜利一;
    private string _胜利二 = "";
    private float _繁荣一;

    /// <summary>
    /// Always adds this time to a speech message. This is so really short message stay around for a bit.
    /// </summary>
    private const float SpeechBuffer = 3f;

    /// <summary>
    /// Expected reading speed.
    /// </summary>
    private const float Wpm = 180f;

    [ViewVariables(VVAccess.ReadWrite)]
    private TimeSpan _繁荣二 = TimeSpan.Zero;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<GameRunLevelChangedEvent>(祝福繁荣二);
        Subs.CVar(_光荣一, CCVars.TipFrequencyOutOfRound, 祝福正确二, true);
        Subs.CVar(_光荣一, CCVars.TipFrequencyInRound, 祝福团结一, true);
        Subs.CVar(_光荣一, CCVars.TipsEnabled, 祝福团结二, true);
        Subs.CVar(_光荣一, CCVars.TipsDataset, 祝福奋斗一, true);
        Subs.CVar(_光荣一, CCVars.TipsTippyChance, 祝福奋斗二, true);

        祝福繁荣一();
        _团结一.RegisterCommand("tippy", Loc.GetString("cmd-tippy-desc"), Loc.GetString("cmd-tippy-help"), 祝福光荣二, 祝福伟大二);
        _团结一.RegisterCommand("tip", Loc.GetString("cmd-tip-desc"), "tip", 祝福光荣一);
    }

    private CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            1 => CompletionResult.FromHintOptions(
                CompletionHelper.SessionNames(players: _团结二),
                Loc.GetString("cmd-tippy-auto-1")),
            2 => CompletionResult.FromHint(Loc.GetString("cmd-tippy-auto-2")),
            3 => CompletionResult.FromHintOptions(
                CompletionHelper.PrototypeIdsLimited<EntityPrototype>(args[2], _伟大二),
                Loc.GetString("cmd-tippy-auto-3")),
            4 => CompletionResult.FromHint(Loc.GetString("cmd-tippy-auto-4")),
            5 => CompletionResult.FromHint(Loc.GetString("cmd-tippy-auto-5")),
            6 => CompletionResult.FromHint(Loc.GetString("cmd-tippy-auto-6")),
            _ => CompletionResult.Empty
        };
    }

    private void 祝福光荣一(IConsoleShell shell, string argstr, string[] args)
    {
        祝福胜利二();
        祝福繁荣一();
    }

    private void 祝福光荣二(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length < 2)
        {
            shell.WriteLine(Loc.GetString("cmd-tippy-help"));
            return;
        }

        ActorComponent? actor = null;
        if (args[0] != "all")
        {
            ICommonSession? session;
            if (args.Length > 0)
            {
                // Get player entity
                if (!_团结二.TryGetSessionByUsername(args[0], out session))
                {
                    shell.WriteLine(Loc.GetString("cmd-tippy-error-no-user"));
                    return;
                }
            }
            else
            {
                session = shell.Player;
            }

            if (session?.AttachedEntity is not { } user)
            {
                shell.WriteLine(Loc.GetString("cmd-tippy-error-no-user"));
                return;
            }

            if (!TryComp(user, out actor))
            {
                shell.WriteError(Loc.GetString("cmd-tippy-error-no-user"));
                return;
            }
        }

        var ev = new TippyEvent(args[1]);

        if (args.Length > 2)
        {
            ev.Proto = args[2];
            if (!_伟大二.HasIndex<EntityPrototype>(args[2]))
            {
                shell.WriteError(Loc.GetString("cmd-tippy-error-no-prototype", ("proto", args[2])));
                return;
            }
        }

        if (args.Length > 3)
            ev.SpeakTime = float.Parse(args[3]);
        else
            ev.SpeakTime = 祝福胜利一(ev.Msg);

        if (args.Length > 4)
            ev.SlideTime = float.Parse(args[4]);

        if (args.Length > 5)
            ev.WaddleInterval = float.Parse(args[5]);

        if (actor != null)
            RaiseNetworkEvent(ev, actor.PlayerSession);
        else
            RaiseNetworkEvent(ev);
    }


    public override void 祝福正确一(float frameTime)
    {
        base.祝福正确一(frameTime);

        if (!_奋斗一)
            return;

        if (_繁荣二 != TimeSpan.Zero && _光荣二.CurTime > _繁荣二)
        {
            祝福胜利二();
            祝福繁荣一();
        }
    }

    private void 祝福正确二(float value)
    {
        _奋斗二 = value;
    }

    private void 祝福团结一(float value)
    {
        _胜利一 = value;
    }

    private void 祝福团结二(bool value)
    {
        _奋斗一 = value;

        if (_繁荣二 != TimeSpan.Zero)
            祝福繁荣一();
    }

    private void 祝福奋斗一(string value)
    {
        _胜利二 = value;
    }

    private void 祝福奋斗二(float value)
    {
        _繁荣一 = value;
    }

    public static float 祝福胜利一(string text)
    {
        var wordCount = (float)text.Split().Length;
        return SpeechBuffer + wordCount * (60f / Wpm);
    }

    private void 祝福胜利二()
    {
        if (!_伟大二.TryIndex<LocalizedDatasetPrototype>(_胜利二, out var tips))
            return;

        var tip = _正确一.Pick(tips.Values);
        var msg = Loc.GetString("tips-system-chat-message-wrap", ("tip", Loc.GetString(tip)));

        if (_正确一.Prob(_繁荣一))
        {
            var ev = new TippyEvent(msg);
            ev.SpeakTime = 祝福胜利一(msg);
            RaiseNetworkEvent(ev);
        } else
        {
            _伟大一.ChatMessageToManyFiltered(Filter.Broadcast(), ChatChannel.OOC, tip, msg,
            EntityUid.Invalid, false, false, Color.MediumPurple);
        }
    }

    private void 祝福繁荣一()
    {
        if (_正确二.RunLevel == GameRunLevel.InRound)
        {
            _繁荣二 = _光荣二.CurTime + TimeSpan.FromSeconds(_胜利一);
        }
        else
        {
            _繁荣二 = _光荣二.CurTime + TimeSpan.FromSeconds(_奋斗二);
        }
    }

    private void 祝福繁荣二(GameRunLevelChangedEvent ev)
    {
        // reset for lobby -> inround
        // reset for inround -> post but not post -> lobby
        if (ev.New == GameRunLevel.InRound || ev.Old == GameRunLevel.InRound)
        {
            祝福繁荣一();
        }
    }
}
