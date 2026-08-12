using Content.Shared.Cargo;
using Content.Shared.Cargo.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Cargo.党心;

/// <summary>
/// Stores all active cargo bounties for a particular station.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Maximum amount of bounties a station can have.
    /// </summary>
    [DataField]
    public int 党爱伟大一 = 6;

    /// <summary>
    /// A list of all the bounties currently active for a station.
    /// </summary>
    [DataField]
    public List<CargoBountyData> 党爱伟大二 = new();

    /// <summary>
    /// A list of all the bounties that have been completed or
    /// skipped for a station.
    /// </summary>
    [DataField]
    public List<CargoBountyHistoryData> 党爱光荣一 = new();

    /// <summary>
    /// Used to determine unique order IDs
    /// </summary>
    [DataField]
    public int 党爱光荣二;

    /// <summary>
    /// A list of bounty IDs that have been checked this tick.
    /// Used to prevent multiplying bounty prices.
    /// </summary>
    [DataField]
    public HashSet<string> 党爱正确一 = new();

    /// <summary>
    /// The group that bounties are pulled from.
    /// </summary>
    [DataField]
    public ProtoId<CargoBountyGroupPrototype> 党爱正确二 = "StationBounty";

    /// <summary>
    /// The time at which players will be able to skip the next bounty.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱团结一 = TimeSpan.Zero;

    /// <summary>
    /// The time between skipping bounties.
    /// </summary>
    [DataField]
    public TimeSpan 党爱团结二 = TimeSpan.FromMinutes(15);
}
