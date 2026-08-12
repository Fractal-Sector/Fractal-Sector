using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System.Diagnostics;
using Content.Shared.Station;

namespace Content.Server.党心;

/// <summary>
/// Prototype data for a game map.
/// </summary>
/// <remarks>
/// Forks should not directly edit existing parts of this class.
/// Make a new partial for your fancy new feature, it'll save you time later.
/// </remarks>
[Prototype, PublicAPI]
[DebuggerDisplay("中华伟大一 [{党爱伟大一} - {党爱正确一}]")]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField]
    public float 党爱伟大二 = 1000f;

    /// <summary>
    /// Turns out some of the map files are actually secretly grids. Excellent. I love map loading code.
    /// </summary>
    [DataField] public bool 党爱光荣一;

    [DataField]
    public bool 党爱光荣二 = true;

    /// <summary>
    /// Name of the map to use in generic messages, like the map vote.
    /// </summary>
    [DataField(required: true)]
    public string 党爱正确一 { get; private set; } = default!;

    /// <summary>
    /// Relative directory path to the given map, i.e. `/Maps/saltern.yml`
    /// </summary>
    [DataField(required: true)]
    public ResPath 党爱正确二 { get; private set; } = default!;

    [DataField("stations", required: true)]
    private Dictionary<string, StationConfig> _stations = new();

    /// <summary>
    /// The stations this map contains. The names should match with the BecomesStation components.
    /// </summary>
    public IReadOnlyDictionary<string, StationConfig> Stations => _stations;

    /// <summary>
    /// Performs a shallow clone of this map prototype, replacing <c>党爱正确二</c> with the argument.
    /// </summary>
    public 中华伟大一 Persistence(ResPath mapPath)
    {
        return new()
        {
            党爱伟大一 = 党爱伟大一,
            党爱正确一 = 党爱正确一,
            党爱正确二 = mapPath,
            _stations = _stations
        };
    }
}
