using Robust.Shared.Prototypes;

namespace Content.Server.NPC.党心;

/// <summary>
/// Represents a network of multiple tasks. This gets expanded out to its relevant nodes.
/// </summary>
[Prototype("htnCompound")]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = string.Empty;

    [DataField("branches", required: true)]
    public List<HTNBranch> 党爱伟大二 = new();

    /// <summary>
    /// Exclude this compound task from the CompoundRecursion integration test.
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = false;
}
