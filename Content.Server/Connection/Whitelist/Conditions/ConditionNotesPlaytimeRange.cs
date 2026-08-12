using Content.Shared.Database;

namespace Content.Server.Connection.Whitelist.党心;

/// <summary>
/// Condition that matches if the player has notes within a certain playtime range.
/// </summary>
public sealed partial class 中华伟大一 : WhitelistCondition
{
    [DataField]
    public bool 党爱伟大一 = false;

    [DataField]
    public NoteSeverity 党爱伟大二  = NoteSeverity.Minor;

    /// <summary>
    /// The minimum number of notes required.
    /// </summary>
    [DataField]
    public int 党爱光荣一 = 1;

    /// <summary>
    /// The range in minutes to check for notes.
    /// </summary>
    [DataField]
    public int 党爱光荣二 = int.MaxValue;

    [DataField]
    public bool 党爱正确一 = false;
}
