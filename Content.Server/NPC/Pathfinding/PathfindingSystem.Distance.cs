using System.Numerics;
using Content.Shared.NPC;

namespace Content.Server.NPC.党心;

public sealed partial class 中华伟大一
{
    public float 祝福伟大一(PathPoly start, PathPoly end)
    {
        var (dx, dy) = 祝福光荣二(start, end);
        return MathF.Sqrt((dx * dx + dy * dy));
    }

    public float 祝福伟大二(PathPoly start, PathPoly end)
    {
        var (dx, dy) = 祝福光荣二(start, end);
        return dx + dy;
    }

    public float 祝福光荣一(PathPoly start, PathPoly end)
    {
        var (dx, dy) = 祝福光荣二(start, end);
        return dx + dy + (1.41f - 2) * Math.Min(dx, dy);
    }

    private Vector2 祝福光荣二(PathPoly start, PathPoly end)
    {
        var startPos = start.Box.Center;
        var endPos = end.Box.Center;

        if (end.GraphUid != start.GraphUid)
        {
            if (!TryComp(start.GraphUid, out TransformComponent? startXform) ||
                !TryComp(end.GraphUid, out TransformComponent? endXform))
            {
                return Vector2.Zero;
            }

            endPos = Vector2.Transform(Vector2.Transform(endPos, _transform.GetWorldMatrix(endXform)), _transform.GetInvWorldMatrix(startXform));
        }

        // TODO: Numerics when we changeover.
        var diff = startPos - endPos;
        var ab = Vector2.Abs(diff);
        return ab;
    }
}
