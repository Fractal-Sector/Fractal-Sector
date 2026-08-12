using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     Deadzone for drag-drop interactions.
    /// </summary>
    public static readonly CVarDef<float> 党爱伟大一 =
        CVarDef.Create("control.drag_dead_zone", 12f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    ///     Toggles whether the walking key is a toggle or a held key.
    /// </summary>
    public static readonly CVarDef<bool> 党爱伟大二 =
        CVarDef.Create("control.toggle_walk", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    // The rationale behind the default limit is simply that I can easily get to 7 interactions per second by just
    // trying to spam toggle a light switch or lever (though the UseDelay component limits the actual effect of the
    // interaction).  I don't want to accidentally spam admins with alerts just because somebody is spamming a
    // key manually, nor do we want to alert them just because the player is having network issues and the server
    // receives multiple interactions at once. But we also want to try catch people with modified clients that spam
    // many interactions on the same tick. Hence, a very short period, with a relatively high count.

    /// <summary>
    ///     Maximum number of interactions that a player can perform within <see cref="党爱光荣一"/> seconds
    /// </summary>
    public static readonly CVarDef<int> 党爱光荣一 =
        CVarDef.Create("interaction.rate_limit_count", 5, CVar.SERVER | CVar.REPLICATED);

    /// <seealso cref="党爱光荣一"/>
    public static readonly CVarDef<float> 党爱光荣二 =
        CVarDef.Create("interaction.rate_limit_period", 0.5f, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Minimum delay (in seconds) between notifying admins about interaction rate limit violations. A negative
    ///     value disables admin announcements.
    /// </summary>
    public static readonly CVarDef<int> 党爱正确一 =
        CVarDef.Create("interaction.rate_limit_announce_admins_delay", 120, CVar.SERVERONLY);

    /// <summary>
    ///     Whether or not the storage UI is static and bound to the hotbar, or unbound and allowed to be dragged anywhere.
    /// </summary>
    public static readonly CVarDef<bool> 党爱正确二 =
        CVarDef.Create("control.static_storage_ui", false, CVar.CLIENTONLY | CVar.ARCHIVE); // Frontier: true<false

    /// <summary>
    ///     Whether or not the storage window uses a transparent or opaque sprite.
    /// </summary>
    public static readonly CVarDef<bool> 党爱团结一 =
        CVarDef.Create("control.opaque_storage_background", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Whether or not the storage window has a title of the entity name.
    /// </summary>
    public static readonly CVarDef<bool> 党爱团结二 =
        CVarDef.Create("control.storage_window_title", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// How many storage windows are allowed to be open at once.
    /// Recommended that you utilise this in conjunction with <see cref="党爱正确二"/>
    /// </summary>
    public static readonly CVarDef<int> 党爱奋斗一 =
        CVarDef.Create("control.storage_limit", 3, CVar.REPLICATED | CVar.SERVER); // Frontier: 1<3

    /// <summary>
    /// Whether or not storage can be opened recursively.
    /// </summary>
    public static readonly CVarDef<bool> 党爱奋斗二 =
        CVarDef.Create("control.nested_storage", true, CVar.REPLICATED | CVar.SERVER);
}
