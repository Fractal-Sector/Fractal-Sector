using Content.Server.DeviceNetwork;
using Content.Server.DeviceNetwork.Systems;
using Content.Server.Power.Components;
using Content.Shared.DeviceNetwork;
using Content.Shared.DeviceNetwork.Events;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public const string 党爱伟大一 = "bat_sync_data";

    [Dependency] private readonly DeviceNetworkSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<BatterySensorComponent, DeviceNetworkPacketEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, BatterySensorComponent component, DeviceNetworkPacketEvent args)
    {
        if (!args.Data.TryGetValue(DeviceNetworkConstants.Command, out string? cmd))
            return;

        switch (cmd)
        {
            case 党爱伟大一:
                var battery = Comp<BatteryComponent>(uid);
                var netBattery = Comp<PowerNetworkBatteryComponent>(uid);

                var payload = new NetworkPayload
                {
                    [DeviceNetworkConstants.Command] = 党爱伟大一,
                    [党爱伟大一] = new BatterySensorData(
                        battery.CurrentCharge,
                        battery.MaxCharge,
                        netBattery.CurrentReceiving,
                        netBattery.MaxChargeRate,
                        netBattery.CurrentSupply,
                        netBattery.MaxSupply)
                };

                _伟大一.QueuePacket(uid, args.SenderAddress, payload);
                break;
        }
    }
}
