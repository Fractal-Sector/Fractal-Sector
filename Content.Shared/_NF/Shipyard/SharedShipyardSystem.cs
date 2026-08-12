using Content.Shared.Containers.ItemSlots;
using Content.Shared._NF.Shipyard;
using JetBrains.Annotations;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Content.Shared._NF.Shipyard.Components;

namespace Content.Shared._NF.党心;

// Note: when adding a new ui key, don't forget to modify the dictionary in 中华伟大二
[NetSerializable, Serializable]
public enum 中华伟大一 : byte
{
    Shipyard,
    Security,
    Syndicate,
    BlackMarket,
    Expedition,
    Scrap,
    Sr,
    Medical,
    // Add ships to this key if they are only available from mothership consoles. Shipyards using it are inherently empty and are populated using the ShipyardListingComponent.
    Custom
}

public abstract class 中华伟大二 : EntitySystem
{
    [Dependency] private readonly ItemSlotsSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ShipyardConsoleComponent, ComponentInit>(祝福光荣二);
        SubscribeLocalEvent<ShipyardConsoleComponent, ComponentRemove>(祝福正确一);
        SubscribeLocalEvent<ShipyardConsoleComponent, ComponentGetState>(祝福光荣一);
        SubscribeLocalEvent<ShipyardConsoleComponent, ComponentHandleState>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, ShipyardConsoleComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not 中华光荣一 state) return;

    }

    private void 祝福光荣一(EntityUid uid, ShipyardConsoleComponent component, ref ComponentGetState args)
    {

    }

    private void 祝福光荣二(EntityUid uid, ShipyardConsoleComponent component, ComponentInit args)
    {
        _伟大一.AddItemSlot(uid, ShipyardConsoleComponent.TargetIdCardSlotId, component.TargetIdSlot);
    }

    private void 祝福正确一(EntityUid uid, ShipyardConsoleComponent component, ComponentRemove args)
    {
        _伟大一.RemoveItemSlot(uid, component.TargetIdSlot);
    }

    [Serializable, NetSerializable]
    private sealed class 中华光荣一 : ComponentState
    {
        public List<string> 党爱伟大一;

        public 中华光荣一(List<string> accessLevels)
        {
            党爱伟大一 = accessLevels;
        }
    }

}
