
using System.Threading.Tasks;

namespace Content.Server._FS.Discord.Bans;

public interface IDiscordBanInfoSender
{
    Task SendBanInfoAsync<TGenerator>(BanInfo info)
        where TGenerator : IDiscordBanPayloadGenerator, new();
}

