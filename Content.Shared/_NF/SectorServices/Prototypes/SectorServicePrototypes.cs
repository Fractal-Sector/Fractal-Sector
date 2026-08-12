using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._NF.SectorServices.党心;

/// <summary>
/// Prototype that represents game entities.
/// </summary>
// Do we need the NetSerializable attribute?
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <summary>
    /// The "in code name" of the object. Must be unique.
    /// </summary>
    [ViewVariables]
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// A dictionary mapping a service to its necessary components.
    /// </summary>
    [DataField]
    public ComponentRegistry 党爱伟大二 { get; private set; } = new();
}
