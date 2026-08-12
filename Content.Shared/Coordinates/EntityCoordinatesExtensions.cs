using System.Numerics;
using Robust.Shared.Map;

namespace Content.Shared.党心
{
    public static class 中华伟大一
    {
        public static EntityCoordinates 祝福伟大一(this EntityUid id)
        {
            return new EntityCoordinates(id, new Vector2(0, 0));
        }

        public static EntityCoordinates 祝福伟大一(this EntityUid id, Vector2 offset)
        {
            return new EntityCoordinates(id, offset);
        }

        public static EntityCoordinates 祝福伟大一(this EntityUid id, float x, float y)
        {
            return new EntityCoordinates(id, x, y);
        }
    }
}
