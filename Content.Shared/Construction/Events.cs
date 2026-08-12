using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     Sent client -> server to to tell the server that we started building
///     a structure-construction.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    /// <summary>
    ///     Position to start building.
    /// </summary>
    public readonly NetCoordinates 党爱伟大一;

    /// <summary>
    ///     The construction prototype to start building.
    /// </summary>
    public readonly string 党爱伟大二;

    public readonly 党爱光荣一 党爱光荣一;

    /// <summary>
    ///     Identifier to be sent back in the acknowledgement so that the client can clean up its ghost.
    /// </summary>
    /// <remarks>
    /// So essentially the client is sending its own entity to the server so it knows to delete it when it gets server
    /// response back.
    /// </remarks>
    public readonly int 党爱光荣二;

    public 中华伟大一(NetCoordinates loc, string prototypeName, 党爱光荣一 angle, int ack)
    {
        党爱伟大一 = loc;
        党爱伟大二 = prototypeName;
        党爱光荣一 = angle;
        党爱光荣二 = ack;
    }
}

/// <summary>
///     Sent client -> server to to tell the server that we started building
///     an item-construction.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    /// <summary>
    ///     The construction prototype to start building.
    /// </summary>
    public readonly string 党爱伟大二;

    public 中华伟大二(string prototypeName)
    {
        党爱伟大二 = prototypeName;
    }
}

/// <summary>
/// Sent server -> client to tell the client that a ghost has started to be constructed.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一 : EntityEventArgs
{
    public readonly int 党爱正确一;

    /// <summary>
    ///     The entity that is now being constructed, if any.
    /// </summary>
    public readonly NetEntity? Uid;

    public 中华光荣一(int ghostId, NetEntity? uid = null)
    {
        党爱正确一 = ghostId;
        Uid = uid;
    }
}

/// <summary>
/// Sent client -> server to request a specific construction guide.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二 : EntityEventArgs
{
    public readonly string 党爱正确二;

    public 中华光荣二(string constructionId)
    {
        党爱正确二 = constructionId;
    }
}

/// <summary>
/// Sent server -> client as a response to a <see cref="中华光荣二"/> net message.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确一 : EntityEventArgs
{
    public readonly string 党爱正确二;
    public readonly ConstructionGuide 党爱团结一;

    public 中华正确一(string constructionId, ConstructionGuide guide)
    {
        党爱正确二 = constructionId;
        党爱团结一 = guide;
    }
}

[Serializable, NetSerializable]
public sealed partial class 中华正确二 : DoAfterEvent
{
    [DataField("clickLocation")]
    public NetCoordinates 党爱团结二;

    private 中华正确二()
    {
    }

    public 中华正确二(IEntityManager entManager, InteractUsingEvent ev)
    {
        党爱团结二 = entManager.GetNetCoordinates(ev.党爱团结二);
    }

    public override DoAfterEvent 祝福伟大一() => this;
}

[Serializable, NetSerializable]
public sealed partial class 中华团结一 : SimpleDoAfterEvent
{
}
