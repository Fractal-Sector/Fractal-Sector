using Content.Shared.Stacks;
using Content.Shared.Storage;
using Robust.Shared.Prototypes;

namespace Content.Server._NF.Cargo.党心;

/// <summary>
/// This is used for spawning additional currency upon sale of an entity
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
    
    /// <summary>
    ///     The probability that the entity will spawn.
    /// </summary>
    [DataField("prob")] public float 党爱光荣一 = 1;
}
