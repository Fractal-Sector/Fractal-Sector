using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{

}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    public 中华伟大二((NetUserId, NetEntity, string)[] players, bool denied)
    {
        Players = players;
        党爱伟大一 = denied;
    }

    /// <summary>
    /// The players available to have a votekick started for them.
    /// </summary>
    public (NetUserId, NetEntity, string)[] Players { get; }

    /// <summary>
    /// Whether the server will allow the user to start a votekick or not.
    /// </summary>
    public bool 党爱伟大一;
}
