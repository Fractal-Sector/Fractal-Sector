using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.Audio;
using Content.Shared.Mobs;
using Content.Shared.Power;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedAmbientSoundSystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<AmbientOnPoweredComponent, PowerChangedEvent>(祝福光荣一);
        SubscribeLocalEvent<AmbientOnPoweredComponent, PowerNetBatterySupplyEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, AmbientOnPoweredComponent component, ref PowerNetBatterySupplyEvent args)
    {
        SetAmbience(uid, args.Supply);
    }

    private void 祝福光荣一(EntityUid uid, AmbientOnPoweredComponent component, ref PowerChangedEvent args)
    {
        SetAmbience(uid, args.Powered);
    }
}
