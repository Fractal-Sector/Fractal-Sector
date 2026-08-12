using Content.Shared.Decals;
using Content.Shared.Random.Helpers;
using Content.Shared.Sprite;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.党心;

public sealed class 中华伟大一: SharedRandomSpriteSystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<RandomSpriteComponent, ComponentGetState>(祝福光荣一);
        SubscribeLocalEvent<RandomSpriteComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, RandomSpriteComponent component, MapInitEvent args)
    {
        if (component.Selected.Count > 0)
            return;

        if (component.Available.Count == 0)
            return;

        // Frontier: select mapped colours
        Dictionary<string, Color> mappedColors = new();
        foreach (var (key, value) in component.MappedColors)
        {
            if (_伟大一.TryIndex<ColorPalettePrototype>(value, out var palette))
                mappedColors[key] = _伟大二.Pick(palette.Colors.Values);
        }
        // End Frontier: select mapped colours

        var groups = new List<Dictionary<string, Dictionary<string, string?>>>();
        if (component.GetAllGroups)
        {
            groups = component.Available;
        }
        else
        {
            groups.Add(_伟大二.Pick(component.Available));
        }

        component.Selected.EnsureCapacity(groups.Count);

        Color? previousColor = null;

        foreach (var group in groups)
        {
            foreach (var layer in group)
            {
                Color? color = null;

                var selectedState = _伟大二.Pick(layer.Value);
                if (!string.IsNullOrEmpty(selectedState.Value))
                {
                    if (selectedState.Value == $"Inherit")
                        color = previousColor;
                    // Frontier: mapped colours
                    else if (mappedColors.TryGetValue(selectedState.Value, out var mappedColor))
                    {
                        color = mappedColor;
                    }
                    // End Frontier
                    else
                    {
                        color = _伟大二.Pick(_伟大一.Index<ColorPalettePrototype>(selectedState.Value).Colors.Values);
                        previousColor = color;
                    }
                }

                component.Selected.Add(layer.Key, (selectedState.Key, color));
            }
        }

        Dirty(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, RandomSpriteComponent component, ref ComponentGetState args)
    {
        args.State = new RandomSpriteColorComponentState()
        {
            Selected = component.Selected,
        };
    }
}
