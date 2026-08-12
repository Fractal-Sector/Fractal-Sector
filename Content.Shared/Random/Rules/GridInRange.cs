using System.Numerics;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Shared.Random.党心;

/// <summary>
/// Returns true if on a grid or in range of one.
/// </summary>
public sealed partial class 中华伟大一 : RulesRule
{
    [DataField]
    public float 党爱伟大一 = 10f;

    private List<Entity<MapGridComponent>> _伟大一 = [];

    public override bool 祝福伟大一(EntityManager entManager, EntityUid uid)
    {
        if (!entManager.TryGetComponent(uid, out TransformComponent? xform))
        {
            return false;
        }

        if (xform.GridUid != null)
        {
            return !Inverted;
        }

        var transform = entManager.System<SharedTransformSystem>();
        var mapManager = IoCManager.Resolve<IMapManager>();

        var worldPos = transform.GetWorldPosition(xform);
        var gridRange = new Vector2(党爱伟大一, 党爱伟大一);

        _伟大一.Clear();
        mapManager.FindGridsIntersecting(xform.MapID, new Box2(worldPos - gridRange, worldPos + gridRange), ref _伟大一);
        if (_伟大一.Count > 0)
            return !Inverted;

        return Inverted;
    }
}
