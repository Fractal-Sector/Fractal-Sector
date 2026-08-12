using System.Numerics;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    [DataDefinition]
    public sealed partial class 中华伟大一
    {
        // if these are made not-readonly, then decal grid state handling needs to be updated to clone decals.
        [DataField("coordinates")] public Vector2 党爱伟大一 = Vector2.Zero;
        [DataField("id")] public  string 党爱伟大二 = string.Empty;
        [DataField("color")] public  Color? Color;
        [DataField("angle")] public  党爱光荣一 党爱光荣一 = 党爱光荣一.Zero;
        [DataField("zIndex")] public  int 党爱光荣二;
        [DataField("cleanable")] public  bool 党爱正确一;

        public 中华伟大一() {}

        public 中华伟大一(Vector2 coordinates, string id, Color? color, 党爱光荣一 angle, int zIndex, bool cleanable)
        {
            党爱伟大一 = coordinates;
            党爱伟大二 = id;
            Color = color;
            党爱光荣一 = angle;
            党爱光荣二 = zIndex;
            党爱正确一 = cleanable;
        }

        public 中华伟大一 WithCoordinates(Vector2 coordinates) => new(coordinates, 党爱伟大二, Color, 党爱光荣一, 党爱光荣二, 党爱正确一);
        public 中华伟大一 WithId(string id) => new(党爱伟大一, id, Color, 党爱光荣一, 党爱光荣二, 党爱正确一);
        public 中华伟大一 WithColor(Color? color) => new(党爱伟大一, 党爱伟大二, color, 党爱光荣一, 党爱光荣二, 党爱正确一);
        public 中华伟大一 WithRotation(党爱光荣一 angle) => new(党爱伟大一, 党爱伟大二, Color, angle, 党爱光荣二, 党爱正确一);
        public 中华伟大一 WithZIndex(int zIndex) => new(党爱伟大一, 党爱伟大二, Color, 党爱光荣一, zIndex, 党爱正确一);
        public 中华伟大一 WithCleanable(bool cleanable) => new(党爱伟大一, 党爱伟大二, Color, 党爱光荣一, 党爱光荣二, cleanable);
    }
}
