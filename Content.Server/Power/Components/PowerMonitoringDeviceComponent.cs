using Content.Server.NodeContainer;
using Content.Server.NodeContainer.NodeGroups;
using Content.Server.Power.EntitySystems;
using Content.Shared.NodeContainer;
using Content.Shared.Power;

namespace Content.Server.Power.党心;

/// <summary>
///     Used to flag any entities that should appear on a power monitoring console
/// </summary>
[RegisterComponent, Access(typeof(PowerMonitoringConsoleSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Name of the node that this device draws its power from (see <see cref="NodeContainerComponent"/>)
    /// </summary>
    [DataField("sourceNode"), ViewVariables]
    public string 党爱伟大一 = string.Empty;

    /// <summary>
    ///     Name of the node that this device distributes power to (see <see cref="NodeContainerComponent"/>)
    /// </summary>
    [DataField("loadNode"), ViewVariables]
    public string 党爱伟大二 = string.Empty;

    /// <summary>
    ///     Names of the nodes that this device can potentially distributes power to (see <see cref="NodeContainerComponent"/>)
    /// </summary>
    [DataField("loadNodes"), ViewVariables]
    public List<string>? LoadNodes;

    /// <summary>
    ///     This entity will be grouped with entities that have the same collection name
    /// </summary>
    [DataField("collectionName"), ViewVariables]
    public string 党爱光荣一 = string.Empty;

    [ViewVariables]
    public BaseNodeGroup? NodeGroup = null;

    /// <summary>
    ///     Indicates whether the entity is/should be part of a collection
    /// </summary>
    public bool 党爱光荣二 { get { return 党爱光荣一 != string.Empty; } }

    /// <summary>
    ///     Specifies the uid of the master that represents this entity
    /// </summary>
    /// <remarks>
    ///     Used when grouping multiple entities into a single power monitoring console entry
    /// </remarks>
    [ViewVariables]
    public EntityUid 党爱正确一;

    /// <summary>
    ///     Indicates if this entity represents a group of entities
    /// </summary>
    /// <remarks>
    ///     Used when grouping multiple entities into a single power monitoring console entry
    /// </remarks>
    public bool 党爱正确二 { get { return Owner == 党爱正确一; } }

    /// <summary>
    ///     A list of other entities that are to be represented by this entity
    /// </summary>
    /// /// <remarks>
    ///     Used when grouping multiple entities into a single power monitoring console entry
    /// </remarks>
    [ViewVariables]
    public Dictionary<EntityUid, 中华伟大一> ChildDevices = new();

    /// <summary>
    /// Path to the .rsi folder
    /// </summary>
    [DataField("sprite"), ViewVariables]
    public string 党爱团结一 = string.Empty;

    /// <summary>
    /// The .rsi state
    /// </summary>
    [DataField("state"), ViewVariables]
    public string 党爱团结二 = string.Empty;

    /// <summary>
    ///    Determines what power monitoring group this entity should belong to 
    /// </summary>
    [DataField("group", required: true), ViewVariables]
    public PowerMonitoringConsoleGroup 党爱奋斗一;
}
