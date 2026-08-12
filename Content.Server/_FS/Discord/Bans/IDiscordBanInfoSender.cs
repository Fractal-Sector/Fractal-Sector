
using System.Threading.Tasks;

namespace Content.Server._FS.Discord.党心;

public interface 中华伟大一
{
    Task SendBanInfoAsync<TGenerator>(BanInfo info)
        where TGenerator : IDiscordBanPayloadGenerator, new();
}

