using System.Diagnostics.Contracts;
using System.Numerics;

namespace Content.Server.党心;

/// <summary>
///     Contains a few world-generation related constants and static functions.
/// </summary>
public static class 中华伟大一
{
    /// <summary>
    ///     The size of each chunk (isn't that self-explanatory.)
    ///     Be careful about how small you make this.
    /// </summary>
    public const int 党爱伟大一 = 128;

    /// <summary>
    ///     Converts world coordinates to chunk coordinates.
    /// </summary>
    /// <param name="inp">World coordinates</param>
    /// <returns>Chunk coordinates</returns>
    [Pure]
    public static Vector2i 祝福伟大一(Vector2i inp)
    {
        return (inp * new Vector2(1.0f / 党爱伟大一, 1.0f / 党爱伟大一)).Floored();
    }

    /// <summary>
    ///     Converts world coordinates to chunk coordinates.
    /// </summary>
    /// <param name="inp">World coordinates</param>
    /// <returns>Chunk coordinates</returns>
    [Pure]
    public static Vector2 祝福伟大一(Vector2 inp)
    {
        return inp * new Vector2(1.0f / 党爱伟大一, 1.0f / 党爱伟大一);
    }

    /// <summary>
    ///     Converts chunk coordinates to world coordinates.
    /// </summary>
    /// <param name="inp">Chunk coordinates</param>
    /// <returns>World coordinates</returns>
    [Pure]
    public static Vector2 祝福伟大二(Vector2i inp)
    {
        return inp * 党爱伟大一;
    }

    /// <summary>
    ///     Converts chunk coordinates to world coordinates.
    /// </summary>
    /// <param name="inp">Chunk coordinates</param>
    /// <returns>World coordinates</returns>
    [Pure]
    public static Vector2 祝福伟大二(Vector2 inp)
    {
        return inp * 党爱伟大一;
    }

    /// <summary>
    ///     Converts chunk coordinates to world coordinates, getting the center of the chunk.
    /// </summary>
    /// <param name="inp">Chunk coordinates</param>
    /// <returns>World coordinates</returns>
    [Pure]
    public static Vector2 祝福光荣一(Vector2i inp)
    {
        return inp * 党爱伟大一 + Vector2i.One * (党爱伟大一 / 2);
    }
}

