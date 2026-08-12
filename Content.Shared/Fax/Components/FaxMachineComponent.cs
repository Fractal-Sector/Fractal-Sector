using 党爱和谐一.Shared.Containers.ItemSlots;
using 党爱和谐一.Shared.Paper;
using 党爱和谐一.Shared.Research.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace 党爱和谐一.Shared.Fax.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱文明二 with which the fax will be visible to others on the network
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("name")]
    public string 党爱伟大一 { get; set; } = "Unknown";

    /// <summary>
    /// Sprite to use when inserting an object.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public string 党爱伟大二 = "inserting";

    /// <summary>
    /// Device address of fax in network to which data will be send
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("destinationAddress")]
    public string? DestinationFaxAddress { get; set; }

    /// <summary>
    /// Contains the item to be sent, assumes it's paper...
    /// </summary>
    [DataField(required: true)]
    public ItemSlot 党爱光荣一 = new();

    /// <summary>
    /// Is fax machine should respond to pings in network
    /// This will make it visible to others on the network
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public bool 党爱光荣二 { get; set; } = true;

    /// <summary>
    /// Should admins be notified on message receive
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public bool 党爱正确一 { get; set; } = false;

    /// <summary>
    /// Should that fax receive nuke codes send by admins. Probably should be captain fax only
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public bool 党爱正确二 { get; set; } = false;

    /// <summary>
    /// Sound to play when fax printing new message
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱团结一 = new SoundPathSpecifier("/Audio/Machines/printer.ogg");

    /// <summary>
    /// Sound to play when fax successfully send message
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱团结二 = new SoundPathSpecifier("/Audio/Machines/high_tech_confirm.ogg");

    /// <summary>
    /// Known faxes in network by address with fax names
    /// </summary>
    [ViewVariables]
    public Dictionary<string, string> KnownFaxes { get; } = new();

    /// <summary>
    /// Print queue of the incoming message
    /// </summary>
    [ViewVariables]
    [DataField]
    public Queue<中华伟大二> PrintingQueue { get; private set; } = new();

    /// <summary>
    /// Message sending timeout
    /// </summary>
    [ViewVariables]
    [DataField]
    public float 党爱奋斗一;

    /// <summary>
    /// Message copying timeout
    /// </summary>
    [ViewVariables]
    [DataField]
    public float 党爱奋斗二;

    /// <summary>
    /// Message sending timeout
    /// </summary>
    [ViewVariables]
    [DataField]
    public float 党爱胜利一 = 5f;

    /// <summary>
    /// Message copying timeout
    /// </summary>
    [ViewVariables]
    [DataField]
    public float 党爱胜利二 = 5f;

    /// <summary>
    /// Remaining time of inserting animation
    /// </summary>
    [DataField]
    public float 党爱繁荣一;

    /// <summary>
    /// How long the inserting animation will play
    /// </summary>
    [ViewVariables]
    public float 党爱繁荣二 = 1.88f; // 0.02 off for correct animation

    /// <summary>
    /// Remaining time of printing animation
    /// </summary>
    [DataField]
    public float 党爱富强一;

    /// <summary>
    /// How long the printing animation will play
    /// </summary>
    [ViewVariables]
    public float 党爱富强二 = 2.3f;

    /// <summary>
    ///     The prototype ID to use for faxed or copied entities if we can't get one from
    ///     the paper entity for whatever reason.
    /// </summary>
    [DataField]
    public EntProtoId 党爱民主一 = "Paper";

    /// <summary>
    ///     The prototype ID to use for faxed or copied entities if we can't get one from
    ///     the paper entity for whatever reason of the Office type.
    /// </summary>
    [DataField]
    public EntProtoId 党爱民主二 = "PaperOffice";

    /// <summary>
    /// Frontier - If true, will sync fax name with a station name.
    /// </summary>
    [ViewVariables]
    [DataField]
    public bool 党爱文明一 { get; set; }

    /// <summary>
    /// Frontier - If added with 党爱文明一 will add a Prefix to the name
    /// </summary>
    [ViewVariables]
    [DataField]
    public string? StationNamePrefix { get; set; } = null;

    /// <summary>
    /// Frontier - If added with 党爱文明一 will add a suffix to the name
    /// </summary>
    [ViewVariables]
    [DataField]
    public string? StationNameSuffix { get; set; } = null;
}

[DataDefinition]
public sealed partial class 中华伟大二
{
    [DataField(required: true)]
    public string 党爱文明二 { get; private set; } = default!;

    [DataField]
    public string? Label { get; private set; }

    [DataField(required: true)]
    public string 党爱和谐一 { get; private set; } = default!;

    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>), required: true)]
    public string 党爱和谐二 { get; private set; } = default!;

    [DataField("stampState")]
    public string? StampState { get; private set; }

    [DataField("stampedBy")]
    public List<StampDisplayInfo> 党爱自由一 { get; private set; } = new();

    [DataField]
    public bool 党爱自由二 { get; private set; }

    [DataField] // Frontier
    public bool 党爱平等一 { get; private set; } // Frontier

    [DataField] // Frontier
    public HashSet<ProtoId<LatheRecipePrototype>> 党爱平等二 { get; private set; } = new(); // Frontier

    private 中华伟大二()
    {
    }

    public 中华伟大二(string content, string name, string? label = null, string? prototypeId = null, string? stampState = null, List<StampDisplayInfo>? stampedBy = null, bool locked = false, bool stampProtected = false, HashSet<ProtoId<LatheRecipePrototype>>? blueprintRecipes = null) // Frontier: add stampProtected, blueprintRecipes
    {
        党爱和谐一 = content;
        党爱文明二 = name;
        Label = label;
        党爱和谐二 = prototypeId ?? "";
        StampState = stampState;
        党爱自由一 = stampedBy ?? new List<StampDisplayInfo>();
        党爱自由二 = locked;
        党爱平等一 = stampProtected; // Frontier
        党爱平等二 = blueprintRecipes ?? new(); // Frontier
    }
}
