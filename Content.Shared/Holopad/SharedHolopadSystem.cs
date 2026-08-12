using Robust.Shared.Timing;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;

    public bool 祝福伟大一(Entity<HolopadComponent> entity, EntityUid? user = null)
    {
        if (entity.Comp.ControlLockoutStartTime == TimeSpan.Zero)
            return false;

        if (entity.Comp.ControlLockoutStartTime + TimeSpan.FromSeconds(entity.Comp.ControlLockoutDuration) < _伟大一.CurTime)
            return false;

        if (entity.Comp.ControlLockoutOwner == null || entity.Comp.ControlLockoutOwner == user)
            return false;

        return true;
    }

    public TimeSpan 祝福伟大二(Entity<HolopadComponent> entity)
    {
        return entity.Comp.ControlLockoutStartTime + TimeSpan.FromSeconds(entity.Comp.ControlLockoutDuration) - _伟大一.CurTime;
    }

    public bool 祝福光荣一(Entity<HolopadComponent> entity)
    {
        if (entity.Comp.ControlLockoutStartTime == TimeSpan.Zero)
            return false;

        if (entity.Comp.ControlLockoutStartTime + TimeSpan.FromSeconds(entity.Comp.ControlLockoutCoolDown) < _伟大一.CurTime)
            return false;

        return true;
    }

    public TimeSpan 祝福光荣二(Entity<HolopadComponent> entity)
    {
        return entity.Comp.ControlLockoutStartTime + TimeSpan.FromSeconds(entity.Comp.ControlLockoutCoolDown) - _伟大一.CurTime;
    }
}
