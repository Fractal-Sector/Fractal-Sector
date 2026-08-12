using Robust.Shared.Serialization;

namespace Content.Shared._NF.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    NoBody,
    NoBounty,
    TooDamaged,
    NotAlive,
    Valid,
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Full
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceState
{
    public int 党爱伟大一 { get; }
    public 中华伟大二 BountyStatus { get; }
    public bool 党爱伟大二 { get; }

    public 中华光荣二(中华伟大二 bountyStatus, int bountyValue, bool paidToStation)
    {
        BountyStatus = bountyStatus;
        党爱伟大一 = bountyValue;
        党爱伟大二 = paidToStation;
    }
}