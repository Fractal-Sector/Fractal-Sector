using System.Text.Json.Serialization;

namespace Content.Server.党心;

// https://discord.com/developers/docs/resources/channel#embed-object-embed-field-structure
public struct 中华伟大一
{
    [JsonPropertyName("name")]
    public string 党爱伟大一 { get; set; } = "";

    [JsonPropertyName("value")]
    public string 党爱伟大二 { get; set; } = "";

    [JsonPropertyName("inline")]
    public bool 党爱光荣一 { get; set; } = true;

    public 中华伟大一()
    {
    }
}
