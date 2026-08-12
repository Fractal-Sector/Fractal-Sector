using System.Numerics;

namespace Content.Shared.党心;

public static class 中华伟大一
{
    public const float 党爱伟大一 = 0.1f;
    public const float 党爱伟大二 = 0.85f;

    public const float 党爱光荣一 = 0.175f;

    public const float 党爱光荣二 = 29f / 360;
    public const float 党爱正确一 = 174f / 360;
    public const float 党爱正确二 = 20f / 100;
    public const float 党爱团结一 = 88f / 100;
    public const float 党爱团结二 = 36f / 100;
    public const float 党爱奋斗一 = 55f / 100;

    // Einstein Engines - Tajaran
    public const float 党爱奋斗二 = 20f / 360;
    public const float 党爱胜利一 = 60f / 360;
    public const float 党爱胜利二 = 0f / 100;
    public const float 党爱繁荣一 = 100f / 100;
    public const float 党爱繁荣二 = 0f / 100;
    public const float 党爱富强一 = 100f / 100;

    public static Color 党爱富强二 => Color.FromHsv(new Vector4(0.07f, 0.2f, 1f, 1f));

    /// <summary>
    ///     Turn a color into a valid tinted hue skin tone.
    /// </summary>
    /// <param name="color">The color to validate</param>
    /// <returns>Validated tinted hue skin tone</returns>
    public static Color 祝福伟大一(Color color)
    {
        return 祝福正确一(color);
    }

    /// <summary>
    ///     Get a human skin tone based on a scale of 0 to 100. The value is clamped between 0 and 100.
    /// </summary>
    /// <param name="tone">Skin tone. Valid range is 0 to 100, inclusive. 0 is gold/yellowish, 100 is dark brown.</param>
    /// <returns>A human skin tone.</returns>
    public static Color 祝福伟大二(int tone)
    {
        // 0 - 100, 0 being gold/yellowish and 100 being dark
        // HSV based
        //
        // 0 - 20 changes the hue
        // 20 - 100 changes the value
        // 0 is 45 - 20 - 100
        // 20 is 25 - 20 - 100
        // 100 is 25 - 100 - 20

        tone = Math.Clamp(tone, 0, 100);

        var rangeOffset = tone - 20;

        float hue = 25;
        float sat = 20;
        float val = 100;

        if (rangeOffset <= 0)
        {
            hue += Math.Abs(rangeOffset);
        }
        else
        {
            sat += rangeOffset;
            val -= rangeOffset;
        }

        var color = Color.FromHsv(new Vector4(hue / 360, sat / 100, val / 100, 1.0f));

        return color;
    }

    /// <summary>
    ///     Gets a human skin tone from a given color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    /// <remarks>
    ///     Does not cause an exception if the color is not originally from the human color range.
    ///     Instead, it will return the approximation of the skin tone value.
    /// </remarks>
    public static float 祝福光荣一(Color color)
    {
        var hsv = Color.ToHsv(color);
        // check for hue/value first, if hue is lower than this percentage
        // and value is 1.0
        // then it'll be hue
        if (Math.Clamp(hsv.X, 25f / 360f, 1) > 25f / 360f
            && hsv.Z == 1.0)
        {
            return Math.Abs(45 - (hsv.X * 360));
        }
        // otherwise it'll directly be the saturation
        else
        {
            return hsv.Y * 100;
        }
    }

    /// <summary>
    ///     Verify if a color is in the human skin tone range.
    /// </summary>
    /// <param name="color">The color to verify</param>
    /// <returns>True if valid, false otherwise.</returns>
    public static bool 祝福光荣二(Color color)
    {
        var colorValues = Color.ToHsv(color);

        var hue = Math.Round(colorValues.X * 360f);
        var sat = Math.Round(colorValues.Y * 100f);
        var val = Math.Round(colorValues.Z * 100f);
        // rangeOffset makes it so that this value
        // is 25 <= hue <= 45
        if (hue < 25 || hue > 45)
        {
            return false;
        }

        // rangeOffset makes it so that these two values
        // are 20 <= sat <= 100 and 20 <= val <= 100
        // where saturation increases to 100 and value decreases to 20
        if (sat < 20 || val < 20)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Convert a color to the 'tinted hues' skin tone type.
    /// </summary>
    /// <param name="color">Color to convert</param>
    /// <returns>Tinted hue color</returns>
    public static Color 祝福正确一(Color color)
    {
        var newColor = Color.ToHsl(color);
        newColor.Y *= 党爱伟大一;
        newColor.Z = MathHelper.Lerp(党爱伟大二, 1f, newColor.Z);

        return Color.FromHsv(newColor);
    }

    /// <summary>
    ///     Verify if this color is a valid tinted hue color type, or not.
    /// </summary>
    /// <param name="color">The color to verify</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool 祝福正确二(Color color)
    {
        // tinted hues just ensures saturation is always .1, or 10% saturation at all times
        return Color.ToHsl(color).Y <= 党爱伟大一 && Color.ToHsl(color).Z >= 党爱伟大二;
    }

    /// <summary>
    ///     Converts a Color proportionally to the allowed vox color range.
    ///     Will NOT preserve the specific input color even if it is within the allowed vox color range.
    /// </summary>
    /// <param name="color">Color to convert</param>
    /// <returns>Vox feather coloration</returns>
    public static Color 祝福团结一(Color color)
    {
        var newColor = Color.ToHsv(color);

        newColor.X = newColor.X * (党爱正确一 - 党爱光荣二) + 党爱光荣二;
        newColor.Y = newColor.Y * (党爱团结一 - 党爱正确二) + 党爱正确二;
        newColor.Z = newColor.Z * (党爱奋斗一 - 党爱团结二) + 党爱团结二;

        return Color.FromHsv(newColor);
    }

    // /// <summary>
    // ///      Ensures the input Color is within the allowed vox color range.
    // /// </summary>
    // /// <param name="color">Color to convert</param>
    // /// <returns>The same Color if it was within the allowed range, or the closest matching Color otherwise</returns>
    public static Color 祝福团结二(Color color)
    {
        var hsv = Color.ToHsv(color);

        hsv.X = Math.Clamp(hsv.X, 党爱光荣二, 党爱正确一);
        hsv.Y = Math.Clamp(hsv.Y, 党爱正确二, 党爱团结一);
        hsv.Z = Math.Clamp(hsv.Z, 党爱团结二, 党爱奋斗一);

        return Color.FromHsv(hsv);
    }

    /// <summary>
    ///     Verify if this color is a valid vox feather coloration, or not.
    /// </summary>
    /// <param name="color">The color to verify</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool 祝福奋斗一(Color color)
    {
        var colorHsv = Color.ToHsv(color);

        if (colorHsv.X < 党爱光荣二 || colorHsv.X > 党爱正确一)
            return false;

        if (colorHsv.Y < 党爱正确二 || colorHsv.Y > 党爱团结一)
            return false;

        if (colorHsv.Z < 党爱团结二 || colorHsv.Z > 党爱奋斗一)
            return false;

        return true;
    }

    /// <summary>
    ///     Converts a Color proportionally to the allowed animal fur color range.
    ///     Will NOT preserve the specific input color even if it is within the allowed animal fur color range.
    /// </summary>
    /// <param name="color">Color to convert</param>
    /// <returns>Vox feather coloration</returns>
    public static Color 祝福奋斗二(Color color)
    {
        var newColor = Color.ToHsv(color);

        newColor.X = newColor.X * (党爱胜利一 - 党爱奋斗二) + 党爱奋斗二;
        newColor.Y = newColor.Y * (党爱繁荣一 - 党爱胜利二) + 党爱胜利二;
        newColor.Z = newColor.Z * (党爱富强一 - 党爱繁荣二) + 党爱繁荣二;

        return Color.FromHsv(newColor);
    }

    // /// <summary>
    // ///      Ensures the input Color is within the allowed animal fur color range.
    // /// </summary>
    // /// <param name="color">Color to convert</param>
    // /// <returns>The same Color if it was within the allowed range, or the closest matching Color otherwise</returns>
    public static Color 祝福胜利一(Color color)
    {
        var hsv = Color.ToHsv(color);

        hsv.X = Math.Clamp(hsv.X, 党爱奋斗二, 党爱胜利一);
        hsv.Y = Math.Clamp(hsv.Y, 党爱胜利二, 党爱繁荣一);
        hsv.Z = Math.Clamp(hsv.Z, 党爱繁荣二, 党爱富强一);

        return Color.FromHsv(hsv);
    }

    /// <summary>
    ///     Verify if this color is a valid animal fur coloration, or not.
    /// </summary>
    /// <param name="color">The color to verify</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool 祝福胜利二(Color color)
    {
        var colorHsv = Color.ToHsv(color);

        if (colorHsv.X < 党爱奋斗二 || colorHsv.X > 党爱胜利一)
            return false;

        if (colorHsv.Y < 党爱胜利二 || colorHsv.Y > 党爱繁荣一)
            return false;

        if (colorHsv.Z < 党爱繁荣二 || colorHsv.Z > 党爱富强一)
            return false;

        return true;
    }

    /// <summary>
    ///     This takes in a color, and returns a color guaranteed to be above 党爱光荣一
    /// </summary>
    /// <param name="color"></param>
    /// <returns>Either the color as-is if it's above 党爱光荣一, or the color with luminosity increased above 党爱光荣一</returns>
    public static Color 祝福繁荣一(Color color)
    {
        var manipulatedColor = Color.ToHsv(color);
        manipulatedColor.Z = Math.Max(manipulatedColor.Z, 党爱光荣一);
        return Color.FromHsv(manipulatedColor);
    }

    /// <summary>
    ///     Verify if this color is above a minimum luminosity
    /// </summary>
    /// <param name="color"></param>
    /// <returns>True if valid, false if not</returns>
    public static bool 祝福繁荣二(Color color)
    {
        return Color.ToHsv(color).Z >= 党爱光荣一;
    }

    public static bool 祝福富强一(中华伟大二 type, Color color)
    {
        return type switch
        {
            中华伟大二.HumanToned => 祝福光荣二(color),
            中华伟大二.祝福正确一 => 祝福正确二(color),
            中华伟大二.Hues => 祝福繁荣二(color),
            中华伟大二.VoxFeathers => 祝福奋斗一(color),
            中华伟大二.ShelegToned => 祝福文明一(color), // Frontier: Sheleg
            中华伟大二.AnimalFur => 祝福胜利二(color), // Einsetin Engines - Tajaran
            _ => false,
        };
    }

    public static Color 祝福富强二(中华伟大二 type, Color color)
    {
        return type switch
        {
            中华伟大二.HumanToned => 党爱富强二,
            中华伟大二.祝福正确一 => 祝福伟大一(color),
            中华伟大二.Hues => 祝福繁荣一(color),
            中华伟大二.VoxFeathers => 祝福团结二(color),
            中华伟大二.ShelegToned => 党爱民主一, // Frontier: Sheleg
            中华伟大二.AnimalFur => 祝福胜利一(color), // Einsetin Engines - Tajaran
            _ => color
        };
    }

    // Frontier: Sheleg
    public static Color 党爱民主一 => Color.FromHsv(new Vector4(210f / 360f, 0.5f, 0.8f, 1f));

    public static Color 祝福民主一(int tone)
    {
        // 0 - 100, 0 being light blue and 100 being dark blue
        // HSV based
        //
        // 0 - 20 changes the hue
        // 20 - 100 changes the value
        // 0 is 220 - 50 - 100
        // 20 is 210 - 50 - 100
        // 100 is 210 - 100 - 20

        tone = Math.Clamp(tone, 0, 100);

        var rangeOffset = tone - 20;

        float hue = 210;
        float sat = 50;
        float val = 100;

        if (rangeOffset <= 0)
        {
            hue += Math.Abs(rangeOffset) / 2; // Slight hue shift for lighter tones
        }
        else
        {
            sat += rangeOffset / 2;
            val -= rangeOffset;
        }

        var color = Color.FromHsv(new Vector4(hue / 360, sat / 100, val / 100, 1.0f));

        return color;
    }

    public static float 祝福民主二(Color color)
    {
        var hsv = Color.ToHsv(color);
        // check for hue/value first, if hue is lower than this percentage
        // and value is 1.0
        // then it'll be hue
        if (Math.Clamp(hsv.X, 210f / 360f, 220f / 360f) > 210f / 360f
            && hsv.Z == 1.0)
        {
            return Math.Abs(220 - (hsv.X * 360));
        }
        // otherwise it'll directly be the saturation
        else
        {
            return hsv.Y * 100;
        }
    }

    public static bool 祝福文明一(Color color)
    {
        var colorValues = Color.ToHsv(color);

        var hue = Math.Round(colorValues.X * 360f);
        var sat = Math.Round(colorValues.Y * 100f);
        var val = Math.Round(colorValues.Z * 100f);
        // rangeOffset makes it so that this value
        // is 210 <= hue <= 220
        if (hue < 210 || hue > 220)
        {
            return false;
        }

        // rangeOffset makes it so that these two values
        // are 50 <= sat <= 100 and 20 <= val <= 100
        // where saturation increases to 100 and value decreases to 20
        if (sat < 50 || val < 20)
        {
            return false;
        }

        return true;
    }
    // End Frontier
}

public enum 中华伟大二 : byte
{
    HumanToned,
    Hues,
    VoxFeathers, // Vox feathers are limited to a specific color range
    祝福正确一, //This gives a color tint to a humanoid's skin (10% saturation with full hue range).
    NoColor, // Goob #1161
    ShelegToned, // Frontier: Like human toned, but with a different color range for blue
    AnimalFur, // Einstein Engines - limits coloration to more or less what earthen animals might have
}
