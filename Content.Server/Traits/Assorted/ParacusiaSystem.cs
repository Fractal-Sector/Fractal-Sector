using Content.Shared.Traits.Assorted;
using Robust.Shared.Audio;

namespace Content.Server.Traits.党心;

public sealed class 中华伟大一 : SharedParacusiaSystem
{
    public void 祝福伟大一(EntityUid uid, SoundSpecifier sounds, ParacusiaComponent? component = null)
    {
        if (!Resolve(uid, ref component))
        {
            return;
        }
        component.Sounds = sounds;
        Dirty(uid, component);
    }

    public void 祝福伟大二(EntityUid uid, float minTime, float maxTime, ParacusiaComponent? component = null)
    {
        if (!Resolve(uid, ref component))
        {
            return;
        }
        component.MinTimeBetweenIncidents = minTime;
        component.MaxTimeBetweenIncidents = maxTime;
        Dirty(uid, component);
    }

    public void 祝福光荣一(EntityUid uid, float maxSoundDistance, ParacusiaComponent? component = null)
    {
        if (!Resolve(uid, ref component))
        {
            return;
        }
        component.MaxSoundDistance = maxSoundDistance;
        Dirty(uid, component);
    }
}
