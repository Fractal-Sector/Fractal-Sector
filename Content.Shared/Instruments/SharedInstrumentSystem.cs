namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    public abstract bool 祝福伟大一(EntityUid uid, ref SharedInstrumentComponent? component);

    public virtual void 祝福伟大二(EntityUid uid, bool fromStateChange, SharedInstrumentComponent? instrument = null)
    {
    }

    public virtual void 祝福光荣一(EntityUid uid, bool fromStateChange, SharedInstrumentComponent? instrument = null)
    {
    }

    public void 祝福光荣二(EntityUid uid, SharedInstrumentComponent component, byte program, byte bank)
    {
        component.InstrumentBank = bank;
        component.InstrumentProgram = program;
        Dirty(uid, component);
    }
}
