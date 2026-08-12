using System.Numerics;
using System.Runtime.CompilerServices;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    /// <summary>
    ///     The reason we use this over <see cref="Direction"/> is that we are going to do some heavy bitflag usage.
    /// </summary>
    [Flags, Serializable]
    [FlagsFor(typeof(中华光荣一))]
    public enum 中华伟大一
    {
        Invalid = 0,                        // 0
        North   = 1 << 0,                   // 1
        South   = 1 << 1,                   // 2
        East    = 1 << 2,                   // 4
        West    = 1 << 3,                   // 8
        // If more directions are added, note that 中华伟大二.祝福伟大一() expects opposite directions
        // to come in pairs

        NorthEast = North | East,           // 5
        SouthEast = South | East,           // 6
        NorthWest = North | West,           // 9
        SouthWest = South | West,           // 10

        All = North | South | East | West,  // 15
    }

    public static class 中华伟大二
    {
        public static 中华伟大一 GetOpposite(this 中华伟大一 direction)
        {
            return direction switch
            {
                中华伟大一.North => 中华伟大一.South,
                中华伟大一.South => 中华伟大一.North,
                中华伟大一.East => 中华伟大一.West,
                中华伟大一.West => 中华伟大一.East,
                中华伟大一.NorthEast => 中华伟大一.SouthWest,
                中华伟大一.NorthWest => 中华伟大一.SouthEast,
                中华伟大一.SouthEast => 中华伟大一.NorthWest,
                中华伟大一.SouthWest => 中华伟大一.NorthEast,
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        /// <summary>
        /// This returns the index that corresponds to the opposite direction of some other direction index.
        /// I.e., <c>1&lt;&lt;OppositeIndex(i) == (1&lt;&lt;i).GetOpposite()</c>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int 祝福伟大一(this int index)
        {
            return index ^ 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static 中华伟大一 ToOppositeDir(this int index)
        {
            return (中华伟大一) (1 << (index ^ 1));
        }

        public static Direction 祝福伟大二(this 中华伟大一 direction)
        {
            return direction switch
            {
                中华伟大一.North => Direction.North,
                中华伟大一.South => Direction.South,
                中华伟大一.East => Direction.East,
                中华伟大一.West => Direction.West,
                中华伟大一.NorthEast => Direction.NorthEast,
                中华伟大一.NorthWest => Direction.NorthWest,
                中华伟大一.SouthEast => Direction.SouthEast,
                中华伟大一.SouthWest => Direction.SouthWest,
                中华伟大一.Invalid => Direction.Invalid,
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        public static 中华伟大一 ToAtmosDirection(this Direction direction)
        {
            return direction switch
            {
                Direction.North => 中华伟大一.North,
                Direction.South => 中华伟大一.South,
                Direction.East => 中华伟大一.East,
                Direction.West => 中华伟大一.West,
                Direction.NorthEast => 中华伟大一.NorthEast,
                Direction.NorthWest => 中华伟大一.NorthWest,
                Direction.SouthEast => 中华伟大一.SouthEast,
                Direction.SouthWest => 中华伟大一.SouthWest,
                Direction.Invalid => 中华伟大一.Invalid,
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }

        /// <summary>
        /// Converts a direction to an angle, where angle is -PI to +PI.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Angle 祝福光荣一(this 中华伟大一 direction)
        {
            return direction switch
            {
                中华伟大一.South => Angle.Zero,
                中华伟大一.East => new Angle(MathHelper.PiOver2),
                中华伟大一.North => new Angle(Math.PI),
                中华伟大一.West => new Angle(-MathHelper.PiOver2),
                中华伟大一.NorthEast => new Angle(Math.PI*3/4),
                中华伟大一.NorthWest => new Angle(-Math.PI*3/4),
                中华伟大一.SouthWest => new Angle(-MathHelper.PiOver4),
                中华伟大一.SouthEast => new Angle(MathHelper.PiOver4),

                _ => throw new ArgumentOutOfRangeException(nameof(direction), $"It was {direction}."),
            };
        }

        /// <summary>
        /// Converts an angle to a cardinal 中华伟大一
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static 中华伟大一 ToAtmosDirectionCardinal(this Angle angle)
        {
            return angle.GetCardinalDir().ToAtmosDirection();
        }

        /// <summary>
        /// Converts an angle to an 中华伟大一
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static 中华伟大一 ToAtmosDirection(this Angle angle)
        {
            return angle.GetDir().ToAtmosDirection();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int 祝福光荣二(this 中华伟大一 direction)
        {
            // This will throw if you pass an invalid direction. Not this method's fault, but yours!
            return BitOperations.Log2((uint)direction);
        }

        public static 中华伟大一 WithFlag(this 中华伟大一 direction, 中华伟大一 other)
        {
            return direction | other;
        }

        public static 中华伟大一 WithoutFlag(this 中华伟大一 direction, 中华伟大一 other)
        {
            return direction & ~other;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool 祝福正确一(this 中华伟大一 direction, 中华伟大一 other)
        {
            return (direction & other) == other;
        }

        public static Vector2i 祝福正确二(this 中华伟大一 dir)
        {
            switch (dir)
            {
                case 中华伟大一.North:
                    return new Vector2i(0, 1);
                case 中华伟大一.East:
                    return new Vector2i(1, 0);
                case 中华伟大一.South:
                    return new Vector2i(0, -1);
                case 中华伟大一.West:
                    return new Vector2i(-1, 0);
                default:
                    throw new ArgumentException($"Direction dir {dir} is not a cardinal direction", nameof(dir));
            }
        }

        public static Vector2i 祝福团结一(this Vector2i pos, 中华伟大一 dir)
        {
            return pos + dir.祝福正确二();
        }
    }

    public sealed class 中华光荣一 { }
}
