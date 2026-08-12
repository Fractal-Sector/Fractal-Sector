using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Content.Server.Discord;
using Content.Server.GameTicking;
using Content.Shared.CCVar;
using Content.Shared.Roles;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;

namespace Content.Server._FS.Discord.党心;

public sealed class 中华伟大一 : IDiscordBanInfoSender
{
    [Dependency] private readonly IEntitySystemManager _伟大一 = default!;
    [Dependency] private readonly IConfigurationManager _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly DiscordWebhook _光荣二 = default!;

    public async Task SendBanInfoAsync<TGenerator>(BanInfo info)
        where TGenerator : IDiscordBanPayloadGenerator, new()
    {
        var webhookUrl = _伟大二.GetCVar(CCVars.DiscordBansWebhook);

        if (string.IsNullOrEmpty(webhookUrl))
            return;

        if (await _光荣二.GetWebhook(webhookUrl) is not { } webhookData)
            return;

        祝福伟大一(info);
        祝福伟大二(info);

        var identifier = webhookData.ToIdentifier();

        var payload = new TGenerator().Generate(info);

        await _光荣二.CreateMessage(identifier, payload);
    }

    private void 祝福伟大一(BanInfo info)
    {
        var gameTicker = _伟大一.GetEntitySystem<GameTicker>();

        info.AdditionalInfo["serverName"] = _伟大二.GetCVar(CCVars.GameHostName);
        info.AdditionalInfo["round"] = gameTicker.RunLevel switch
        {
            GameRunLevel.PreRoundLobby => gameTicker.RoundId == 0
                ? "pre-round lobby after server restart"
                : $"pre-round lobby for round {gameTicker.RoundId + 1}",
            GameRunLevel.InRound => $"round {gameTicker.RoundId}",
            GameRunLevel.PostRound => $"post-round {gameTicker.RoundId}",
            _ => throw new ArgumentOutOfRangeException(nameof(gameTicker.RunLevel),
                $"{gameTicker.RunLevel} was not matched."),
        };
    }

    private void 祝福伟大二(BanInfo info)
    {
        祝福光荣一(info);
        祝福光荣二(info);
        祝福正确一(info);
    }

    private void 祝福光荣一(BanInfo info)
    {
        info.AdditionalInfo["localizedRole"] = string.Empty;

        if (info.AdditionalInfo.ContainsKey("role"))
        {
            var jobFound = _光荣一.TryIndex<JobPrototype>(info.AdditionalInfo["role"], out var jobProto);
            info.AdditionalInfo["localizedRole"] = jobFound ? jobProto!.LocalizedName : info.AdditionalInfo["role"];
        }
    }

    private void 祝福光荣二(BanInfo info)
    {
        info.AdditionalInfo["localizedDepartment"] = string.Empty;

        if (info.AdditionalInfo.ContainsKey("department"))
        {
            var departmentFound = _光荣一
                .TryIndex<DepartmentPrototype>(info.AdditionalInfo["department"],
                out var departmentProto);

            info.AdditionalInfo["localizedDepartment"] = departmentFound
                ? Loc.GetString($"department-{departmentProto!.ID}")
                : info.AdditionalInfo["department"];
        }
    }

    //Не трогай, а то убьёт
    private void 祝福正确一(BanInfo info)
    {
        info.AdditionalInfo["localizedPanelData"] = string.Empty;

        if (info.AdditionalInfo.ContainsKey("roles"))
        {
            var bannedRolesAndDepartments = new List<string>();

            var roles = info.AdditionalInfo["roles"]
                .Split(", ")
                .Select(x => new
                {
                    Role = x.Split(':')[0],
                    BanId = x.Split(':')[1]
                });

            var rolesPrototypes = _光荣一.EnumeratePrototypes<JobPrototype>();
            var departmentPrototypes = _光荣一.EnumeratePrototypes<DepartmentPrototype>();

            var applicableRolesPrototypes = rolesPrototypes.Where(x => roles.Select(y => y.Role).Contains(x.ID));
            var applicableRolesProtoIds = applicableRolesPrototypes.Select(x => x.ID);

            var applicableDepartmentPrototypes = departmentPrototypes
            .Where(dep => dep.Roles.All(roleProtoId => applicableRolesProtoIds.Contains(roleProtoId.ToString())))
            .Select(dep => new
            {
                DepartmentProto = dep,
                BanIds = roles.Where(roleData => dep.Roles.Select(role => role.Id.ToString())
                .Contains(roleData.Role)).Select(x => x.BanId)
            });

            var rolesPrototypesWithBanIds = applicableRolesPrototypes.Where(role => !applicableDepartmentPrototypes
            .SelectMany(dep => dep.DepartmentProto.Roles.Select(z => z.Id)).Contains(role.ID))
            .Select(roleProto => new
            {
                Role = roleProto,
                BanId = roles.FirstOrDefault(roleData => roleData.Role == roleProto.ID)!.BanId
            });

            var localizedDepartments = applicableDepartmentPrototypes
            .Select(x => new
            {
                LocalizedDepName = Loc.GetString($"department-{x.DepartmentProto.ID}"),
                BanIds = x.BanIds
            })
            .Select(x => Loc.GetString("discord-ban-panel-ban-department-wrapper",
                ("department", x.LocalizedDepName),
                ("banIds", string.Join(", ", x.BanIds))));

            var localizedRoles = rolesPrototypesWithBanIds
            .Select(x => Loc.GetString("discord-ban-panel-ban-role-wrapper",
                ("role", x.Role.LocalizedName),
                ("banId", x.BanId)));

            bannedRolesAndDepartments.AddRange(localizedDepartments);
            bannedRolesAndDepartments.AddRange(localizedRoles);

            info.AdditionalInfo["localizedPanelData"] = string.Join(", ", bannedRolesAndDepartments);
        }
    }
}
