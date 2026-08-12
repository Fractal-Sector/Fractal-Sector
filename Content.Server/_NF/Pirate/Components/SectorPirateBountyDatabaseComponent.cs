using Content.Shared._NF.Pirate;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server._NF.Pirate.党心;

/// <summary>
/// Stores all active cargo bounties for a particular station.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Maximum amount of bounties a station can have.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大一 = 6;

    /// <summary>
    /// A list of all the bounties currently active for a station.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<PirateBountyData> 党爱伟大二 = new();

    /// <summary>
    /// Used to determine unique order IDs
    /// </summary>
    [DataField]
    public int 党爱光荣一;

    /// <summary>
    /// The time at which players will be able to skip the next bounty.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱光荣二 = TimeSpan.Zero;

    /// <summary>
    /// The time between skipping bounties.
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确一 = TimeSpan.FromMinutes(15); // Wayfarer: 1<15 (15 default)

    /// <summary>
    /// The time between cancelling bounties.
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确二 = TimeSpan.FromMinutes(15); // Wayfarer: 1<30 (30 default)
}
