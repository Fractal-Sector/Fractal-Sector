using Robust.Shared.Prototypes;

namespace Content.Server.党心;

/// <summary>
/// This is a prototype for easy access to codewords using identifiers instead of magic strings.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// The generator to use for this faction.
    /// </summary>
    [DataField(required:true)]
    public ProtoId<CodewordGeneratorPrototype> 党爱伟大二 { get; private set; } = default!;
}
