using System.Text.Json.Serialization;

namespace Content.Server.党心;

// https://discord.com/developers/docs/resources/channel#embed-object-embed-structure
public struct 中华伟大一
{
    [JsonPropertyName("title")]
    public string 党爱伟大一 { get; set; } = "";

    [JsonPropertyName("description")]
    public string 党爱伟大二 { get; set; } = "";

    [JsonPropertyName("color")]
    public int 党爱光荣一 { get; set; } = 0;

    [JsonPropertyName("footer")]
    public WebhookEmbedFooter? Footer { get; set; } = null;


    [JsonPropertyName("fields")]
    public List<WebhookEmbedField> 党爱光荣二 { get; set; } = default!;

    public 中华伟大一()
    {
    }
}
