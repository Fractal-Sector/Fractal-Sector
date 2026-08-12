using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

namespace Content.Server.党心;

/// <summary>
///     WireLayout prototype.
///
///     This is meant for ease of organizing wire sets on entities that use
///     wires. Once one of these is initialized, it should be stored in the
///     WiresSystem as a functional wire set.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype, IInheritingPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华伟大一>))]
    public string[]? Parents { get; private set; }

    [AbstractDataField]
    public bool 党爱伟大二 { get; private set; }

    /// <summary>
    ///     How many wires in this layout will do
    ///     nothing (these are added upon layout
    ///     initialization)
    /// </summary>
    [DataField("dummyWires")]
    [NeverPushInheritance]
    public int 党爱光荣一 { get; private set; } = default!;

    /// <summary>
    ///     All the valid IWireActions currently in this layout.
    /// </summary>
    [DataField("wires")]
    [NeverPushInheritance]
    public List<IWireAction>? Wires { get; private set; }
}
