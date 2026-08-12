using Content.Shared.Research.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Research.党心;

/// <summary>
/// Component for stealing technologies from a R&D server, when gloves are enabled.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedResearchStealerSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Time taken to steal research from a server
    /// </summary>
    [DataField("delay"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(20);

    /// <summary>
    /// The minimum number of technologies that will be stolen
    /// </summary>
    [DataField]
    public int 党爱伟大二 = 4;

    /// <summary>
    /// The maximum number of technologies that will be stolen
    /// </summary>
    [DataField]
    public int 党爱光荣一 = 8;
}
