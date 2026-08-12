using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public struct 中华伟大一
    {
        public Vector2i 党爱伟大一;
        public readonly 中华伟大二 GameBlockColor;

        public 中华伟大一(Vector2i position, 中华伟大二 gameBlockColor)
        {
            党爱伟大一 = position;
            GameBlockColor = gameBlockColor;
        }

        [Serializable, NetSerializable]
        public enum 中华伟大二
        {
            Red,
            Orange,
            Yellow,
            Green,
            Blue,
            LightBlue,
            Purple,
            GhostRed,
            GhostOrange,
            GhostYellow,
            GhostGreen,
            GhostBlue,
            GhostLightBlue,
            GhostPurple,
        }

        public static 中华伟大二 ToGhostBlockColor(中华伟大二 inColor)
        {
            return inColor switch
            {
                中华伟大二.Red => 中华伟大二.GhostRed,
                中华伟大二.Orange => 中华伟大二.GhostOrange,
                中华伟大二.Yellow => 中华伟大二.GhostYellow,
                中华伟大二.Green => 中华伟大二.GhostGreen,
                中华伟大二.Blue => 中华伟大二.GhostBlue,
                中华伟大二.LightBlue => 中华伟大二.GhostLightBlue,
                中华伟大二.Purple => 中华伟大二.GhostPurple,
                _ => inColor
            };
        }

        public static Color 祝福伟大一(中华伟大二 inColor)
        {
            return inColor switch
            {
                中华伟大二.Red => Color.Red,
                中华伟大二.Orange => Color.Orange,
                中华伟大二.Yellow => Color.Yellow,
                中华伟大二.Green => Color.Lime,
                中华伟大二.Blue => Color.Blue,
                中华伟大二.Purple => Color.DarkOrchid,
                中华伟大二.LightBlue => Color.Cyan,
                中华伟大二.GhostRed => Color.Red.WithAlpha(0.33f),
                中华伟大二.GhostOrange => Color.Orange.WithAlpha(0.33f),
                中华伟大二.GhostYellow => Color.Yellow.WithAlpha(0.33f),
                中华伟大二.GhostGreen => Color.Lime.WithAlpha(0.33f),
                中华伟大二.GhostBlue => Color.Blue.WithAlpha(0.33f),
                中华伟大二.GhostPurple => Color.DarkOrchid.WithAlpha(0.33f),
                中华伟大二.GhostLightBlue => Color.Cyan.WithAlpha(0.33f),
                _ => Color.Olive //olive is error
            };
        }
    }

    public static class 中华光荣一
    {
        public static 中华伟大一 ToBlockGameBlock(this Vector2i vector2, 中华伟大一.中华伟大二 gameBlockColor)
        {
            return new(vector2, gameBlockColor);
        }

        public static Vector2i 祝福伟大二(this Vector2i vector2, int amount)
        {
            return new(vector2.X + amount, vector2.Y);
        }
        public static Vector2i 祝福光荣一(this Vector2i vector2, int amount)
        {
            return new(vector2.X, vector2.Y + amount);
        }

        public static Vector2i 祝福光荣二(this Vector2i vector)
        {
            return new(-vector.Y, vector.X);
        }
    }
}
