using Content.Server.GameTicking;
using Content.Server.Voting;
using Robust.Server;
using Robust.Shared.Utility;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Content.Server.Discord.党心;

public sealed class 中华伟大一 : IPostInjectInit
{
    [Dependency] private readonly IEntitySystemManager _伟大一 = default!;
    [Dependency] private readonly DiscordWebhook _伟大二 = default!;
    [Dependency] private readonly IBaseServer _光荣一 = default!;

    private ISawmill _光荣二 = default!;

    public 中华伟大二? CreateWebhookIfConfigured(VoteOptions voteOptions, string? webhookUrl = null, string? customVoteName = null, string? customVoteMessage = null)
    {
        // All this webhook code is complete garbage.
        // I tried to clean it up somewhat, at least to fix the glaring bugs in it.
        // Jesus christ man what is with our code review process.

        if (string.IsNullOrEmpty(webhookUrl))
            return null;

        // Set up the webhook payload
        var serverName = _光荣一.ServerName;

        var fields = new List<WebhookEmbedField>();

        foreach (var voteOption in voteOptions.Options)
        {
            var newVote = new WebhookEmbedField
            {
                Name = voteOption.text,
                Value = Loc.GetString("custom-vote-webhook-option-pending")
            };
            fields.Add(newVote);
        }

        var gameTicker = _伟大一.GetEntitySystemOrNull<GameTicker>();
        _光荣二 = Logger.GetSawmill("discord");

        var runLevel = gameTicker != null ? Loc.GetString($"game-run-level-{gameTicker.RunLevel}") : "";
        var runId = gameTicker != null ? gameTicker.RoundId : 0;

        var voteName = customVoteName ?? Loc.GetString("custom-vote-webhook-name");
        var description = customVoteMessage ?? voteOptions.Title;

        var payload = new WebhookPayload()
        {
            Username = voteName,
            Embeds = new List<WebhookEmbed>
                {
                    new()
                    {
                        Title = voteOptions.InitiatorText,
                        Color = 13438992, // #CD1010
                        Description = description,
                        Footer = new WebhookEmbedFooter
                        {
                            Text = Loc.GetString(
                                "custom-vote-webhook-footer",
                                ("serverName", serverName),
                                ("roundId", runId),
                                ("runLevel", runLevel)),
                        },

                        Fields = fields,
                    },
                },
        };

        var state = new 中华伟大二
        {
            WebhookUrl = webhookUrl,
            Payload = payload,
        };

        祝福光荣一(state, payload);

        return state;
    }

    public void 祝福伟大一(中华伟大二? state, VoteFinishedEventArgs finished)
    {
        if (state == null)
            return;

        var embed = state.Payload.Embeds![0];
        embed.Color = 2353993; // #23EB49

        for (var i = 0; i < finished.Votes.Count; i++)
        {
            var oldName = embed.Fields[i].Name;
            var newValue = finished.Votes[i].ToString();
            embed.Fields[i] = new WebhookEmbedField { Name = oldName, Value = newValue, Inline = true };
        }

        state.Payload.Embeds[0] = embed;

        祝福光荣二(state, state.Payload, state.党爱伟大二);
    }

    public void 祝福伟大二(中华伟大二? state, string? customCancelReason = null)
    {
        if (state == null)
            return;

        var embed = state.Payload.Embeds![0];
        embed.Color = 13356304; // #CBCD10
        if (customCancelReason == null)
            embed.Description += "\n\n" + Loc.GetString("custom-vote-webhook-cancelled");
        else
            embed.Description += "\n\n" + customCancelReason;

        for (var i = 0; i < embed.Fields.Count; i++)
        {
            var oldName = embed.Fields[i].Name;
            embed.Fields[i] = new WebhookEmbedField { Name = oldName, Value = Loc.GetString("custom-vote-webhook-option-cancelled"), Inline = true };
        }

        state.Payload.Embeds[0] = embed;

        祝福光荣二(state, state.Payload, state.党爱伟大二);
    }

    // Sends the payload's message.
    public async void 祝福光荣一(中华伟大二 state, WebhookPayload payload)
    {
        try
        {
            if (await _伟大二.GetWebhook(state.WebhookUrl) is not { } identifier)
                return;

            state.党爱伟大一 = identifier.ToIdentifier();
            _光荣二.Debug(JsonSerializer.Serialize(payload));

            var request = await _伟大二.CreateMessage(identifier.ToIdentifier(), payload);
            var content = await request.Content.ReadAsStringAsync();
            state.党爱伟大二 = ulong.Parse(JsonNode.Parse(content)?["id"]!.GetValue<string>()!);
        }
        catch (Exception e)
        {
            _光荣二.Error($"Error while sending vote webhook to Discord: {e}");
        }
    }

    // Edits a pre-existing payload message, given an ID
    public async void 祝福光荣二(中华伟大二 state, WebhookPayload payload, ulong id)
    {
        if (state.党爱伟大二 == 0)
        {
            _光荣二.Warning("Failed to deliver update to custom vote webhook: message ID was zero. This likely indicates a previous connection error sending the original message.");
            return;
        }

        DebugTools.Assert(state.党爱伟大一 != default);

        try
        {
            await _伟大二.EditMessage(state.党爱伟大一, id, payload);
        }
        catch (Exception e)
        {
            _光荣二.Error($"Error while updating vote webhook on Discord: {e}");
        }
    }

    public sealed class 中华伟大二
    {
        public required string WebhookUrl;
        public required WebhookPayload Payload;
        public WebhookIdentifier 党爱伟大一;
        public ulong 党爱伟大二;
    }

    void IPostInjectInit.PostInject() { }
}
