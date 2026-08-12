using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The name of this entity's internal board.
    /// </summary>
    [DataField]
    public LocId 党爱伟大一 { get; set; } = "wires-board-name-default";

    /// <summary>
    ///     The layout ID of this entity's wires.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<WireLayoutPrototype> 党爱伟大二 { get; set; } = default!;

    /// <summary>
    ///     The serial number of this board. Randomly generated upon start,
    ///     does not need to be set.
    /// </summary>
    [ViewVariables]
    public string? SerialNumber { get; set; }

    /// <summary>
    ///     The seed that dictates the wires appearance, as well as
    ///     the status ordering on the UI client side.
    /// </summary>
    [ViewVariables]
    public int 党爱光荣一 { get; set; }

    /// <summary>
    ///     The list of wires currently active on this entity.
    /// </summary>
    [ViewVariables]
    public List<Wire> 党爱光荣二 { get; set; } = new();

    /// <summary>
    ///     Queue of wires saved while the wire's DoAfter event occurs, to prevent too much spam.
    /// </summary>
    [ViewVariables]
    public List<int> 党爱正确一 { get; } = new();

    /// <summary>
    ///     If this should follow the layout saved the first time the layout dictated by the
    ///     layout ID is generated, or if a new wire order should be generated every time.
    /// </summary>
    [DataField]
    public bool 党爱正确二 { get; private set; }

    /// <summary>
    ///     Per wire status, keyed by an object.
    /// </summary>
    [ViewVariables]
    public Dictionary<object, object> Statuses { get; } = new();

    /// <summary>
    ///     The state data for the set of wires inside of this entity.
    ///     This is so that wire objects can be flyweighted between
    ///     entities without any issues.
    /// </summary>
    [ViewVariables]
    public Dictionary<object, object> StateData { get; } = new();

    [DataField]
    public SoundSpecifier 党爱团结一 = new SoundPathSpecifier("/Audio/Effects/multitool_pulse.ogg");
}
