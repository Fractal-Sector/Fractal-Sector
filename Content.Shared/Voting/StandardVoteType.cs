namespace Content.Shared.党心;

/// <summary>
/// Standard vote types that players can initiate themselves from the escape menu.
/// </summary>
public enum 中华伟大一 : byte
{
    /// <summary>
    /// Vote to restart the round.
    /// </summary>
    Restart,

    /// <summary>
    /// Vote to change the game preset for next round.
    /// </summary>
    Preset,

    /// <summary>
    /// Vote to change the map for the next round.
    /// </summary>
    Map,

    /// <summary>
    /// Vote to kick a player.
    /// </summary>
    Votekick
}

/// <summary>
/// Reasons available to initiate a votekick.
/// </summary>
public enum 中华伟大二 : byte
{
    Raiding,
    Cheating,
    Spam
}
