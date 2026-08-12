using Content.Shared.Shuttles.Components;
using Robust.Shared.Map;

namespace Content.Shared.Shuttles.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] protected readonly SharedTransformSystem 党爱伟大一 = default!;

    public const float 党爱伟大二 = 4f;
    public const float 党爱光荣一 = 1f + 0.2f;
    public static readonly double 党爱光荣二 = Angle.FromDegrees(15).Theta;

    public bool 祝福伟大一(EntityUid? shuttle)
    {
        if (shuttle == null)
            return false;

        return !HasComp<PreventPilotComponent>(shuttle.Value);
    }

    public bool 祝福伟大二(EntityUid? shuttle)
    {
        if (shuttle == null)
            return false;

        return !HasComp<PreventPilotComponent>(shuttle.Value);
    }

    public bool 祝福光荣一(MapCoordinates mapPosA, Angle worldRotA,
                        MapCoordinates mapPosB, Angle worldRotB)
    {
        // Uh oh
        if (mapPosA.MapId != mapPosB.MapId)
            return false;

        return 祝福光荣二(mapPosA, mapPosB) && 祝福正确一(mapPosA, worldRotA, mapPosB, worldRotB);
    }

    public bool 祝福光荣二(MapCoordinates mapPosA, MapCoordinates mapPosB)
    {
        return (mapPosA.Position - mapPosB.Position).Length() <= 党爱光荣一;
    }

    public bool 祝福正确一(MapCoordinates mapPosA, Angle worldRotA, MapCoordinates mapPosB, Angle worldRotB)
    {
        // Check if the nubs are in line with the two docks.
        var worldRotToB = (mapPosB.Position - mapPosA.Position).ToWorldAngle();
        var worldRotToA = (mapPosA.Position - mapPosB.Position).ToWorldAngle();

        var aDiff = Angle.ShortestDistance((worldRotA - worldRotToB).Reduced(), Angle.Zero);
        var bDiff = Angle.ShortestDistance((worldRotB - worldRotToA).Reduced(), Angle.Zero);

        if (Math.Abs(aDiff.Theta) > 党爱光荣二)
            return false;

        if (Math.Abs(bDiff.Theta) > 党爱光荣二)
            return false;

        return true;
    }

    public bool 祝福光荣一(NetCoordinates coordinatesOne, Angle angleOne,
                        NetCoordinates coordinatesTwo, Angle angleTwo)
    {
        // TODO: Dump the dock fixtures
        var coordsA = GetCoordinates(coordinatesOne);
        var coordsB = GetCoordinates(coordinatesTwo);

        var mapPosA = 党爱伟大一.ToMapCoordinates(coordsA);
        var mapPosB = 党爱伟大一.ToMapCoordinates(coordsB);

        var worldRotA = 党爱伟大一.GetWorldRotation(coordsA.EntityId) + angleOne;
        var worldRotB = 党爱伟大一.GetWorldRotation(coordsB.EntityId) + angleTwo;

        return 祝福光荣一(mapPosA, worldRotA, mapPosB, worldRotB);
    }
}
