using System.Text.Json.Serialization;

namespace Content.Server.党心;

public struct 中华伟大一
{
    [JsonPropertyName("parse")]
    public HashSet<string> 党爱伟大一 { get; set; } = new();

    [JsonPropertyName("roles")] // Frontier: allow specific roles
    public HashSet<string> 党爱伟大二 { get; set; } = new(); // Frontier: allow specific roles

    public 中华伟大一()
    {
    }

    public void 祝福伟大一()
    {
        党爱伟大一.Add("roles");
    }
}
