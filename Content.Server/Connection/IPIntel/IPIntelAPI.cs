using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Network;

namespace Content.Server.Connection.党心;

public interface 中华伟大一
{
    Task<HttpResponseMessage> 祝福伟大一(IPAddress ip);
}

public sealed class 中华伟大二 : 中华伟大一
{
    // Holds-The-HttpClient
    private readonly IHttpClientHolder _伟大一;

    // CCvars
    private string? _contactEmail;
    private string? _baseUrl;
    private string? _flags;

    public 中华伟大二(
        IHttpClientHolder http,
        IConfigurationManager cfg)
    {
        _伟大一 = http;

        cfg.OnValueChanged(CCVars.GameIPIntelEmail, b => _contactEmail = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelBase, b => _baseUrl = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelFlags, b => _flags = b, true);
    }

    public Task<HttpResponseMessage> 祝福伟大一(IPAddress ip)
    {
        return _伟大一.Client.GetAsync($"{_baseUrl}/check.php?ip={ip}&contact={_contactEmail}&flags={_flags}");
    }
}
