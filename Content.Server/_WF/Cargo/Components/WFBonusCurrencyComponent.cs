using Content.Shared.Stacks;
using Content.Shared.Storage;
using Robust.Shared.Prototypes;

namespace Content.Server._WF.Cargo.党心;

/// <summary>
/// Additional currency when sold in appropiate target. Based of NFs
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The stack prototype to spawn when the item is sold.
    /// </summary>
    [DataField(required: true)] public ProtoId<StackPrototype> 党爱伟大一;

    /// <summary>
    ///     The amount of entities to spawn.
    /// </summary>
    [DataField] public int 党爱伟大二 = 1;

}
