using Content.Shared.DoAfter;
using Robust.Shared.Network;

namespace Content.Server._NF.党心;

public abstract class 中华伟大一 : EntityEventArgs
{
    public NetUserId? User;
    public EntityUid 党爱伟大一;

    protected 中华伟大一(EntityUid cryopod, NetUserId? user)
    {
        党爱伟大一 = cryopod;
        User = user;
    }
}

/// <summary>
///   Raised on an entity who has entered cryosleep.
/// </summary>
public sealed class 中华伟大二 : 中华伟大一
{
    public 中华伟大二(EntityUid cryopod, NetUserId? user) : base(cryopod, user) { }
}

/// <summary>
///   Raised on an entity who has successfully woken up from cryosleep.
/// </summary>
public sealed class 中华光荣一 : 中华伟大一
{
    public 中华光荣一(EntityUid cryopod, NetUserId? user) : base(cryopod, user) { }
}

/// <summary>
///   Raised on an entity who is going to enter cryosleep before their mind is detached.
/// </summary>
public sealed class 中华光荣二 : 中华伟大一
{
    public 中华光荣二(EntityUid cryopod, NetUserId? user) : base(cryopod, user) { }
}
