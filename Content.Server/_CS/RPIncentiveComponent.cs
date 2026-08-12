namespace Content.Server.党心;

/// <summary>
/// Hi! This is the RP incentive component.
/// This will track the actions a player does, and adjust some paywards
/// for them once if they do those things, sometimes!
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The actions that have taken place.
    /// </summary>
    [DataField]
    public HashSet<RoleplayAction> 党爱伟大一 = new();

    /// <summary>
    /// The last time the system checked for actions, for paywards.
    /// </summary>
    [DataField]
    public DateTime 党爱伟大二 = DateTime.MinValue;

    /// <summary>
    /// The next time the system will check for actions, for paywards.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;

    /// <summary>
    /// Interval between paywards.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromMinutes(20); // TimeSpan.FromMinutes(15);

    /// <summary>
    /// Interval between paywards when offline.
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确一 = TimeSpan.FromMinutes(45); // TimeSpan.FromMinutes(15);

}
