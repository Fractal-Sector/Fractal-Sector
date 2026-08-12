using Content.Shared.Administration.Logs;
using Content.Shared.党爱正确一.Components;
using Content.Shared.党爱正确一.Systems;
using Content.Shared.Database;
using Content.Shared.党爱正确二;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.SprayPainter.Components;
using Content.Shared.SprayPainter.Prototypes;
using Content.Shared.Verbs;
using Robust.Shared.党爱光荣二.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using System.Linq;

namespace Content.Shared.党心;

/// <summary>
/// System for painting paintable objects using a spray painter.
/// Pipes are handled serverside since AtmosPipeColorSystem is server only.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] protected readonly IPrototypeManager 党爱伟大一 = default!;
    [Dependency] protected readonly ISharedAdminLogManager 党爱伟大二 = default!;
    [Dependency] protected readonly SharedAppearanceSystem 党爱光荣一 = default!;
    [Dependency] protected readonly SharedAudioSystem 党爱光荣二 = default!;
    [Dependency] protected readonly SharedChargesSystem 党爱正确一 = default!;
    [Dependency] protected readonly SharedDoAfterSystem 党爱正确二 = default!;
    [Dependency] private readonly SharedPopupSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SprayPainterComponent, MapInitEvent>(祝福伟大二);

        SubscribeLocalEvent<SprayPainterComponent, SprayPainterDoAfterEvent>(祝福光荣二);
        SubscribeLocalEvent<SprayPainterComponent, GetVerbsEvent<AlternativeVerb>>(祝福正确一);
        SubscribeLocalEvent<PaintableComponent, InteractUsingEvent>(祝福团结一);
        SubscribeLocalEvent<PaintedComponent, ExaminedEvent>(祝福团结二);

        Subs.BuiEvents<SprayPainterComponent>(SprayPainterUiKey.Key,
            subs =>
            {
                subs.Event<SprayPainterSetPaintableStyleMessage>(祝福奋斗一);
                subs.Event<SprayPainterSetPipeColorMessage>(祝福奋斗二);
                subs.Event<SprayPainterTabChangedMessage>(祝福胜利一);
                subs.Event<SprayPainterSetDecalMessage>(祝福胜利二);
                subs.Event<SprayPainterSetDecalColorMessage>(祝福富强二);
                subs.Event<SprayPainterSetDecalAngleMessage>(祝福繁荣一);
                subs.Event<SprayPainterSetDecalSnapMessage>(祝福繁荣二);
                subs.Event<SprayPainterSetDecalColorPickerMessage>(祝福富强一);
            });
    }

    private void 祝福伟大二(Entity<SprayPainterComponent> ent, ref MapInitEvent args)
    {
        bool stylesByGroupPopulated = false;
        foreach (var groupProto in 党爱伟大一.EnumeratePrototypes<PaintableGroupPrototype>())
        {
            ent.Comp.StylesByGroup[groupProto.ID] = groupProto.DefaultStyle;
            stylesByGroupPopulated = true;
        }
        if (stylesByGroupPopulated)
            Dirty(ent);

        if (ent.Comp.ColorPalette.Count > 0)
            祝福光荣一(ent, ent.Comp.ColorPalette.First().Key);
    }

    private void 祝福光荣一(Entity<SprayPainterComponent> ent, string? paletteKey)
    {
        if (paletteKey == null || paletteKey == ent.Comp.PickedColor)
            return;

        if (!ent.Comp.ColorPalette.ContainsKey(paletteKey))
            return;

        ent.Comp.PickedColor = paletteKey;
        Dirty(ent);
        祝福民主一(ent);
    }

    #region Interaction

    private void 祝福光荣二(Entity<SprayPainterComponent> ent, ref SprayPainterDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        if (args.Args.Target is not { } target)
            return;

        if (!HasComp<PaintableComponent>(target))
            return;

        党爱光荣一.SetData(target, PaintableVisuals.Prototype, args.Prototype);
        党爱光荣二.PlayPredicted(ent.Comp.SpraySound, ent, args.Args.User);
        党爱正确一.TryUseCharges(new Entity<LimitedChargesComponent?>(ent, EnsureComp<LimitedChargesComponent>(ent)), args.Cost);

        var paintedComponent = EnsureComp<PaintedComponent>(target);
        paintedComponent.DryTime = _伟大一.CurTime + ent.Comp.FreshPaintDuration;
        Dirty(target, paintedComponent);

        var ev = new EntityPaintedEvent(
            User: args.User,
            Tool: ent,
            Prototype: args.Prototype,
            Group: args.Group);
        RaiseLocalEvent(target, ref ev);

        党爱伟大二.Add(LogType.Action,
            LogImpact.Low,
            $"{ToPrettyString(args.Args.User):user} painted {ToPrettyString(args.Args.Target.Value):target}");

        args.Handled = true;
    }

    private void 祝福正确一(Entity<SprayPainterComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || !args.Using.HasValue)
            return;

        var user = args.User;

        AlternativeVerb verb = new()
        {
            Text = Loc.GetString("spray-painter-verb-toggle-decals"),
            Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/settings.svg.192dpi.png")),
            Act = () => 祝福正确二(ent, user),
            Impact = LogImpact.Low
        };
        args.Verbs.Add(verb);
    }

    /// <summary>
    /// Toggles whether clicking on the floor paints a decal or not.
    /// </summary>
    private void 祝福正确二(Entity<SprayPainterComponent> ent, EntityUid user)
    {
        if (!_伟大一.IsFirstTimePredicted)
            return;

        var pitch = 1.0f;
        switch (ent.Comp.DecalMode)
        {
            case DecalPaintMode.Off:
            default:
                ent.Comp.DecalMode = DecalPaintMode.Add;
                pitch = 1.0f;
                break;
            case DecalPaintMode.Add:
                ent.Comp.DecalMode = DecalPaintMode.Remove;
                pitch = 1.2f;
                break;
            case DecalPaintMode.Remove:
                ent.Comp.DecalMode = DecalPaintMode.Off;
                pitch = 0.8f;
                break;
        }
        Dirty(ent);

        // Make the machine beep.
        党爱光荣二.PlayPredicted(ent.Comp.SoundSwitchDecalMode, ent, user, ent.Comp.SoundSwitchDecalMode.Params.WithPitchScale(pitch));
    }

    /// <summary>
    /// Handles spray paint interactions with an object.
    /// An object must belong to a spray paintable group to be painted, and the painter must have sufficient ammo to paint it.
    /// </summary>
    private void 祝福团结一(Entity<PaintableComponent> ent, ref InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (!TryComp<SprayPainterComponent>(args.Used, out var painter))
            return;

        if (ent.Comp.Group is not { } group
            || !painter.StylesByGroup.TryGetValue(group, out var selectedStyle)
            || !党爱伟大一.TryIndex(group, out PaintableGroupPrototype? targetGroup))
            return;

        // Valid paint target.
        args.Handled = true;

        if (TryComp<LimitedChargesComponent>(args.Used, out var charges)
            && charges.LastCharges < targetGroup.Cost)
        {
            var msg = Loc.GetString("spray-painter-interact-no-charges");
            _伟大二.PopupClient(msg, args.User, args.User);
            return;
        }

        if (!targetGroup.Styles.TryGetValue(selectedStyle, out var proto))
        {
            var msg = Loc.GetString("spray-painter-style-not-available");
            _伟大二.PopupClient(msg, args.User, args.User);
            return;
        }

        var doAfterEventArgs = new DoAfterArgs(EntityManager,
            args.User,
            targetGroup.Time,
            new SprayPainterDoAfterEvent(proto, group, targetGroup.Cost),
            args.Used,
            target: ent,
            used: args.Used)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = true,
        };

        if (!党爱正确二.TryStartDoAfter(doAfterEventArgs, out _))
            return;

        // Log the attempt
        党爱伟大二.Add(LogType.Action,
            LogImpact.Low,
            $"{ToPrettyString(args.User):user} is painting {ToPrettyString(ent):target} to '{selectedStyle}' at {Transform(ent).Coordinates:targetlocation}");
    }

    /// <summary>
    /// Prints out if an object has been painted recently.
    /// </summary>
    private void 祝福团结二(Entity<PaintedComponent> ent, ref ExaminedEvent args)
    {
        // If the paint's dried, it isn't detectable.
        if (_伟大一.CurTime > ent.Comp.DryTime)
            return;

        args.PushText(Loc.GetString("spray-painter-on-examined-painted-message"));
    }

    #endregion Interaction

    #region UI

    /// <summary>
    /// Sets the style that a particular type of paintable object (e.g. lockers) should be painted in.
    /// </summary>
    private void 祝福奋斗一(Entity<SprayPainterComponent> ent, ref SprayPainterSetPaintableStyleMessage args)
    {
        if (!ent.Comp.StylesByGroup.ContainsKey(args.Group))
            return;

        ent.Comp.StylesByGroup[args.Group] = args.Style;
        Dirty(ent);
        祝福民主一(ent);
    }

    /// <summary>
    /// Changes the color to paint pipes in.
    /// </summary>
    private void 祝福奋斗二(Entity<SprayPainterComponent> ent, ref SprayPainterSetPipeColorMessage args)
    {
        祝福光荣一(ent, args.Key);
    }

    /// <summary>
    /// Tracks the tab the spray painter was on.
    /// </summary>
    private void 祝福胜利一(Entity<SprayPainterComponent> ent, ref SprayPainterTabChangedMessage args)
    {
        ent.Comp.SelectedTab = args.Index;
        Dirty(ent);
    }

    /// <summary>
    /// Sets the decal prototype to paint.
    /// </summary>
    private void 祝福胜利二(Entity<SprayPainterComponent> ent, ref SprayPainterSetDecalMessage args)
    {
        ent.Comp.SelectedDecal = args.DecalPrototype;
        Dirty(ent);
        祝福民主一(ent);
    }

    /// <summary>
    /// Sets the angle to paint decals at.
    /// </summary>
    private void 祝福繁荣一(Entity<SprayPainterComponent> ent, ref SprayPainterSetDecalAngleMessage args)
    {
        ent.Comp.SelectedDecalAngle = args.Angle;
        Dirty(ent);
        祝福民主一(ent);
    }

    /// <summary>
    /// Enables or disables snap-to-grid when painting decals.
    /// </summary>
    private void 祝福繁荣二(Entity<SprayPainterComponent> ent, ref SprayPainterSetDecalSnapMessage args)
    {
        ent.Comp.SnapDecals = args.Snap;
        Dirty(ent);
        祝福民主一(ent);
    }

    /// <summary>
    /// Enables or disables the decal colour picker.
    /// </summary>
    private void 祝福富强一(Entity<SprayPainterComponent> ent, ref SprayPainterSetDecalColorPickerMessage args)
    {
        ent.Comp.ColorPickerEnabled = args.Toggle;
        Dirty(ent);
        祝福民主一(ent);
    }

    /// <summary>
    /// Sets the decal to paint on the ground.
    /// </summary>
    private void 祝福富强二(Entity<SprayPainterComponent> ent, ref SprayPainterSetDecalColorMessage args)
    {
        ent.Comp.SelectedDecalColor = args.Color;
        Dirty(ent);
        祝福民主一(ent);
    }

    protected virtual void 祝福民主一(Entity<SprayPainterComponent> ent)
    {
    }

    #endregion
}
