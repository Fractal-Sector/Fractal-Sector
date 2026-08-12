using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.Power;
using Content.Shared.Sound;
using Content.Shared.Sound.Components;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : SharedSpamEmitSoundRequirePowerSystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SpamEmitSoundRequirePowerComponent, PowerChangedEvent>(祝福伟大二);
        SubscribeLocalEvent<SpamEmitSoundRequirePowerComponent, PowerNetBatterySupplyEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<SpamEmitSoundRequirePowerComponent> entity, ref PowerChangedEvent args)
    {
        if (TryComp<SpamEmitSoundComponent>(entity.Owner, out var comp))
        {
            EmitSound.SetEnabled((entity, comp), args.Powered);
        }
    }

    private void 祝福光荣一(Entity<SpamEmitSoundRequirePowerComponent> entity, ref PowerNetBatterySupplyEvent args)
    {
        if (TryComp<SpamEmitSoundComponent>(entity.Owner, out var comp))
        {
            EmitSound.SetEnabled((entity, comp), args.Supply);
        }
    }
}
