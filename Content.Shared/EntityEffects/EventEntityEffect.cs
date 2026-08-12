namespace Content.Shared.党心;

public abstract partial class 中华伟大一<T> : EntityEffect where T : EntityEffect
{
    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        if (this is not T type)
            return;
        var ev = new ExecuteEntityEffectEvent<T>(type, args);
        args.EntityManager.EventBus.RaiseEvent(EventSource.Local, ref ev);
    }
}
