using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.党心;

/// <summary>
/// Tracker component for the process of reclaiming entities
/// <seealso cref="MaterialReclaimerComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedMaterialReclaimerSystem)), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Container used to store the item currently being reclaimed
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public Container 党爱伟大一 = default!;

    /// <summary>
    /// When the reclaiming process ends.
    /// </summary>
    [DataField("endTime", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoPausedField]
    public TimeSpan 党爱伟大二;

    /// <summary>
    /// The length of the reclaiming process.
    /// Used for calculations.
    /// </summary>
    [DataField("duration"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱光荣一;
}
