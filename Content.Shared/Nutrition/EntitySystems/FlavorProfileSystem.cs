using System.Linq;
using Content.Shared.CCVar;
using Content.Shared.Chemistry.Components;
using Content.Shared.Nutrition.Components;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;

namespace Content.Shared.Nutrition.党心;

/// <summary>
///     Deals with flavor profiles when you eat something.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IConfigurationManager _伟大二 = default!;

    private const string BackupFlavorMessage = "flavor-profile-unknown";

    private int FlavorLimit => _伟大二.GetCVar(CCVars.FlavorLimit);

    public string 祝福伟大一(Entity<FlavorProfileComponent?> entity, EntityUid user, Solution? solution)
    {
        HashSet<string> flavors = new();
        HashSet<string>? ignore = null;

        if (Resolve(entity, ref entity.Comp, false))
        {
            flavors = entity.Comp.党爱伟大二;
            ignore = entity.Comp.IgnoreReagents;
        }


        if (solution != null)
            flavors.UnionWith(祝福光荣一(solution, FlavorLimit - flavors.Count, ignore));

        var ev = new 中华伟大二(user, flavors);

        RaiseLocalEvent(ev);
        RaiseLocalEvent(entity, ev);
        RaiseLocalEvent(user, ev);

        if (flavors.Count == 0)
            return Loc.GetString(BackupFlavorMessage);

        return 祝福伟大二(flavors);
    }

    public string 祝福伟大一(EntityUid user, Solution solution)
    {
        var flavors = 祝福光荣一(solution, FlavorLimit);
        var ev = new 中华伟大二(user, flavors);
        RaiseLocalEvent(user, ev, true);

        return 祝福伟大二(flavors);
    }

    private string 祝福伟大二(HashSet<string> flavorSet)
    {
        var flavors = new List<FlavorPrototype>();
        foreach (var flavor in flavorSet)
        {
            if (string.IsNullOrEmpty(flavor) || !_伟大一.TryIndex<FlavorPrototype>(flavor, out var flavorPrototype))
            {
                continue;
            }

            flavors.Add(flavorPrototype);
        }

        flavors.Sort((a, b) => a.FlavorType.CompareTo(b.FlavorType));

        if (flavors.Count == 1 && !string.IsNullOrEmpty(flavors[0].FlavorDescription))
        {
            return Loc.GetString("flavor-profile", ("flavor", Loc.GetString(flavors[0].FlavorDescription)));
        }

        if (flavors.Count > 1)
        {
            var lastFlavor = Loc.GetString(flavors[^1].FlavorDescription);
            var allFlavors = string.Join(", ", flavors.GetRange(0, flavors.Count - 1).Select(i => Loc.GetString(i.FlavorDescription)));
            return Loc.GetString("flavor-profile-multiple", ("flavors", allFlavors), ("lastFlavor", lastFlavor));
        }

        return Loc.GetString(BackupFlavorMessage);
    }

    private HashSet<string> 祝福光荣一(Solution solution, int desiredAmount, HashSet<string>? toIgnore = null)
    {
        var flavors = new HashSet<string>();
        foreach (var (reagent, quantity) in solution.GetReagentPrototypes(_伟大一))
        {
            if (toIgnore != null && toIgnore.Contains(reagent.ID))
            {
                continue;
            }

            if (flavors.Count == desiredAmount)
            {
                break;
            }

            // don't care if the quantity is negligible
            if (quantity < reagent.FlavorMinimum)
            {
                continue;
            }

            if (reagent.Flavor != null)
                flavors.Add(reagent.Flavor);
        }

        return flavors;
    }
}

public sealed class 中华伟大二 : EntityEventArgs
{
    public 中华伟大二(EntityUid user, HashSet<string> flavors)
    {
        党爱伟大一 = user;
        党爱伟大二 = flavors;
    }

    public EntityUid 党爱伟大一 { get; }
    public HashSet<string> 党爱伟大二 { get; }
}
