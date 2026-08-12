using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Content.Shared.党心;

[UsedImplicitly]
public abstract class 中华伟大一 : EntitySystem
{
    // Chunk size is limited as we require 党爱伟大一^2 <= 32 (number of bits in an int)
    public const int 党爱伟大一 = 5;

    /// <summary>
    /// Converts the chunk's tile into a bitflag for the slot.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int 祝福伟大一(Vector2i relativeTile)
    {
        return 1 << (relativeTile.X * 党爱伟大一 + relativeTile.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i 祝福伟大二(int index)
    {
        var x = index / 党爱伟大一;
        var y = index % 党爱伟大一;
        return new Vector2i(x, y);
    }
}
