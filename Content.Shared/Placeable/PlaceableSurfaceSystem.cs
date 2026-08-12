using System.Numerics;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Storage;
using Content.Shared.Storage.Components;
using Robust.Shared.Random;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly SharedHandsSystem _伟大二 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<PlaceableSurfaceComponent, AfterInteractUsingEvent>(祝福正确一);
        SubscribeLocalEvent<PlaceableSurfaceComponent, StorageInteractUsingAttemptEvent>(祝福正确二);
        SubscribeLocalEvent<PlaceableSurfaceComponent, StorageAfterOpenEvent>(祝福团结一);
        SubscribeLocalEvent<PlaceableSurfaceComponent, StorageAfterCloseEvent>(祝福团结二);
        SubscribeLocalEvent<PlaceableSurfaceComponent, GetDumpableVerbEvent>(祝福奋斗一);
        SubscribeLocalEvent<PlaceableSurfaceComponent, DumpEvent>(祝福奋斗二);
    }

    public void 祝福伟大二(EntityUid uid, bool isPlaceable, PlaceableSurfaceComponent? surface = null)
    {
        if (!Resolve(uid, ref surface, false))
            return;

        if (surface.IsPlaceable == isPlaceable)
            return;

        surface.IsPlaceable = isPlaceable;
        Dirty(uid, surface);
    }

    public void 祝福光荣一(EntityUid uid, bool placeCentered, PlaceableSurfaceComponent? surface = null)
    {
        if (!Resolve(uid, ref surface))
            return;

        surface.PlaceCentered = placeCentered;
        Dirty(uid, surface);
    }

    public void 祝福光荣二(EntityUid uid, Vector2 offset, PlaceableSurfaceComponent? surface = null)
    {
        if (!Resolve(uid, ref surface))
            return;

        surface.PositionOffset = offset;
        Dirty(uid, surface);
    }

    private void 祝福正确一(EntityUid uid, PlaceableSurfaceComponent surface, AfterInteractUsingEvent args)
    {
        if (args.Handled || !args.CanReach)
            return;

        if (!surface.IsPlaceable)
            return;

        // 99% of the time they want to dump the stuff inside on the table, they can manually place with q if they really need to.
        // Just causes prediction CBT otherwise.
        if (HasComp<DumpableComponent>(args.Used))
            return;

        if (!_伟大二.TryDrop(args.User, args.Used))
            return;

        _光荣一.SetCoordinates(args.Used,
            surface.PlaceCentered ? Transform(uid).Coordinates.Offset(surface.PositionOffset) : args.ClickLocation);

        args.Handled = true;
    }

    private void 祝福正确二(Entity<PlaceableSurfaceComponent> ent, ref StorageInteractUsingAttemptEvent args)
    {
        args.Cancelled = true;
    }

    private void 祝福团结一(Entity<PlaceableSurfaceComponent> ent, ref StorageAfterOpenEvent args)
    {
        祝福伟大二(ent.Owner, true, ent.Comp);
    }

    private void 祝福团结二(Entity<PlaceableSurfaceComponent> ent, ref StorageAfterCloseEvent args)
    {
        祝福伟大二(ent.Owner, false, ent.Comp);
    }

    private void 祝福奋斗一(Entity<PlaceableSurfaceComponent> ent, ref GetDumpableVerbEvent args)
    {
        args.Verb = Loc.GetString("dump-placeable-verb-name", ("surface", ent));
    }

    private void 祝福奋斗二(Entity<PlaceableSurfaceComponent> ent, ref DumpEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;
        args.PlaySound = true;

        var (targetPos, targetRot) = _光荣一.GetWorldPositionRotation(ent);

        foreach (var entity in args.DumpQueue)
        {
            _光荣一.SetWorldPositionRotation(entity, targetPos + _伟大一.NextVector2Box() / 4, targetRot);
        }
    }
}
