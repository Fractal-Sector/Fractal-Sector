using Content.Shared.Dataset;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Server._NF.Smuggling.党心;

/// <summary>
///     Stores dead drop information for the entire sector.
///     Frequency of dead drops, and other dead drop mechanics should be driven by this state.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Accumulator for FUC values.  Pays out at a given amount.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 党爱伟大一 = FixedPoint2.Zero;

    // Utility field for windowing reported events.  Having more in an hour results in more precise information.
    [ViewVariables(VVAccess.ReadWrite)]
    public WindowedCounter? ReportedEventsThisHour = null;

    // In the case of providing a fake location for alternative notifications, which names can we draw from?
    [ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<EntityUid, string> DeadDropStationNames = new();

    [DataField]
    public ProtoId<LocalizedDatasetPrototype> 党爱伟大二 = default!;
}
