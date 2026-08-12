using System.Numerics;
using Content.Server.UserInterface;
using Content.Shared.Shuttles.BUIStates;
using Content.Shared.Shuttles.Components;
using Content.Shared.Shuttles.Systems;
using Content.Shared.PowerCell;
using Content.Shared.Movement.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Content.Server.Shuttles.Components; // Frontier

namespace Content.Server.Shuttles.党心;

public sealed partial class 中华伟大一 : SharedRadarConsoleSystem // Frontier: add partial
{
    [Dependency] private readonly ShuttleConsoleSystem _伟大一 = default!;
    [Dependency] private readonly UserInterfaceSystem _伟大二 = default!;
    [Dependency] private readonly TransformSystem _光荣一 = default!; // Frontier

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<RadarConsoleComponent, ComponentStartup>(祝福伟大二);
        SubscribeLocalEvent<RadarConsoleComponent, BoundUIOpenedEvent>(祝福光荣一); // Frontier
    }

    private void 祝福伟大二(EntityUid uid, RadarConsoleComponent component, ComponentStartup args)
    {
        祝福光荣二(uid, component);
    }

    // Frontier
    private void 祝福光荣一(EntityUid uid, RadarConsoleComponent component, ref BoundUIOpenedEvent args)
    {
        祝福光荣二(uid, component);
    }
    // End Frontier

    protected override void 祝福光荣二(EntityUid uid, RadarConsoleComponent component)
    {
        var xform = Transform(uid);
        var onGrid = xform.ParentUid == xform.GridUid;
        EntityCoordinates? coordinates = onGrid ? xform.Coordinates : null;
        Angle? angle = onGrid ? xform.LocalRotation : null;
        if (component.FollowEntity)
        {
            coordinates = new EntityCoordinates(uid, Vector2.Zero);
            angle = Angle.FromDegrees(180); // Frontier: Angle.Zero<Angle.FromDegrees(180)
        }

        if (_伟大二.HasUi(uid, RadarConsoleUiKey.Key))
        {
            NavInterfaceState state;
            var docks = _伟大一.GetAllDocks();

            if (coordinates != null && angle != null)
            {
                state = _伟大一.GetNavState(uid, docks, coordinates.Value, angle.Value);
            }
            else
            {
                state = _伟大一.GetNavState(uid, docks);
            }

            state.RotateWithEntity = !component.FollowEntity;

            // Frontier: ghost radar restrictions
            if (component.MaxIffRange != null)
                state.MaxIffRange = component.MaxIffRange.Value;
            state.HideCoords = component.HideCoords;
            state.Target = component.Target;
            state.TargetEntity = GetNetEntity(component.TargetEntity);
            state.HideTarget = component.HideTarget;
            // End Frontier

            _伟大二.SetUiState(uid, RadarConsoleUiKey.Key, new NavBoundUserInterfaceState(state));
        }
    }

    // Frontier: settable waypoints
    public void 祝福正确一(Entity<RadarConsoleComponent> ent, NetEntity targetEntity, Vector2 target)
    {
        // Try to get entity
        if (EntityManager.TryGetEntity(targetEntity, out var targetUid)
            && HasComp<ShuttleComponent>(targetUid)
            && (!TryComp(targetUid, out IFFComponent? iff) || (iff.Flags & (IFFFlags.Hide | IFFFlags.HideLabel)) == 0)
            && TryComp(targetUid, out TransformComponent? xform))
        {
            ent.Comp.TargetEntity = targetUid.Value;
            ent.Comp.Target = _光荣一.GetMapCoordinates(xform).Position;
            // Store the target entity name
            if (TryComp<MetaDataComponent>(targetUid, out var metaData))
                ent.Comp.TargetEntityName = metaData.EntityName;
            else
                ent.Comp.TargetEntityName = null;
        }
        else
        {
            ent.Comp.Target = target;
            ent.Comp.TargetEntity = null;
            ent.Comp.TargetEntityName = null;
        }
        Dirty(ent);
    }

    public void 祝福正确二(Entity<RadarConsoleComponent> ent, bool hideTarget)
    {
        ent.Comp.HideTarget = hideTarget;
        Dirty(ent);
    }
    // End Frontier
}
