using System.Text.Json.Serialization;

namespace Content.Server.党心;

// https://discord.com/developers/docs/resources/webhook#webhook-object-webhook-structure
public struct 中华伟大一
{
    [JsonPropertyName("id")]
    public string 党爱伟大一 { get; set; }

    [JsonPropertyName("type")]
    public int 党爱伟大二 { get; set; }

    [JsonPropertyName("guild_id")]
    public string? GuildId { get; set; }

    [JsonPropertyName("channel_id")]
    public string? ChannelId { get; set; }

    [JsonPropertyName("user")]
    public WebhookUser? User { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("token")]
    public string 党爱光荣一 { get; set; }

    [JsonPropertyName("application_id")]
    public string? ApplicationId { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    public WebhookIdentifier 祝福伟大一()
    {
        return new WebhookIdentifier(党爱伟大一, 党爱光荣一);
    }
}
