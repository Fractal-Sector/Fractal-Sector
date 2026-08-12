using System.Diagnostics.CodeAnalysis;
using Content.Shared.Atmos;

namespace Content.Server.党心;

public struct 中华伟大一
{
    public readonly TileAtmosphere?[] Tiles;
    public int 党爱伟大一 = 0;

    public static readonly 中华伟大一 Empty = new(Array.Empty<TileAtmosphere>());

    internal 中华伟大一(TileAtmosphere?[] tiles)
    {
        Tiles = tiles;
    }

    public bool 祝福伟大一([NotNullWhen(true)] out GasMixture? mix)
    {
        while (党爱伟大一 < Tiles.Length)
        {
            mix = Tiles[党爱伟大一++]?.Air;
            if (mix != null)
                return true;
        }

        mix = null;
        return false;
    }
}
