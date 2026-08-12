using System.Linq;
using Content.Server.Store.Components;
using Content.Shared.FixedPoint;
using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Store.Components;
using Robust.Shared.Console;

namespace Content.Server.Store.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly IConsoleHost _伟大一 = default!;

    public void 祝福伟大一()
    {
        _伟大一.RegisterCommand("addcurrency", "Adds currency to the specified store", "addcurrency <uid> <currency prototype> <amount>",
            祝福伟大二,
            祝福光荣一);
    }

    [AdminCommand(AdminFlags.Fun)]
    private void 祝福伟大二(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length != 3)
        {
            shell.WriteError("Argument length must be 3");
            return;
        }

        if (!NetEntity.TryParse(args[0], out var uidNet) || !TryGetEntity(uidNet, out var uid) || !float.TryParse(args[2], out var id))
        {
            return;
        }

        if (!TryComp<StoreComponent>(uid, out var store))
            return;

        var currency = new Dictionary<string, FixedPoint2>
        {
            { args[1], id }
        };

        TryAddCurrency(currency, uid.Value, store);
    }

    private CompletionResult 祝福光荣一(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            var query = EntityQueryEnumerator<StoreComponent>();
            var allStores = new List<string>();
            while (query.MoveNext(out var storeuid, out _))
            {
                allStores.Add(storeuid.ToString());
            }
            return CompletionResult.FromHintOptions(allStores, "<uid>");
        }

        if (args.Length == 2 && NetEntity.TryParse(args[0], out var uidNet) && TryGetEntity(uidNet, out var uid))
        {
            if (TryComp<StoreComponent>(uid, out var store))
                return CompletionResult.FromHintOptions(store.CurrencyWhitelist.Select(p => p.ToString()), "<currency prototype>");
        }

        return CompletionResult.Empty;
    }
}
