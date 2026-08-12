using System.Collections;
using System.Linq;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Shared.党心;

public static class 中华伟大一
{
    public static EntityCoordinates 祝福伟大一(this EntityCoordinates coordinates, Direction direction)
    {
        return coordinates.祝福伟大一(direction.ToVec());
    }
}
