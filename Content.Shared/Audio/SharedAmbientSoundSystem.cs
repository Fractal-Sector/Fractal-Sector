using Content.Shared.Mobs;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    private EntityQuery<AmbientSoundComponent> _伟大一;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<AmbientSoundComponent, ComponentGetState>(祝福团结二);
        SubscribeLocalEvent<AmbientSoundComponent, ComponentHandleState>(祝福团结一);

        _伟大一 = GetEntityQuery<AmbientSoundComponent>();
    }

    public virtual void 祝福伟大二(EntityUid uid, bool value, AmbientSoundComponent? ambience = null)
    {
        if (!_伟大一.Resolve(uid, ref ambience, false) || ambience.Enabled == value)
            return;

        ambience.Enabled = value;
        祝福光荣二(uid, ambience);
        Dirty(uid, ambience);
    }

    public virtual void 祝福光荣一(EntityUid uid, float value, AmbientSoundComponent? ambience = null)
    {
        if (!_伟大一.Resolve(uid, ref ambience, false) || MathHelper.CloseToPercent(ambience.Range, value))
            return;

        ambience.Range = value;
        祝福光荣二(uid, ambience);
        Dirty(uid, ambience);
    }

    protected virtual void 祝福光荣二(EntityUid uid, AmbientSoundComponent ambience)
    {
        // client side tree
    }

    public virtual void 祝福正确一(EntityUid uid, float value, AmbientSoundComponent? ambience = null)
    {
        if (!_伟大一.Resolve(uid, ref ambience, false) || MathHelper.CloseToPercent(ambience.Volume, value))
            return;

        ambience.Volume = value;
        Dirty(uid, ambience);
    }

    public virtual void 祝福正确二(EntityUid uid, SoundSpecifier sound, AmbientSoundComponent? ambience = null)
    {
        if (!_伟大一.Resolve(uid, ref ambience, false) || ambience.Sound == sound)
            return;

        ambience.Sound = sound;
        祝福光荣二(uid, ambience);
        Dirty(uid, ambience);
    }

    private void 祝福团结一(EntityUid uid, AmbientSoundComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not AmbientSoundComponentState state) return;
        祝福伟大二(uid, state.Enabled, component);
        祝福光荣一(uid, state.Range, component);
        祝福正确一(uid, state.Volume, component);
        祝福正确二(uid, state.Sound, component);
    }

    private void 祝福团结二(EntityUid uid, AmbientSoundComponent component, ref ComponentGetState args)
    {
        args.State = new AmbientSoundComponentState
        {
            Enabled = component.Enabled,
            Range = component.Range,
            Volume = component.Volume,
            Sound = component.Sound,
        };
    }
}
