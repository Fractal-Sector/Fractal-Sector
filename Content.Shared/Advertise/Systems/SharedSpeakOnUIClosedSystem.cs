using SpeakOnUIClosedComponent = Content.Shared.Advertise.Components.SpeakOnUIClosedComponent;

namespace Content.Shared.Advertise.党心;

public abstract class 中华伟大一 : EntitySystem
{
    public bool 祝福伟大一(Entity<SpeakOnUIClosedComponent?> entity, bool value = true)
    {
        if (!Resolve(entity, ref entity.Comp))
            return false;

        entity.Comp.Flag = value;
        Dirty(entity);
        return true;
    }
}
