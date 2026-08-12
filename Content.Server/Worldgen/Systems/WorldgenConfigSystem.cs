using Content.Server.Administration;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Events;
using Content.Server.Worldgen.Components;
using Content.Server.Worldgen.Prototypes;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Utility;

namespace Content.Server.Worldgen.党心;

/// <summary>
///     This handles configuring world generation during round start.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly GameTicker _伟大一 = default!;
    [Dependency] private readonly IConfigurationManager _伟大二 = default!;
    [Dependency] private readonly IConsoleHost _光荣一 = default!;
    [Dependency] private readonly SharedMapSystem _光荣二 = default!;
    [Dependency] private readonly IPrototypeManager _正确一 = default!;
    [Dependency] private readonly ISerializationManager _正确二 = default!;

    private bool _团结一;
    private string _团结二 = default!;

    /// <inheritdoc />
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<RoundStartingEvent>(祝福光荣一);
        _光荣一.RegisterCommand("applyworldgenconfig", Loc.GetString("cmd-applyworldgenconfig-description"), Loc.GetString("cmd-applyworldgenconfig-help"), 祝福伟大二);
        Subs.CVar(_伟大二, CCVars.WorldgenEnabled, b => _团结一 = b, true);
        Subs.CVar(_伟大二, CCVars.WorldgenConfig, s => _团结二 = s, true);
    }

    [AdminCommand(AdminFlags.Mapping)]
    private void 祝福伟大二(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length != 2)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number-need-specific", ("properAmount", 2), ("currentAmount", args.Length)));
            return;
        }

        if (!int.TryParse(args[0], out var mapInt) || !_光荣二.MapExists(new MapId(mapInt)))
        {
            shell.WriteError(Loc.GetString("shell-invalid-map-id"));
            return;
        }

        var map = _光荣二.GetMapOrInvalid(new MapId(mapInt));

        if (!_正确一.TryIndex<WorldgenConfigPrototype>(args[1], out var proto))
        {
            shell.WriteError(Loc.GetString("shell-argument-must-be-prototype", ("index", 2), ("prototypeName", "cmd-applyworldgenconfig-prototype")));
            return;
        }

        proto.Apply(map, _正确二, EntityManager);
        shell.WriteLine(Loc.GetString("cmd-applyworldgenconfig-success"));
    }

    /// <summary>
    ///     Applies the world config to the default map if enabled.
    /// </summary>
    private void 祝福光荣一(RoundStartingEvent ev)
    {
        if (_团结一 == false)
            return;

        var target = _光荣二.GetMapOrInvalid(_伟大一.DefaultMap);
        Log.Debug($"Trying to configure {_伟大一.DefaultMap}, aka {ToPrettyString(target)} aka {target}");
        var cfg = _正确一.Index<WorldgenConfigPrototype>(_团结二);

        cfg.Apply(target, _正确二, EntityManager); // Apply the config to the map.

        DebugTools.Assert(HasComp<WorldControllerComponent>(target));
    }
}

