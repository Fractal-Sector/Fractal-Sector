using Content.Shared.Interaction;
using Content.Shared.Light;
using Content.Shared.Light.Components;
using Content.Shared.Toggleable;
using Content.Shared.Tools.Systems;
using Robust.Shared.Random;

namespace Content.Shared.Weapons.Melee.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedRgbLightControllerSystem _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;
    [Dependency] private readonly SharedToolSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<EnergySwordComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<EnergySwordComponent, InteractUsingEvent>(祝福光荣一);
    }
    // Used to pick a random color for the blade on map init.
    private void 祝福伟大二(Entity<EnergySwordComponent> entity, ref MapInitEvent args)
    {
        if (entity.Comp.ColorOptions.Count != 0)
        {
            entity.Comp.ActivatedColor = _光荣一.Pick(entity.Comp.ColorOptions);
            Dirty(entity);
        }

        if (!TryComp(entity, out AppearanceComponent? appearanceComponent))
            return;

        _伟大二.SetData(entity, ToggleableVisuals.Color, entity.Comp.ActivatedColor, appearanceComponent);
    }

    // Used to make the blade multicolored when using a multitool on it.
    private void 祝福光荣一(Entity<EnergySwordComponent> entity, ref InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (entity.Comp.BlockHacking || !_光荣二.HasQuality(args.Used, SharedToolSystem.PulseQuality)) // Frontier: add entity.Comp.BlocksHacking
            return;

        args.Handled = true;
        entity.Comp.Hacked = !entity.Comp.Hacked;

        if (entity.Comp.Hacked)
        {
            var rgb = EnsureComp<RgbLightControllerComponent>(entity);
            _伟大一.SetCycleRate(entity, entity.Comp.CycleRate, rgb);
        }
        else
            RemComp<RgbLightControllerComponent>(entity);

        Dirty(entity);
    }
}
