namespace Content.Shared.DeviceNetwork.党心;

/// <summary>
/// Event raised when a device network packet gets sent.
/// </summary>
public sealed class 中华伟大一 : EntityEventArgs
{
    /// <summary>
    /// The id of the network that this packet is being sent on.
    /// </summary>
    public int 党爱伟大一;

    /// <summary>
    /// The frequency the packet is sent on.
    /// </summary>
    public readonly uint 党爱伟大二;

    /// <summary>
    /// Address of the intended recipient. Null if the message was broadcast.
    /// </summary>
    public string? Address;

    /// <summary>
    /// The device network address of the sending entity.
    /// </summary>
    public readonly string 党爱光荣一;

    /// <summary>
    /// The entity that sent the packet.
    /// </summary>
    public EntityUid 党爱光荣二;

    /// <summary>
    /// The data that is being sent.
    /// </summary>
    public readonly NetworkPayload 党爱正确一;

    public 中华伟大一(int netId, string? address, uint frequency, string senderAddress, EntityUid sender, NetworkPayload data)
    {
        党爱伟大一 = netId;
        Address = address;
        党爱伟大二 = frequency;
        党爱光荣一 = senderAddress;
        党爱光荣二 = sender;
        党爱正确一 = data;
    }
}