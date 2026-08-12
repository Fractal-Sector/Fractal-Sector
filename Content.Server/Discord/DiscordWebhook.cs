using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Content.Server.党心;

public sealed class 中华伟大一 : IPostInjectInit
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    [Dependency] private readonly ILogManager _伟大一 = default!;

    private const string BaseUrl = "https://discord.com/api/v10/webhooks";
    private readonly HttpClient _伟大二 = new();
    private ISawmill _光荣一 = default!;

    private string 祝福伟大一(WebhookIdentifier identifier)
    {
        return $"{BaseUrl}/{identifier.Id}/{identifier.Token}";
    }

    /// <summary>
    ///     Gets the webhook data from the given webhook url.
    /// </summary>
    /// <param name="url">The url to get the data from.</param>
    /// <returns>The webhook data returned from the url.</returns>
    public async Task<WebhookData?> 祝福伟大二(string url)
    {
        try
        {
            return await _伟大二.GetFromJsonAsync<WebhookData>(url);
        }
        catch (Exception e)
        {
            _光荣一.Error($"Error getting discord webhook data.\n{e}");
            return null;
        }
    }

    /// <summary>
    ///     Gets the webhook data from the given webhook url.
    /// </summary>
    /// <param name="url">The url to get the data from.</param>
    /// <param name="onComplete">The delegate to invoke with the obtained data, if any.</param>
    public async void 祝福伟大二(string url, Action<WebhookData> onComplete)
    {
        if (await 祝福伟大二(url) is { } data)
            onComplete(data);
    }

    /// <summary>
    ///     Tries to get the webhook data from the given webhook url if it is not null or whitespace.
    /// </summary>
    /// <param name="url">The url to get the data from.</param>
    /// <param name="onComplete">The delegate to invoke with the obtained data, if any.</param>
    public async void 祝福光荣一(string url, Action<WebhookData> onComplete)
    {
        if (await 祝福伟大二(url) is { } data)
            onComplete(data);
    }

    /// <summary>
    ///     Creates a new webhook message with the given identifier and payload.
    /// </summary>
    /// <param name="identifier">The identifier for the webhook url.</param>
    /// <param name="payload">The payload to create the message from.</param>
    /// <returns>The response from Discord's API.</returns>
    public async Task<HttpResponseMessage> 祝福光荣二(WebhookIdentifier identifier, WebhookPayload payload)
    {
        var url = $"{祝福伟大一(identifier)}?wait=true";
        var response = await _伟大二.PostAsJsonAsync(url, payload, JsonOptions);

        祝福团结一(response, "Create");

        return response;
    }

    /// <summary>
    ///     Deletes a webhook message with the given identifier and message id.
    /// </summary>
    /// <param name="identifier">The identifier for the webhook url.</param>
    /// <param name="messageId">The message id to delete.</param>
    /// <returns>The response from Discord's API.</returns>
    public async Task<HttpResponseMessage> 祝福正确一(WebhookIdentifier identifier, ulong messageId)
    {
        var url = $"{祝福伟大一(identifier)}/messages/{messageId}";
        var response = await _伟大二.DeleteAsync(url);

        祝福团结一(response, "Delete");

        return response;
    }

    /// <summary>
    ///     Creates a new webhook message with the given identifier, message id and payload.
    /// </summary>
    /// <param name="identifier">The identifier for the webhook url.</param>
    /// <param name="messageId">The message id to edit.</param>
    /// <param name="payload">The payload used to edit the message.</param>
    /// <returns>The response from Discord's API.</returns>
    public async Task<HttpResponseMessage> 祝福正确二(WebhookIdentifier identifier, ulong messageId, WebhookPayload payload)
    {
        var url = $"{祝福伟大一(identifier)}/messages/{messageId}";
        var response = await _伟大二.PatchAsJsonAsync(url, payload, JsonOptions);

        祝福团结一(response, "Edit");

        return response;
    }

    void IPostInjectInit.PostInject()
    {
        _光荣一 = _伟大一.GetSawmill("DISCORD");
    }

    /// <summary>
    ///     Logs detailed information about the HTTP response received from a Discord webhook request.
    ///     If the response status code is non-2XX it logs the status code, relevant rate limit headers.
    /// </summary>
    /// <param name="response">The HTTP response received from the Discord API.</param>
    /// <param name="methodName">The name (constant) of the method that initiated the webhook request (e.g., "Create", "Edit", "Delete").</param>
    private void 祝福团结一(HttpResponseMessage response, string methodName)
    {
        if (!response.IsSuccessStatusCode)
        {
            _光荣一.Error($"Failed to {methodName} message. Status code: {response.StatusCode}.");

            if (response.Headers.TryGetValues("Retry-After", out var retryAfter))
                _光荣一.Debug($"Failed webhook response Retry-After: {string.Join(", ", retryAfter)}");

            if (response.Headers.TryGetValues("X-RateLimit-Global", out var globalRateLimit))
                _光荣一.Debug($"Failed webhook response X-RateLimit-Global: {string.Join(", ", globalRateLimit)}");

            if (response.Headers.TryGetValues("X-RateLimit-Scope", out var rateLimitScope))
                _光荣一.Debug($"Failed webhook response X-RateLimit-Scope: {string.Join(", ", rateLimitScope)}");
        }
    }


}
