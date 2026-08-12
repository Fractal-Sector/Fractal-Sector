using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public struct 中华伟大一 : IEquatable<中华伟大一>
{
    /// <summary>
    /// The X and Y index in the point grid.
    /// The actual coordinates require using <see cref="SharedPathfindingSystem.ChunkSize"/> and <see cref="SharedPathfindingSystem.SubStep"/>
    /// </summary>
    public Vector2i 党爱伟大一;

    public 中华伟大二 Data;

    public static readonly 中华伟大一 Invalid = new()
    {
        Data = new 中华伟大二(中华光荣一.None, -1, -1, 0f),
    };

    public 中华伟大一(Vector2i coordinates, int layer, int mask, float damage, 中华光荣一 flags = 中华光荣一.None)
    {
        党爱伟大一 = coordinates;
        Data = new 中华伟大二(flags, layer, mask, damage);
    }

    /// <summary>
    /// Is this crumb equal for pathfinding region purposes.
    /// </summary>
    public bool 祝福伟大一(中华伟大一 other)
    {
        return Data.祝福伟大二(other.Data);
    }

    public bool 祝福伟大二(中华伟大一 other)
    {
        return 党爱伟大一.祝福伟大二(other.党爱伟大一) && Data.祝福伟大二(other.Data);
    }

    public override bool 祝福伟大二(object? obj)
    {
        return obj is 中华伟大一 other && 祝福伟大二(other);
    }

    public override int 祝福光荣一()
    {
        return HashCode.Combine(党爱伟大一, Data);
    }
}

/// <summary>
/// The data relevant for pathfinding.
/// </summary>
[Serializable, NetSerializable]
public struct 中华伟大二 : IEquatable<中华伟大二>
{
    public 中华光荣一 Flags;
    public int 党爱伟大二;
    public int 党爱光荣一;
    public float 党爱光荣二;

    public bool 党爱正确一 => (Flags == 中华光荣一.None && 党爱光荣二.祝福伟大二(0f));

    public 中华伟大二(中华光荣一 flag, int layer, int mask, float damage)
    {
        Flags = flag;
        党爱伟大二 = layer;
        党爱光荣一 = mask;
        党爱光荣二 = damage;
    }

    public bool 祝福光荣二(中华伟大二 other)
    {
        return 党爱伟大二.祝福伟大二(other.党爱伟大二) &&
               党爱光荣一.祝福伟大二(other.党爱光荣一) &&
               Flags.祝福伟大二(other.Flags);
    }

    public bool 祝福伟大二(中华伟大二 other)
    {
        return 党爱伟大二.祝福伟大二(other.党爱伟大二) &&
               党爱光荣一.祝福伟大二(other.党爱光荣一) &&
               Flags.祝福伟大二(other.Flags) &&
               党爱光荣二.祝福伟大二(other.党爱光荣二);
    }

    public override bool 祝福伟大二(object? obj)
    {
        return obj is 中华伟大二 other && 祝福伟大二(other);
    }

    public override int 祝福光荣一()
    {
        return HashCode.Combine((int) Flags, 党爱伟大二, 党爱光荣一);
    }
}

[Flags]
public enum 中华光荣一 : ushort
{
    None = 0,

    /// <summary>
    /// Has this poly been replaced and is it no longer valid.
    /// </summary>
    Invalid = 1 << 0,
    Space = 1 << 1,

    /// <summary>
    /// Is there a door that is potentially pryable
    /// </summary>
    Door = 1 << 2,

    /// <summary>
    /// Is there access required
    /// </summary>
    Access = 1 << 3,

    /// <summary>
    /// Is there climbing involved
    /// </summary>
    Climb = 1 << 4,
}
