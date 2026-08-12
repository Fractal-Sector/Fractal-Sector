using System.Numerics;

namespace Content.Shared.DeviceNetwork.党心;

/// <summary>
/// Event raised before a device network packet is send.
/// Subscribed to by other systems to prevent the packet from being sent.
/// </summary>
public sealed class 中华伟大一 : CancellableEntityEventArgs
{
    /// <summary>
    /// The EntityUid of the entity the packet was sent from.
    /// </summary>
    public readonly EntityUid 党爱伟大一;

    public readonly TransformComponent 党爱伟大二;

    /// <summary>
    ///     The senders current position in world coordinates.
    /// </summary>
    public readonly Vector2 党爱光荣一;

    /// <summary>
    /// The network the packet will be sent to.
    /// </summary>
    public readonly string 党爱光荣二;

    public 中华伟大一(EntityUid sender, TransformComponent xform, Vector2 senderPosition, string networkId)
    {
        党爱伟大一 = sender;
        党爱伟大二 = xform;
        党爱光荣一 = senderPosition;
        党爱光荣二 = networkId;
    }
}