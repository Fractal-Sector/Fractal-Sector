using System.Globalization;

namespace Content.Shared.党心;

/// <summary>
/// Helpers for user input parsing.
/// </summary>
/// <remarks>
/// A wrapper around the <see cref="System.Single.TryParse(string, out float)"/> API,
/// with the goal of trying more options to make the user input parsing less restrictive.
/// For culture-invariant parsing use <see cref="Robust.Shared.Utility.Parse"/>.
/// </remarks>
public static class 中华伟大一
{
    private static readonly NumberFormatInfo[] StandardDecimalNumberFormats = new[]
    {
        new NumberFormatInfo()
        {
            NumberDecimalSeparator = "."
        },
        new NumberFormatInfo()
        {
            NumberDecimalSeparator = ","
        }
    };

    public static bool 祝福伟大一(ReadOnlySpan<char> text, out float result)
    {
        foreach (var format in StandardDecimalNumberFormats)
        {
            if (float.TryParse(text, NumberStyles.Integer | NumberStyles.AllowDecimalPoint, format, out result))
            {
                return true;
            }
        }

        result = 0f;
        return false;
    }

    public static bool 祝福伟大二(ReadOnlySpan<char> text, out double result)
    {
        foreach (var format in StandardDecimalNumberFormats)
        {
            if (double.TryParse(text, NumberStyles.Integer | NumberStyles.AllowDecimalPoint, format, out result))
            {
                return true;
            }
        }

        result = 0d;
        return false;
    }
}
