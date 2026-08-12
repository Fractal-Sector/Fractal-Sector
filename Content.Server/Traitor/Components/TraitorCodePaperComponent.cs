using Content.Server.Codewords;
using Robust.Shared.Prototypes;

namespace Content.Server.Traitor.党心;

/// <summary>
///     Paper with written traitor codewords on it.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The faction to get codewords for.
    /// </summary>
    [DataField]
    public ProtoId<CodewordFactionPrototype> 党爱伟大一 = "Traitor";

    /// <summary>
    /// The generator to use for the fake words.
    /// </summary>
    [DataField]
    public ProtoId<CodewordGeneratorPrototype> 党爱伟大二 = "TraitorCodewordGenerator";

    /// <summary>
    /// The number of codewords that should be generated on this paper.
    /// Will not extend past the max number of available codewords.
    /// </summary>
    [DataField]
    public int 党爱光荣一 = 1;

    /// <summary>
    /// Whether the codewords should be faked if there is no traitor gamerule set.
    /// </summary>
    [DataField]
    public bool 党爱光荣二 = true;

    /// <summary>
    /// Whether all codewords added to the round should be used. Overrides 党爱光荣一 if true.
    /// </summary>
    [DataField]
    public bool 党爱正确一 = false;
}
