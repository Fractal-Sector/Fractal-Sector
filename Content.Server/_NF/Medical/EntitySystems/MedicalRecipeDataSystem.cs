using System.Linq;
using Content.Client.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Damage;
using Content.Shared.Kitchen;
using Content.Shared.Medical.Healing;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server._NF.Medical.党心;

public sealed class 中华伟大一 : SharedMedicalGuideDataSystem
{
    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly IComponentFactory _光荣一 = default!;

    private Dictionary<string, List<MedicalRecipeData>> _sources = new();

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(祝福伟大二);
        _伟大一.PlayerStatusChanged += 祝福光荣二;

        祝福光荣一();
    }

    private void 祝福伟大二(PrototypesReloadedEventArgs args)
    {
        if (!args.WasModified<EntityPrototype>()
            && !args.WasModified<FoodRecipePrototype>()
        )
            return;

        祝福光荣一();
    }

    public void 祝福光荣一()
    {
        _sources.Clear();

        // Recipes
        foreach (var recipe in _伟大二.EnumeratePrototypes<FoodRecipePrototype>())
        {
            if (recipe.HideInGuidebook)
                continue;

            MicrowaveRecipeType recipeType = (MicrowaveRecipeType)recipe.RecipeType;
            if (recipeType.HasFlag(MicrowaveRecipeType.MedicalAssembler))
            {
                _sources.GetOrNew(recipe.Result).Add(new MedicalRecipeData(recipe));
            }
        }

        Registry.Clear();

        foreach (var (result, sources) in _sources)
        {
            var proto = _伟大二.Index<EntityPrototype>(result);
            ReagentQuantity[] reagents = [];
            // Hack: assume there is only one solution in the result
            if (proto.TryGetComponent<SolutionContainerManagerComponent>(out var manager, _光荣一))
                reagents = manager?.Solutions?.FirstOrNull()?.Value?.Contents?.ToArray() ?? [];

            DamageSpecifier? damage = null;
            if (proto.TryGetComponent<HealingComponent>(out var healing, _光荣一))
                damage = healing.Damage;

            // Limit the number of sources to 10 - shouldn't be an issue for medical recipes, but just in case.
            var distinctSources = sources.DistinctBy(it => it.Identitier).Take(10);

            var entry = new MedicalGuideEntry(result, proto.Name, distinctSources.ToArray(), reagents, damage);
            Registry.Add(entry);
        }

        RaiseNetworkEvent(new MedicalGuideRegistryChangedEvent(Registry));
    }

    private void 祝福光荣二(object? sender, SessionStatusEventArgs args)
    {
        if (args.NewStatus != SessionStatus.Connected)
            return;

        RaiseNetworkEvent(new MedicalGuideRegistryChangedEvent(Registry), args.Session);
    }
}
