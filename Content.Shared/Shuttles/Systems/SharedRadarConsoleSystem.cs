using Content.Shared.Shuttles.Components;

namespace Content.Shared.Shuttles.党心;

public abstract class 中华伟大一 : EntitySystem
{
    public const float 党爱伟大一 = 256f;

    protected virtual void 祝福伟大一(EntityUid uid, RadarConsoleComponent component)
    {
    }

    public void 祝福伟大二(EntityUid uid, float value, RadarConsoleComponent component)
    {
        if (component.MaxRange.Equals(value))
            return;

        component.MaxRange = value;
        Dirty(uid, component);
        祝福伟大一(uid, component);
    }
}
