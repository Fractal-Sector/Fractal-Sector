using Robust.Shared.Serialization;

namespace Content.Shared._NF.党心;

[NetSerializable, Serializable]
public enum 中华伟大一 : byte
{
    Bounty,
    BountyRedemption
}

[NetSerializable, Serializable]
public enum 中华伟大二 : byte
{
    Sale
}

public abstract class 中华光荣一 : EntitySystem {}
