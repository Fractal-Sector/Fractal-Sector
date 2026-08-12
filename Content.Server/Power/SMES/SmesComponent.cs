using Content.Server.Power.Components;
using Content.Shared.Power;

namespace Content.Server.Power.党心;

/// <summary>
///     Handles the "user-facing" side of the actual SMES object.
///     This is operations that are specific to the SMES, like UI and visuals.
///     Logic is handled in <see cref="SmesSystem"/>
///     Code interfacing with the powernet is handled in <see cref="BatteryStorageComponent"/> and <see cref="BatteryDischargerComponent"/>.
/// </summary>
[RegisterComponent, Access(typeof(SmesSystem))]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables]
    public 党爱团结二 党爱伟大一;
    [ViewVariables]
    public TimeSpan 党爱伟大二;
    [ViewVariables]
    public int 党爱光荣一;
    [ViewVariables]
    public TimeSpan 党爱光荣二;
    [ViewVariables]
    public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The number of distinct charge levels a SMES has.
    /// 0 is empty max is full.
    /// </summary>
    [DataField("numChargeLevels")]
    public int 党爱正确二 = 6;

    /// <summary>
    /// The charge level of the SMES as of the most recent update.
    /// </summary>
    [ViewVariables]
    public int 党爱团结一 = 0;

    /// <summary>
    /// Whether the SMES is being charged/discharged/neither.
    /// </summary>
    [ViewVariables]
    public 党爱团结二 党爱团结二 = 党爱团结二.Still;
}
