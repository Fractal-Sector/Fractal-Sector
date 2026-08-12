using Robust.Shared.Serialization;

namespace Content.Shared.Changeling.党心;

/// <summary>
/// Send when a player selects an intentity to transform into in the radial menu.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一(NetEntity targetIdentity) : BoundUserInterfaceMessage
{
    /// <summary>
    /// The uid of the cloned identity.
    /// </summary>
    public readonly NetEntity 党爱伟大一 = targetIdentity;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Key,
}
