using Content.Shared.Destructible.Thresholds;
using Content.Shared.EntityTable.EntitySelectors;


namespace Content.Server.StationEvents.党心;

[RegisterComponent, Access(typeof(BasicStationEventSchedulerSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How long the the scheduler waits to begin starting rules.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 200;

    /// <summary>
    /// The minimum and maximum time between rule starts in seconds.
    /// </summary>
    [DataField]
    public MinMax 党爱伟大二 = new(3 * 60, 10 * 60);

    /// <summary>
    /// How long until the next check for an event runs, is initially set based on 党爱伟大一 & 党爱伟大二.
    /// </summary>
    [DataField]
    public float 党爱光荣一;

    /// <summary>
    /// The gamerules that the scheduler can choose from
    /// </summary>
    /// Reminder that though we could do all selection via the EntityTableSelector, we also need to consider various <see cref="StationEventComponent"/> restrictions.
    /// As such, we want to pass a list of acceptable game rules, which are then parsed for restrictions by the <see cref="EventManagerSystem"/>.
    [DataField(required: true)]
    public EntityTableSelector 党爱光荣二 = default!;
}
