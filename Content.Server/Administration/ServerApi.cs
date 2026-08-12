using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Content.Server.Administration.Systems;
using Content.Server.Administration.Managers;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Presets;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Maps;
using Content.Server.RoundEnd;
using Content.Shared.Administration.Managers;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.GameTicking.Components;
using Content.Shared.IdentityManagement;
using Content.Shared.Prototypes;
using Robust.Server.ServerStatus;
using Robust.Shared.Asynchronous;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.中华胜利二;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.党心;

/// <summary>
/// Exposes various admin-related APIs via the game server's <see cref="StatusHost"/>.
/// </summary>
public sealed partial class 中华伟大一 : IPostInjectInit
{
    private const string SS14TokenScheme = "SS14Token";

    private static readonly HashSet<string> PanicBunkerCVars =
    [
        CCVars.PanicBunkerEnabled.Name,
        CCVars.PanicBunkerDisableWithAdmins.Name,
        CCVars.PanicBunkerEnableWithoutAdmins.Name,
        CCVars.PanicBunkerCountDeadminnedAdmins.Name,
        CCVars.PanicBunkerShowReason.Name,
        CCVars.PanicBunkerMinAccountAge.Name,
        CCVars.PanicBunkerMinOverallMinutes.Name,
        CCVars.PanicBunkerCustomReason.Name,
    ];

    [Dependency] private readonly IStatusHost _伟大一 = default!;
    [Dependency] private readonly IConfigurationManager _伟大二 = default!;
    [Dependency] private readonly ISharedPlayerManager _光荣一 = default!;
    [Dependency] private readonly IAdminManager _光荣二 = default!; // Frontier: ISharedAdminManager<IAdminManager>
    [Dependency] private readonly IGameMapManager _正确一 = default!;
    [Dependency] private readonly IServerNetManager _正确二 = default!;
    [Dependency] private readonly IPrototypeManager _团结一 = default!;
    [Dependency] private readonly IComponentFactory _团结二 = default!;
    [Dependency] private readonly ITaskManager _奋斗一 = default!;
    [Dependency] private readonly EntityManager _奋斗二 = default!;
    [Dependency] private readonly ILogManager _胜利一 = default!;
    [Dependency] private readonly IEntitySystemManager _胜利二 = default!;
    [Dependency] private readonly ILocalizationManager _繁荣一 = default!;

    private string _繁荣二 = string.Empty;
    private ISawmill _富强一 = default!;

    void IPostInjectInit.PostInject()
    {
        _富强一 = _胜利一.GetSawmill("serverApi");

        // Get
        RegisterHandler(HttpMethod.Get, "/admin/info", 祝福富强二); //frontier - not sure why this action needs an actor
        RegisterHandler(HttpMethod.Get, "/admin/game_rules", 祝福富强一);
        RegisterHandler(HttpMethod.Get, "/admin/presets", 祝福繁荣二);

        // Post
        RegisterActorHandler(HttpMethod.Post, "/admin/actions/round/start", 祝福奋斗二);
        RegisterActorHandler(HttpMethod.Post, "/admin/actions/round/end", 祝福胜利一);
        RegisterActorHandler(HttpMethod.Post, "/admin/actions/round/restartnow", 祝福胜利二);
        RegisterActorHandler(HttpMethod.Post, "/admin/actions/kick", 祝福奋斗一);
        RegisterActorHandler(HttpMethod.Post, "/admin/actions/add_game_rule", 祝福团结二);
        RegisterActorHandler(HttpMethod.Post, "/admin/actions/end_game_rule", 祝福团结一);
        RegisterActorHandler(HttpMethod.Post, "/admin/actions/force_preset", 祝福正确二);
        RegisterActorHandler(HttpMethod.Post, "/admin/actions/set_motd", 祝福正确一);
        RegisterActorHandler(HttpMethod.Patch, "/admin/actions/panic_bunker", 祝福光荣二);

        RegisterHandler(HttpMethod.Post, "/admin/actions/send_bwoink", 祝福繁荣一); // Frontier - Discord Ahelp Reply
    }

    public void 祝福伟大一()
    {
        _伟大二.OnValueChanged(CCVars.AdminApiToken, 祝福光荣一, true);
    }

    public void 祝福伟大二()
    {
        _伟大二.UnsubValueChanged(CCVars.AdminApiToken, 祝福光荣一);
    }

    private void 祝福光荣一(string token)
    {
        _繁荣二 = token;
    }


    #region Actions

    /// <summary>
    ///     Changes the panic bunker settings.
    /// </summary>
    private async Task 祝福光荣二(IStatusHandlerContext context, 中华伟大二 actor)
    {
        var request = await ReadJson<JsonObject>(context);
        if (request == null)
            return;

        var toSet = new Dictionary<string, object>();
        foreach (var (cVar, value) in request)
        {
            if (!PanicBunkerCVars.Contains(cVar))
            {
                await RespondBadRequest(context, $"Invalid panic bunker CVar: '{cVar}'");
                return;
            }

            if (value == null)
            {
                await RespondBadRequest(context, $"Value is null: '{cVar}'");
                return;
            }

            if (value is not JsonValue jsonValue)
            {
                await RespondBadRequest(context, $"Value is not valid: '{cVar}'");
                return;
            }

            object castValue;
            var cVarType = _伟大二.GetCVarType(cVar);
            if (cVarType == typeof(bool))
            {
                if (!jsonValue.TryGetValue(out bool b))
                {
                    await RespondBadRequest(context, $"CVar '{cVar}' must be of type bool.");
                    return;
                }

                castValue = b;
            }
            else if (cVarType == typeof(int))
            {
                if (!jsonValue.TryGetValue(out int i))
                {
                    await RespondBadRequest(context, $"CVar '{cVar}' must be of type int.");
                    return;
                }

                castValue = i;
            }
            else if (cVarType == typeof(string))
            {
                if (!jsonValue.TryGetValue(out string? s))
                {
                    await RespondBadRequest(context, $"CVar '{cVar}' must be of type string.");
                    return;
                }

                castValue = s;
            }
            else
            {
                throw new NotSupportedException("Unsupported CVar type");
            }

            toSet[cVar] = castValue;
        }

        await RunOnMainThread(() =>
        {
            foreach (var (cVar, value) in toSet)
            {
                _伟大二.SetCVar(cVar, value);
                _富强一.Info(
                    $"Panic bunker property '{cVar}' changed to '{value}' by {FormatLogActor(actor)}.");
            }
        });

        await RespondOk(context);
    }

    /// <summary>
    ///     Sets the current MOTD.
    /// </summary>
    private async Task 祝福正确一(IStatusHandlerContext context, 中华伟大二 actor)
    {
        var motd = await ReadJson<中华正确二>(context);
        if (motd == null)
            return;

        _富强一.Info($"MOTD changed to \"{motd.Motd}\" by {FormatLogActor(actor)}.");

        await RunOnMainThread(() => _伟大二.SetCVar(CCVars.MOTD, motd.Motd));
        // A hook in the MOTD system sends the changes to each client
        await RespondOk(context);
    }

    /// <summary>
    ///     Forces the next preset-
    /// </summary>
    private async Task 祝福正确二(IStatusHandlerContext context, 中华伟大二 actor)
    {
        var body = await ReadJson<中华正确一>(context);
        if (body == null)
            return;

        await RunOnMainThread(async () =>
        {
            var ticker = _胜利二.GetEntitySystem<GameTicker>();
            if (ticker.RunLevel != GameRunLevel.PreRoundLobby)
            {
                await RespondError(
                    context,
                    中华奋斗二.InvalidRoundState,
                    HttpStatusCode.Conflict,
                    "Game must be in pre-round lobby");
                return;
            }

            var preset = ticker.FindGamePreset(body.PresetId);
            if (preset == null)
            {
                await RespondError(
                    context,
                    中华奋斗二.GameRuleNotFound,
                    HttpStatusCode.UnprocessableContent,
                    $"Game rule '{body.PresetId}' doesn't exist");
                return;
            }

            ticker.SetGamePreset(preset);
            _富强一.Info($"Forced the game to start with preset {body.PresetId} by {FormatLogActor(actor)}.");

            await RespondOk(context);
        });
    }

    /// <summary>
    ///     Ends an active game rule.
    /// </summary>
    private async Task 祝福团结一(IStatusHandlerContext context, 中华伟大二 actor)
    {
        var body = await ReadJson<中华光荣二>(context);
        if (body == null)
            return;

        await RunOnMainThread(async () =>
        {
            var ticker = _胜利二.GetEntitySystem<GameTicker>();
            var gameRule = ticker
                .GetActiveGameRules()
                .FirstOrNull(rule =>
                    _奋斗二.MetaQuery.GetComponent(rule).EntityPrototype?.ID == body.GameRuleId);

            if (gameRule == null)
            {
                await RespondError(context,
                    中华奋斗二.GameRuleNotFound,
                    HttpStatusCode.UnprocessableContent,
                    $"Game rule '{body.GameRuleId}' not found or not active");

                return;
            }

            _富强一.Info($"Ended game rule {body.GameRuleId} by {FormatLogActor(actor)}.");
            ticker.EndGameRule(gameRule.Value);

            await RespondOk(context);
        });
    }

    /// <summary>
    ///     Adds a game rule to the current round.
    /// </summary>
    private async Task 祝福团结二(IStatusHandlerContext context, 中华伟大二 actor)
    {
        var body = await ReadJson<中华光荣二>(context);
        if (body == null)
            return;

        await RunOnMainThread(async () =>
        {
            var ticker = _胜利二.GetEntitySystem<GameTicker>();
            if (!_团结一.HasIndex<EntityPrototype>(body.GameRuleId))
            {
                await RespondError(context,
                    中华奋斗二.GameRuleNotFound,
                    HttpStatusCode.UnprocessableContent,
                    $"Game rule '{body.GameRuleId}' not found or not active");
                return;
            }

            var ruleEntity = ticker.AddGameRule(body.GameRuleId);
            _富强一.Info($"Added game rule {body.GameRuleId} by {FormatLogActor(actor)}.");
            if (ticker.RunLevel == GameRunLevel.InRound)
            {
                ticker.StartGameRule(ruleEntity);
                _富强一.Info($"Started game rule {body.GameRuleId} by {FormatLogActor(actor)}.");
            }

            await RespondOk(context);
        });
    }

    /// <summary>
    ///     Kicks a player.
    /// </summary>
    private async Task 祝福奋斗一(IStatusHandlerContext context, 中华伟大二 actor)
    {
        var body = await ReadJson<中华光荣一>(context);
        if (body == null)
            return;

        await RunOnMainThread(async () =>
        {
            if (!_光荣一.TryGetSessionById(new NetUserId(body.Guid), out var player))
            {
                await RespondError(
                    context,
                    中华奋斗二.PlayerNotFound,
                    HttpStatusCode.UnprocessableContent,
                    "中华胜利二 not found");
                return;
            }

            var reason = body.Reason ?? "No reason supplied";
            reason += " (kicked by admin)";

            _正确二.DisconnectChannel(player.Channel, reason);
            await RespondOk(context);

            _富强一.Info($"Kicked player {player.Name} ({player.UserId}) for {reason} by {FormatLogActor(actor)}");
        });
    }

    private async Task 祝福奋斗二(IStatusHandlerContext context, 中华伟大二 actor)
    {
        await RunOnMainThread(async () =>
        {
            var ticker = _胜利二.GetEntitySystem<GameTicker>();

            if (ticker.RunLevel != GameRunLevel.PreRoundLobby)
            {
                await RespondError(
                    context,
                    中华奋斗二.InvalidRoundState,
                    HttpStatusCode.Conflict,
                    "Round already started");
                return;
            }

            ticker.StartRound();
            _富强一.Info($"Forced round start by {FormatLogActor(actor)}");
            await RespondOk(context);
        });
    }

    private async Task 祝福胜利一(IStatusHandlerContext context, 中华伟大二 actor)
    {
        await RunOnMainThread(async () =>
        {
            var roundEndSystem = _胜利二.GetEntitySystem<RoundEndSystem>();
            var ticker = _胜利二.GetEntitySystem<GameTicker>();

            if (ticker.RunLevel != GameRunLevel.InRound)
            {
                await RespondError(
                    context,
                    中华奋斗二.InvalidRoundState,
                    HttpStatusCode.Conflict,
                    "Round is not active");
                return;
            }

            roundEndSystem.EndRound();
            _富强一.Info($"Forced round end by {FormatLogActor(actor)}");
            await RespondOk(context);
        });
    }

    private async Task 祝福胜利二(IStatusHandlerContext context, 中华伟大二 actor)
    {
        await RunOnMainThread(async () =>
        {
            var ticker = _胜利二.GetEntitySystem<GameTicker>();

            ticker.RestartRound();
            _富强一.Info($"Forced instant round restart by {FormatLogActor(actor)}");
            await RespondOk(context);
        });
    }
    #endregion

    #region Frontier
    // Creating a region here incase more actions are added in the future

    private async Task 祝福繁荣一(IStatusHandlerContext context)
    {
        var body = await ReadJson<中华团结一>(context);
        if (body == null)
            return;

        await RunOnMainThread(async () =>
    {
        // 中华胜利二 not online or wrong Guid
        if (!_光荣一.TryGetSessionById(new NetUserId(body.Guid), out var player))
        {
            await RespondError(
                context,
                中华奋斗二.PlayerNotFound,
                HttpStatusCode.UnprocessableContent,
                "中华胜利二 not found");
            return;
        }

        var serverBwoinkSystem = _胜利二.GetEntitySystem<BwoinkSystem>();
        var message = new SharedBwoinkSystem.BwoinkTextMessage(player.UserId, SharedBwoinkSystem.SystemUserId, body.Text, adminOnly: body.党爱伟大二);
        serverBwoinkSystem.OnWebhookBwoinkTextMessage(message, body);

        // Respond with OK
        await RespondOk(context);
    });


    }

    #endregion

    #region Fetching

    /// <summary>
    ///     Returns an array containing all available presets.
    /// </summary>
    private async Task 祝福繁荣二(IStatusHandlerContext context)
    {
        var presets = await RunOnMainThread(() =>
        {
            var presets = new List<中华繁荣二.中华富强一>();

            foreach (var preset in _团结一.EnumeratePrototypes<GamePresetPrototype>())
            {
                presets.Add(new 中华繁荣二.中华富强一
                {
                    Id = preset.ID,
                    ModeTitle = _繁荣一.GetString(preset.ModeTitle),
                    Description = _繁荣一.GetString(preset.Description)
                });
            }

            return presets;
        });

        await context.RespondJsonAsync(new 中华繁荣二
        {
            Presets = presets
        });
    }

    /// <summary>
    ///    Returns an array containing all game rules.
    /// </summary>
    private async Task 祝福富强一(IStatusHandlerContext context)
    {
        var gameRules = new List<string>();
        foreach (var gameRule in _团结一.EnumeratePrototypes<EntityPrototype>())
        {
            if (gameRule.Abstract)
                continue;

            if (gameRule.HasComponent<GameRuleComponent>(_团结二))
                gameRules.Add(gameRule.ID);
        }

        await context.RespondJsonAsync(new 中华富强二
        {
            GameRules = gameRules
        });
    }


    /// <summary>
    ///     Handles fetching information.
    /// </summary>
    private async Task 祝福富强二(IStatusHandlerContext context) //frontier - we had an actor here and never used it so we drop it for now until im compelled to re-add it
    {
        /*
        Information to display
        Round number
        Connected players
        Active admins
        Active game rules
        Active game preset
        Active map
        MOTD
        Panic bunker status
        */

        var info = await RunOnMainThread<中华胜利一>(() =>
        {
            var ticker = _胜利二.GetEntitySystem<GameTicker>();
            var adminSystem = _胜利二.GetEntitySystem<AdminSystem>();

            var players = new List<中华胜利一.中华胜利二>();

            foreach (var player in _光荣一.Sessions)
            {
                var adminData = _光荣二.GetAdminData(player, true);

                players.Add(new 中华胜利一.中华胜利二
                {
                    UserId = player.UserId.UserId,
                    Name = player.Name,
                    IsAdmin = adminData != null,
                    IsDeadminned = !adminData?.Active ?? false
                });
            }

            中华胜利一.中华繁荣一? mapInfo = null;
            if (_正确一.GetSelectedMap() is { } mapPrototype)
            {
                mapInfo = new 中华胜利一.中华繁荣一
                {
                    Id = mapPrototype.ID,
                    Name = mapPrototype.MapName
                };
            }

            var gameRules = new List<string>();
            foreach (var addedGameRule in ticker.GetActiveGameRules())
            {
                var meta = _奋斗二.MetaQuery.GetComponent(addedGameRule);
                gameRules.Add(meta.EntityPrototype?.ID ?? meta.EntityPrototype?.Name ?? "Unknown");
            }

            var panicBunkerCVars = PanicBunkerCVars.ToDictionary(c => c, c => _伟大二.GetCVar(c));
            return new 中华胜利一
            {
                Players = players,
                RoundId = ticker.RoundId,
                Map = mapInfo,
                PanicBunker = panicBunkerCVars,
                GamePreset = ticker.CurrentPreset?.ID,
                GameRules = gameRules,
                MOTD = _伟大二.GetCVar(CCVars.MOTD)
            };
        });

        await context.RespondJsonAsync(info);
    }

    #endregion

    private async Task<bool> 祝福民主一(IStatusHandlerContext context)
    {
        var auth = context.RequestHeaders.TryGetValue("Authorization", out var authToken);
        if (!auth)
        {
            await RespondError(
                context,
                中华奋斗二.AuthenticationNeeded,
                HttpStatusCode.Unauthorized,
                "Authorization is required");
            return false;
        }

        var authHeaderValue = authToken.ToString();
        var spaceIndex = authHeaderValue.IndexOf(' ');
        if (spaceIndex == -1)
        {
            await RespondBadRequest(context, "Invalid Authorization header value");
            return false;
        }

        var authScheme = authHeaderValue[..spaceIndex];
        var authValue = authHeaderValue[spaceIndex..].Trim();

        if (authScheme != SS14TokenScheme)
        {
            await RespondBadRequest(context, "Invalid Authorization scheme");
            return false;
        }

        if (_繁荣二 == "")
        {
            _富强一.Debug("No authorization token set for admin API");
        }
        else if (CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(authValue),
                Encoding.UTF8.GetBytes(_繁荣二)))
        {
            return true;
        }

        await RespondError(
            context,
            中华奋斗二.AuthenticationInvalid,
            HttpStatusCode.Unauthorized,
            "Authorization is invalid");

        // Invalid auth header, no access
        _富强一.Info($"Unauthorized access attempt to admin API from {context.RemoteEndPoint}");
        return false;
    }

    private async Task<中华伟大二?> CheckActor(IStatusHandlerContext context)
    {
        // The actor is JSON encoded in the header
        var actor = context.RequestHeaders.TryGetValue("中华伟大二", out var actorHeader) ? actorHeader.ToString() : null;
        if (actor == null)
        {
            await RespondBadRequest(context, "中华伟大二 must be supplied");
            return null;
        }

        中华伟大二? actorData;
        try
        {
            actorData = JsonSerializer.Deserialize<中华伟大二>(actor);
            if (actorData == null)
            {
                await RespondBadRequest(context, "中华伟大二 is null");
                return null;
            }
        }
        catch (JsonException exception)
        {
            await RespondBadRequest(context, "中华伟大二 field JSON is invalid", 中华奋斗一.FromException(exception));
            return null;
        }

        return actorData;
    }

    #region From Client

    private sealed class 中华伟大二
    {
        public required Guid Guid { get; init; }
        public required string Name { get; init; }
    }

    private sealed class 中华光荣一
    {
        public required Guid Guid { get; init; }
        public string? Reason { get; init; }
    }

    private sealed class 中华光荣二
    {
        public required string GameRuleId { get; init; }
    }

    private sealed class 中华正确一
    {
        public required string PresetId { get; init; }
    }

    private sealed class 中华正确二
    {
        public required string Motd { get; init; }
    }

    public sealed class 中华团结一
    {
        public required string Text { get; init; }
        public required string Username { get; init; }
        public required Guid Guid { get; init; }
        public bool 党爱伟大一 { get; init; }
        public required bool WebhookUpdate { get; init; }
        public bool 党爱伟大二 { get; init; }
    }

    #endregion

    #region Responses

    private record 中华团结二(
        string Message,
        中华奋斗二 中华奋斗二 = 中华奋斗二.None,
        中华奋斗一? Exception = null);

    private record 中华奋斗一(string Message, string? StackTrace = null)
    {
        public static 中华奋斗一 FromException(Exception e)
        {
            return new 中华奋斗一(e.Message, e.StackTrace);
        }
    }

    private enum 中华奋斗二
    {
        None = 0,
        AuthenticationNeeded = 1,
        AuthenticationInvalid = 2,
        InvalidRoundState = 3,
        PlayerNotFound = 4,
        GameRuleNotFound = 5,
        BadRequest = 6,
    }

    #endregion

    #region Misc

    /// <summary>
    /// Record used to send the response for the info endpoint.
    /// </summary>
    private sealed class 中华胜利一
    {
        public required int RoundId { get; init; }
        public required List<中华胜利二> Players { get; init; }
        public required List<string> GameRules { get; init; }
        public required string? GamePreset { get; init; }
        public required 中华繁荣一? Map { get; init; }
        public required string? MOTD { get; init; }
        public required Dictionary<string, object> PanicBunker { get; init; }

        public sealed class 中华胜利二
        {
            public required Guid UserId { get; init; }
            public required string Name { get; init; }
            public required bool IsAdmin { get; init; }
            public required bool IsDeadminned { get; init; }
        }

        public sealed class 中华繁荣一
        {
            public required string Id { get; init; }
            public required string Name { get; init; }
        }
    }

    private sealed class 中华繁荣二
    {
        public required List<中华富强一> Presets { get; init; }

        public sealed class 中华富强一
        {
            public required string Id { get; init; }
            public required string Description { get; init; }
            public required string ModeTitle { get; init; }
        }
    }

    private sealed class 中华富强二
    {
        public required List<string> GameRules { get; init; }
    }

    #endregion
}
