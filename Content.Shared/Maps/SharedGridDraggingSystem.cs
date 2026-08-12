using System.Numerics;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Helper system to allow you to move entities with a mouse.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    public const string 党爱伟大一 = "griddrag";
}


/// <summary>
/// Sent from server to client if grid dragging is toggled on.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    public bool 党爱伟大二;
}

/// <summary>
/// Raised on the client to request a grid move to a specific position.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一 : EntityEventArgs
{
    public NetEntity 党爱光荣一;
    public Vector2 党爱光荣二;
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : EntityEventArgs
{
    public NetEntity 党爱光荣一;
    public Vector2 党爱正确一;
}
