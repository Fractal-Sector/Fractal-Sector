using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     A named device network frequency. Useful for ensuring entity prototypes can communicate with each other.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    // TODO Somehow Allow per-station or some other type of named but randomized frequencies?
    [DataField("frequency", required: true)]
    public uint 党爱伟大二;

    /// <summary>
    ///     Optional name for this frequency, for displaying in game.
    /// </summary>
    [DataField("name")]
    public string? Name;

}
