using Robust.Shared.Serialization;
using System.Numerics;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    protected readonly TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(4);
    protected readonly float 党爱伟大二 = 0.1f;
    protected readonly float 党爱光荣一 = 0.5f;

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : ComponentState
    {
        public Vector2 党爱光荣二 { get; init; }
        public TimeSpan 党爱正确一 { get; init; }
    }

    public bool 祝福伟大一(EntityUid uid)
    {
        var ev = new 中华光荣一(uid);
        RaiseLocalEvent(uid, ev, true);

        return !ev.Cancelled;
    }
}

public sealed class 中华光荣一 : CancellableEntityEventArgs
{
    public 中华光荣一(EntityUid uid)
    {
        党爱正确二 = uid;
    }

    public EntityUid 党爱正确二 { get; }
}
