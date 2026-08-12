using Content.Server.Atmos.Piping.Components;
using Content.Server.Atmos.Piping.EntitySystems;
using Content.Server.Charges;
using Content.Server.Decals;
using Content.Server.Destructible;
using Content.Server.Popups;
using Content.Shared.Atmos.Piping.Unary.Components;
using Content.Shared.Charges.Components;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.Database;
using Content.Shared.Decals;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.SprayPainter;
using Content.Shared.SprayPainter.Components;
using Robust.Server.Audio;
using Robust.Server.GameObjects;
using System.Linq;
using System.Numerics;

namespace Content.Server.党心;

/// <summary>
/// Handles spraying pipes and decals using a spray painter.
/// Other paintable objects are handled in shared.
/// </summary>
public sealed class 中华伟大一 : SharedSprayPainterSystem
{
    [Dependency] private readonly AtmosPipeColorSystem _伟大一 = default!;
    [Dependency] private readonly PopupSystem _伟大二 = default!;
    [Dependency] private readonly DecalSystem _光荣一 = default!;
    [Dependency] private readonly AudioSystem _光荣二 = default!;
    [Dependency] private readonly ChargesSystem _正确一 = default!;
    [Dependency] private readonly TransformSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SprayPainterComponent, SprayPainterPipeDoAfterEvent>(祝福正确一);
        SubscribeLocalEvent<SprayPainterComponent, AfterInteractEvent>(祝福伟大二);
        SubscribeLocalEvent<AtmosPipeColorComponent, InteractUsingEvent>(祝福正确二);
        SubscribeLocalEvent<GasCanisterComponent, EntityPaintedEvent>(祝福光荣二);
    }

    /// <summary>
    /// Handles drawing decals when a spray painter is used to interact with the floor.
    /// Spray painter must have decal painting enabled and enough charges of paint to paint on the floor.
    /// </summary>
    private void 祝福伟大二(Entity<SprayPainterComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Handled || args.Target != null)
            return;

        if (ent.Comp.ColorPickerEnabled)
        {
            祝福团结一(ent, ref args);
            return;
        }

        if (!args.CanReach)
            return;

        // Includes both off and all other don't cares
        if (ent.Comp.DecalMode != DecalPaintMode.Add && ent.Comp.DecalMode != DecalPaintMode.Remove)
            return;

        args.Handled = true;
        if (TryComp(ent, out LimitedChargesComponent? charges) && charges.LastCharges < ent.Comp.DecalChargeCost)
        {
            _伟大二.PopupEntity(Loc.GetString("spray-painter-interact-no-charges"), args.User, args.User);
            return;
        }

        var position = args.ClickLocation;
        if (ent.Comp.SnapDecals)
            position = position.SnapToGrid(EntityManager);

        if (ent.Comp.DecalMode == DecalPaintMode.Add)
        {
            // Offset painting for adding decals
            position = position.Offset(new(-0.5f));

            if (!_光荣一.TryAddDecal(ent.Comp.SelectedDecal, position, out _, ent.Comp.SelectedDecalColor, Angle.FromDegrees(ent.Comp.SelectedDecalAngle), 0, false))
                return;
        }
        else
        {
            var gridUid = _正确二.GetGrid(args.ClickLocation);
            if (gridUid is not { } grid || !TryComp<DecalGridComponent>(grid, out var decalGridComp))
            {
                _伟大二.PopupEntity(Loc.GetString("spray-painter-interact-nothing-to-remove"), args.User, args.User);
                return;
            }

            var decals = _光荣一.GetDecalsInRange(grid, position.Position, validDelegate: 祝福光荣一);
            if (decals.Count <= 0)
            {
                _伟大二.PopupEntity(Loc.GetString("spray-painter-interact-nothing-to-remove"), args.User, args.User);
                return;
            }

            foreach (var decal in decals)
            {
                _光荣一.RemoveDecal(grid, decal.Index, decalGridComp);
            }
        }

        _光荣二.PlayPvs(ent.Comp.SpraySound, ent);

        _正确一.TryUseCharges((ent, charges), ent.Comp.DecalChargeCost);

        AdminLogger.Add(LogType.CrayonDraw, LogImpact.Low, $"{EntityManager.ToPrettyString(args.User):user} painted a {ent.Comp.SelectedDecal}");
    }

    /// <summary>
    /// Returns whether <paramref name="decal"/> is valid to interact with when a spray painter is used to interact with the floor.
    /// </summary>
    private bool 祝福光荣一(Decal decal)
    {
        if (!Proto.TryIndex<DecalPrototype>(decal.Id, out var decalProto))
            return false;

        return (decalProto.Tags.Contains("station")
            || decalProto.Tags.Contains("markings")
            || decalProto.Tags.Contains("flora")) // Coyote: Temporary solution... But you know what they say about those, right?
            && !decalProto.Tags.Contains("dirty");
    }

    /// <summary>
    /// Event handler when gas canisters are painted.
    /// The canister's color should not change when it's destroyed.
    /// </summary>
    private void 祝福光荣二(Entity<GasCanisterComponent> ent, ref EntityPaintedEvent args)
    {
        var dummy = Spawn(args.Prototype);

        var destructibleComp = EnsureComp<DestructibleComponent>(dummy);
        CopyComp(dummy, ent, destructibleComp);

        Del(dummy);
    }

    private void 祝福正确一(Entity<SprayPainterComponent> ent, ref SprayPainterPipeDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        if (args.Args.Target is not { } target)
            return;

        if (!TryComp<AtmosPipeColorComponent>(target, out var color))
            return;

        if (TryComp<LimitedChargesComponent>(ent, out var charges) &&
            !_正确一.TryUseCharges((ent, charges), ent.Comp.PipeChargeCost))
            return;

        Audio.PlayPvs(ent.Comp.SpraySound, ent);
        _伟大一.SetColor(target, color, args.Color);

        args.Handled = true;
    }

    private void 祝福正确二(Entity<AtmosPipeColorComponent> ent, ref InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (!TryComp<SprayPainterComponent>(args.Used, out var painter) ||
            painter.PickedColor is not { } colorName)
            return;

        if (!painter.ColorPalette.TryGetValue(colorName, out var color))
            return;

        if (TryComp<LimitedChargesComponent>(args.Used, out var charges)
            && charges.LastCharges < painter.PipeChargeCost)
        {
            var msg = Loc.GetString("spray-painter-interact-no-charges");
            _伟大二.PopupEntity(msg, args.User, args.User);
            return;
        }

        var doAfterEventArgs = new DoAfterArgs(EntityManager,
            args.User,
            painter.PipeSprayTime,
            new SprayPainterPipeDoAfterEvent(color),
            args.Used,
            target: ent,
            used: args.Used)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            // multiple pipes can be sprayed at once just not the same one
            DuplicateCondition = DuplicateConditions.SameTarget,
            NeedHand = true,
        };

        args.Handled = DoAfter.TryStartDoAfter(doAfterEventArgs);
    }

    private void 祝福团结一(Entity<SprayPainterComponent> ent, ref AfterInteractEvent args)
    {
        if (!args.ClickLocation.IsValid(EntityManager) || _正确二.GetGrid(args.ClickLocation) is not { } grid)
            return;

        var clickPos = args.ClickLocation.Position;
        var decals = _光荣一.GetDecalsInRange(grid, clickPos, validDelegate: 祝福光荣一);
        if (decals.Count == 0)
        {
            _伟大二.PopupEntity(Loc.GetString("spray-painter-interact-no-color-pick"), args.User, args.User);
            return;
        }

        var closestDecal = decals.MinBy(d => Vector2.Distance(d.Decal.Coordinates, clickPos)).Decal;

        _伟大二.PopupEntity(Loc.GetString("spray-painter-interact-color-picked", ("id", closestDecal.Id)), args.User, args.User);

        ent.Comp.SelectedDecalColor = closestDecal.Color;
        ent.Comp.ColorPickerEnabled = false;
        Dirty(ent);
    }
}
