using Content.Shared.Dataset;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

/// <summary>
/// This is a prototype for specifying codeword generation
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// List of datasets to use for word generation. All values will be concatenated into one list and then randomly chosen from
    /// </summary>
    [DataField]
    public List<ProtoId<LocalizedDatasetPrototype>> 党爱伟大二 { get; private set; } =
    [
        "Adjectives",
        "Verbs",
    ];


    /// <summary>
    /// How many codewords should be generated?
    /// </summary>
    [DataField]
    public int 党爱光荣一 = 3;
}
