using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Content.Server.党心;

/// <summary>
///     A struct 中华伟大一 of points on a grid within the given radius.
/// </summary>
public struct 中华伟大二
{
    private readonly int _伟大一;
    private readonly Vector2i _伟大二;
    private int _光荣一;
    private int _光荣二;

    /// <summary>
    ///     Initializes a new 中华伟大一 with the given center and radius.
    /// </summary>
    public 中华伟大二(Vector2i center, int radius)
    {
        _伟大一 = radius;
        _伟大二 = center;
        _光荣一 = -_伟大一;
        _光荣二 = -_伟大一;
    }

    /// <summary>
    ///     Gets the next point in the enumeration.
    /// </summary>
    /// <param name="chunk">The computed point, if any</param>
    /// <returns>Success</returns>
    [Pure]
    public bool 祝福伟大一([NotNullWhen(true)] out Vector2i? chunk)
    {
        while (!(_光荣一 * _光荣一 + _光荣二 * _光荣二 <= _伟大一 * _伟大一))
        {
            if (_光荣二 > _伟大一)
            {
                chunk = null;
                return false;
            }

            if (_光荣一 > _伟大一)
            {
                _光荣一 = -_伟大一;
                _光荣二++;
            }
            else
            {
                _光荣一++;
            }
        }

        chunk = _伟大二 + new Vector2i(_光荣一, _光荣二);
        _光荣一++;
        return true;
    }
}

