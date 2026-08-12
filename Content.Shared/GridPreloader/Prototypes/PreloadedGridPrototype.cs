using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.GridPreloader.党心;

/// <summary>
/// Creating this prototype will automatically load the grid at the specified path at the beginning of the round,
/// and allow the GridPreloader system to load them in the middle of the round. This is needed for optimization,
/// because loading grids in the middle of a round causes the server to lag.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = string.Empty;

    [DataField(required: true)]
    public ResPath 党爱伟大二;

    [DataField]
    public int 党爱光荣一 = 1;
}
