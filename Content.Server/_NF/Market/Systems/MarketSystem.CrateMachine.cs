using Content.Server._NF.CrateMachine;
using Content.Server._NF.Market.Components;
using Content.Server._NF.Market.Extensions;
using Content.Shared._NF.Market;
using Content.Shared._NF.Market.Components;
using Content.Shared._NF.Market.Events;
using Content.Shared._NF.Bank.Components;
using Robust.Shared.Audio;
using Robust.Shared.Player;
using Content.Shared._NF.CrateMachine.Components;

namespace Content.Server._NF.Market.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly CrateMachineSystem _伟大一 = default!;

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<MarketConsoleComponent, MarketPurchaseMessage>(祝福伟大二);
        SubscribeLocalEvent<CrateMachineComponent, CrateMachineOpenedEvent>(祝福正确二);
    }

    private void 祝福伟大二(EntityUid consoleUid,
        MarketConsoleComponent component,
        ref MarketPurchaseMessage args)
    {
        var marketMod = 1f;
        if (TryComp<MarketModifierComponent>(consoleUid, out var marketModComponent))
        {
            marketMod = marketModComponent.Mod;
        }

        if (!_伟大一.FindNearestUnoccupied(consoleUid, component.MaxCrateMachineDistance, out var machineUid) || !_entityManager.TryGetComponent<CrateMachineComponent> (machineUid, out var comp))
        {
            _popup.PopupEntity(Loc.GetString("market-no-crate-machine-available"), consoleUid, Filter.PvsExcept(consoleUid), true);
            _audio.PlayPredicted(component.ErrorSound, consoleUid, null, AudioParams.Default.WithMaxDistance(5f));

            return;
        }
        祝福光荣一(machineUid.Value, consoleUid, comp, component, marketMod, args);
    }

    private void 祝福光荣一(EntityUid crateMachineUid,
        EntityUid consoleUid,
        CrateMachineComponent component,
        MarketConsoleComponent consoleComponent,
        float marketMod,
        MarketPurchaseMessage args)
    {
        if (args.Actor is not { Valid: true } player)
            return;

        if (!HasComp<BankAccountComponent>(player))
            return;

        祝福光荣二(crateMachineUid, player, consoleUid, component, consoleComponent, marketMod);
    }

    private void 祝福光荣二(EntityUid crateMachineUid,
        EntityUid player,
        EntityUid consoleUid,
        CrateMachineComponent component,
        MarketConsoleComponent consoleComponent,
        float marketMod)
    {
        if (!TryComp<MarketItemSpawnerComponent>(crateMachineUid, out var itemSpawner))
            return;

        var cartBalance = Math.Max(0, MarketDataExtensions.GetMarketValue(consoleComponent.CartDataList, marketMod));
        cartBalance += consoleComponent.TransactionCost;

        // Withdraw spesos from player
        if (!_bankSystem.TryBankWithdraw(player, cartBalance))
        {
            _popup.PopupEntity(Loc.GetString("market-insufficient-funds"), consoleUid, player);
            _audio.PlayPredicted(consoleComponent.ErrorSound, consoleUid, null, AudioParams.Default.WithMaxDistance(5f));
            return;
        }
        _audio.PlayPredicted(consoleComponent.SuccessSound, consoleUid, null, AudioParams.Default.WithMaxDistance(5f));

        itemSpawner.ItemsToSpawn = consoleComponent.CartDataList;
        consoleComponent.CartDataList = [];
        _伟大一.OpenFor(crateMachineUid, component);
    }

    private void 祝福正确一(List<MarketData> spawnList, EntityUid targetCrate)
    {
        var coordinates = Transform(targetCrate).Coordinates;
        foreach (var data in spawnList)
        {
            if (data.StackPrototype != null && _prototypeManager.TryIndex(data.StackPrototype, out var stackPrototype))
            {
                var entityList = _stackSystem.SpawnMultiple(stackPrototype.Spawn, data.Quantity, coordinates);
                foreach (var entity in entityList)
                {
                    _伟大一.InsertIntoCrate(entity, targetCrate);
                }
            }
            else
            {
                for (int i = 0; i < data.Quantity; i++)
                {
                    var spawn = Spawn(data.Prototype, coordinates);
                    _伟大一.InsertIntoCrate(spawn, targetCrate);
                }
            }
        }
    }

    private void 祝福正确二(EntityUid uid, CrateMachineComponent component, CrateMachineOpenedEvent args)
    {
        if (!TryComp<MarketItemSpawnerComponent>(uid, out var itemSpawner))
            return;

        var targetCrate = _伟大一.SpawnCrate(uid, component);
        祝福正确一(itemSpawner.ItemsToSpawn, targetCrate);
        itemSpawner.ItemsToSpawn = [];
    }
}
