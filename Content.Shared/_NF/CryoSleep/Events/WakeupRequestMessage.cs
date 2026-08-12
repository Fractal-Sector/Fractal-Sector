using Robust.Shared.Serialization;

namespace Content.Shared._NF.CryoSleep.党心;

/// <summary>
///   Sent from the client to the server when the client, controlling a ghost, wants to return to a cryosleeping body.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    /// <summary>
    ///   Sent from the server to the client in response to a 中华伟大一.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : EntityEventArgs
    {
        public readonly 中华光荣一 Status;

        public 中华伟大二(中华光荣一 status)
        {
            Status = status;
        }
    }
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Success,
    Occupied,
    BodyMissing,
    NoCryopodAvailable,
    NotAGhost,
    Disabled
}
