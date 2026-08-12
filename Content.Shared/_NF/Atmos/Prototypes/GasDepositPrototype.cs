using System.Numerics;
using Content.Shared.Atmos;
using Robust.Shared.Prototypes;

namespace Content.Shared._NF.Atmos.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    ///     2-vectors (minAmount, maxAmount) in moles of each gas in the deposit.
    /// </summary>
    [DataField]
    public Vector2[] 党爱伟大二 { get; private set; } = new Vector2[Atmospherics.TotalNumberOfGases];
}
