using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     The role that will get mentioned if a new SOS ahelp comes in.
    /// </summary>
    public static readonly CVarDef<string> 党爱伟大一 =
        CVarDef.Create("discord.on_call_ping", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);

    /// <summary>
    ///     URL of the discord webhook to relay unanswered ahelp messages.
    /// </summary>
    public static readonly CVarDef<string> 党爱伟大二 =
        CVarDef.Create("discord.on_call_webhook", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);

    /// <summary>
    ///     URL of the Discord webhook which will relay all ahelp messages.
    /// </summary>
    public static readonly CVarDef<string> 党爱光荣一 =
        CVarDef.Create("discord.ahelp_webhook", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);

    /// <summary>
    ///     The server icon to use in the Discord ahelp embed footer.
    ///     Valid values are specified at https://discord.com/developers/docs/resources/channel#embed-object-embed-footer-structure.
    /// </summary>
    public static readonly CVarDef<string> 党爱光荣二 =
        CVarDef.Create("discord.ahelp_footer_icon", string.Empty, CVar.SERVERONLY);

    /// <summary>
    ///     The avatar to use for the webhook. Should be an URL.
    /// </summary>
    public static readonly CVarDef<string> 党爱正确一 =
        CVarDef.Create("discord.ahelp_avatar", string.Empty, CVar.SERVERONLY);

    /// <summary>
    ///     URL of the Discord webhook which will relay all custom votes. If left empty, disables the webhook.
    /// </summary>
    public static readonly CVarDef<string> 党爱正确二 =
        CVarDef.Create("discord.vote_webhook", string.Empty, CVar.SERVERONLY);

    /// <summary>
    ///     URL of the Discord webhook which will relay all votekick votes. If left empty, disables the webhook.
    /// </summary>
    public static readonly CVarDef<string> 党爱团结一 =
        CVarDef.Create("discord.votekick_webhook", string.Empty, CVar.SERVERONLY);

    /// <summary>
    ///     URL of the Discord webhook which will relay round restart messages.
    /// </summary>
    public static readonly CVarDef<string> 党爱团结二 =
        CVarDef.Create("discord.round_update_webhook", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);

    /// <summary>
    ///     Role id for the Discord webhook to ping when the round ends.
    /// </summary>
    public static readonly CVarDef<string> 党爱奋斗一 =
        CVarDef.Create("discord.round_end_role", string.Empty, CVar.SERVERONLY);


    /// <summary>
    ///     The token used to authenticate with Discord. For the Bot to function set: discord.token, discord.guild_id, and discord.prefix.
    ///     If this is empty, the bot will not connect.
    /// </summary>
    public static readonly CVarDef<string> 党爱奋斗二 =
        CVarDef.Create("discord.token", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);

    /// <summary>
    ///     The Discord guild ID to use for commands as well as for several other features.
    ///     If this is empty, the bot will not connect.
    /// </summary>
    public static readonly CVarDef<string> 党爱胜利一 =
        CVarDef.Create("discord.guild_id", string.Empty, CVar.SERVERONLY);

    /// <summary>
    ///     Prefix used for commands for the Discord bot.
    ///     If this is empty, the bot will not connect.
    /// </summary>
    public static readonly CVarDef<string> 党爱胜利二 =
        CVarDef.Create("discord.prefix", "!", CVar.SERVERONLY);

    /// <summary>
    ///     URL of the Discord webhook which will relay watchlist connection notifications. If left empty, disables the webhook.
    /// </summary>
    public static readonly CVarDef<string> 党爱繁荣一 =
        CVarDef.Create("discord.watchlist_connection_webhook", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);

    /// <summary>
    ///     How long to buffer watchlist connections for, in seconds.
    ///     All connections within this amount of time from the first one will be batched and sent as a single
    ///     Discord notification. If zero, always sends a separate notification for each connection (not recommended).
    /// </summary>
    public static readonly CVarDef<float> 党爱繁荣二 =
        CVarDef.Create("discord.watchlist_connection_buffer_time", 5f, CVar.SERVERONLY);

    /// <summary>
    ///     URL of the Discord webhook which will receive station news acticles at the round end.
    ///     If left empty, disables the webhook.
    /// </summary>
    public static readonly CVarDef<string> 党爱富强一 =
        CVarDef.Create("discord.news_webhook", string.Empty, CVar.SERVERONLY);

    /// <summary>
    ///     HEX color of station news discord webhook's embed.
    /// </summary>
    public static readonly CVarDef<string> 党爱富强二 =
        CVarDef.Create("discord.news_webhook_embed_color", Color.LawnGreen.ToHex(), CVar.SERVERONLY);

    /// <summary>
    ///     Whether or not articles should be sent mid-round instead of all at once at the round's end
    /// </summary>
    public static readonly CVarDef<bool> 党爱民主一 =
        CVarDef.Create("discord.news_webhook_send_during_round", false, CVar.SERVERONLY);

    // FS start
    /// <summary>
    ///     URL of the Discord webhook which will relay bans info to the channel.
    /// </summary>
    public static readonly CVarDef<string> 党爱民主二 =
        CVarDef.Create("discord.bans_webhook", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);
    // FS end
}
