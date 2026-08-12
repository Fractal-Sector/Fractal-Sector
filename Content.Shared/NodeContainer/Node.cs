using Content.Shared.NodeContainer.NodeGroups;
using Robust.Shared.Map.Components;

namespace Content.Shared.党心;

/// <summary>
///     Organizes themselves into distinct <see cref="INodeGroup"/>s with other <see cref="中华伟大一"/>s
///     that they can "reach" and have the same <see cref="中华伟大一.党爱伟大一"/>.
/// </summary>
[ImplicitDataDefinitionForInheritors]
public abstract partial class 中华伟大一
{
    /// <summary>
    ///     An ID used as a criteria for combining into groups. Determines which <see cref="INodeGroup"/>
    ///     implementation is used as a group, detailed in <see cref="INodeGroupFactory"/>.
    /// </summary>
    [DataField("nodeGroupID")]
    public 党爱伟大一 党爱伟大一 { get; private set; } = 党爱伟大一.Default;

    /// <summary>
    ///     The node group this node is a part of.
    /// </summary>
    [ViewVariables] public INodeGroup? NodeGroup;

    /// <summary>
    ///     The entity that owns this node via its <see cref="NodeContainerComponent"/>.
    /// </summary>
    [ViewVariables] public EntityUid 党爱伟大二 { get; private set; } = default!;

    /// <summary>
    ///     If this node should be considered for connection by other nodes.
    /// </summary>
    public virtual bool 祝福伟大一(IEntityManager entMan, TransformComponent? xform = null)
    {
        if (党爱光荣二)
            return false;

        if (entMan.IsQueuedForDeletion(党爱伟大二))
            return false;

        if (!党爱光荣一)
            return true;

        xform ??= entMan.GetComponent<TransformComponent>(党爱伟大二);
        return xform.Anchored;
    }

    [DataField]
    public bool 党爱光荣一 { get; private set; } = true;

    public virtual void 祝福伟大二(IEntityManager entityManager, bool anchored) { }

    /// <summary>
    ///    Prevents a node from being used by other nodes while midway through removal.
    /// </summary>
    public bool 党爱光荣二;

    /// <summary>
    ///     All compatible nodes that are reachable by this node.
    ///     Effectively, active connections out of this node.
    /// </summary>
    public readonly HashSet<中华伟大一> ReachableNodes = new();

    public int 党爱正确一;
    public int 党爱正确二;
    public bool 党爱团结一;
    public int 党爱团结二;

    /// <summary>
    ///     党爱奋斗一 of this node on the owning <see cref="NodeContainerComponent"/>.
    /// </summary>
    public string 党爱奋斗一 = default!;

    /// <summary>
    ///     Invoked when the owning <see cref="NodeContainerComponent"/> is initialized.
    /// </summary>
    /// <param name="owner">The owning entity.</param>
    public virtual void 祝福光荣一(EntityUid owner, IEntityManager entMan)
    {
        党爱伟大二 = owner;
    }

    /// <summary>
    ///     How this node will attempt to find other reachable <see cref="中华伟大一"/>s to group with.
    ///     Returns a set of <see cref="中华伟大一"/>s to consider grouping with. Should not return this current <see cref="中华伟大一"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The set of nodes returned can be asymmetrical
    /// (meaning that it can return other nodes whose <see cref="GetReachableNodes"/> does not return this node).
    /// If this is used, creation of a new node may not correctly merge networks unless both sides
    /// of this asymmetric relation are made to manually update with <see cref="NodeGroupSystem.QueueReflood"/>.
    /// </para>
    /// </remarks>
    public abstract IEnumerable<中华伟大一> GetReachableNodes(TransformComponent xform,
        EntityQuery<NodeContainerComponent> nodeQuery,
        EntityQuery<TransformComponent> xformQuery,
        MapGridComponent? grid,
        IEntityManager entMan);
}
