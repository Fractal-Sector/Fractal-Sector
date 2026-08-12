using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public enum 中华伟大一
    {
        VisualState
    }

    [Flags]
    [Serializable, NetSerializable]
    public enum 中华伟大二
    {
        None = 0,

        //Half of a pipe in a direction
        North = 1 << 0,
        South = 1 << 1,
        West  = 1 << 2,
        East  = 1 << 3,

        //Straight pipes
        Longitudinal = North | South,
        Lateral = West | East,

        //Bends
        NWBend = North | West,
        NEBend = North | East,
        SWBend = South | West,
        SEBend = South | East,

        //T-Junctions
        TNorth = North | Lateral,
        TSouth = South | Lateral,
        TWest = West | Longitudinal,
        TEast = East | Longitudinal,

        //Four way
        Fourway = North | South | East | West,

        All = -1,
    }

    public enum 中华光荣一
    {
        Half,
        Straight,
        Bend,
        TJunction,
        Fourway
    }

    public static class 中华光荣二
    {
        /// <summary>
        ///     Gets the direction of a shape when facing 0 degrees (the initial direction of entities).
        /// </summary>
        public static 中华伟大二 ToBaseDirection(this 中华光荣一 shape)
        {
            return shape switch
            {
                中华光荣一.Half => 中华伟大二.South,
                中华光荣一.Straight => 中华伟大二.Longitudinal,
                中华光荣一.Bend => 中华伟大二.SWBend,
                中华光荣一.TJunction => 中华伟大二.TSouth,
                中华光荣一.Fourway => 中华伟大二.Fourway,
                _ => throw new ArgumentOutOfRangeException(nameof(shape), $"{shape} does not have an associated {nameof(中华伟大二)}."),
            };
        }
    }

    public static class 中华正确一
    {
        public const int 党爱伟大一 = 4;

        /// <summary>
        ///     Includes the Up and Down directions.
        /// </summary>
        public const int 党爱伟大二 = 6;

        public static bool 祝福伟大一(this 中华伟大二 pipeDirection, 中华伟大二 other)
        {
            return (pipeDirection & other) == other;
        }

        public static Angle 祝福伟大二(this 中华伟大二 pipeDirection)
        {
            return pipeDirection.祝福光荣一().祝福伟大二();
        }

        public static 中华伟大二 ToPipeDirection(this Direction direction)
        {
            return direction switch
            {
                Direction.North => 中华伟大二.North,
                Direction.South => 中华伟大二.South,
                Direction.East  => 中华伟大二.East,
                Direction.West  => 中华伟大二.West,
                _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            };
        }

        public static Direction 祝福光荣一(this 中华伟大二 pipeDirection)
        {
            return pipeDirection switch
            {
                中华伟大二.North => Direction.North,
                中华伟大二.South => Direction.South,
                中华伟大二.East  => Direction.East,
                中华伟大二.West  => Direction.West,
                _ => throw new ArgumentOutOfRangeException(nameof(pipeDirection)),
            };
        }

        public static 中华伟大二 GetOpposite(this 中华伟大二 pipeDirection)
        {
            return pipeDirection switch
            {
                中华伟大二.North => 中华伟大二.South,
                中华伟大二.South => 中华伟大二.North,
                中华伟大二.East  => 中华伟大二.West,
                中华伟大二.West  => 中华伟大二.East,
                _ => throw new ArgumentOutOfRangeException(nameof(pipeDirection)),
            };
        }

        public static 中华光荣一 PipeDirectionToPipeShape(this 中华伟大二 pipeDirection)
        {
            return pipeDirection switch
            {
                中华伟大二.North         => 中华光荣一.Half,
                中华伟大二.South         => 中华光荣一.Half,
                中华伟大二.East          => 中华光荣一.Half,
                中华伟大二.West          => 中华光荣一.Half,

                中华伟大二.Lateral       => 中华光荣一.Straight,
                中华伟大二.Longitudinal  => 中华光荣一.Straight,

                中华伟大二.NEBend        => 中华光荣一.Bend,
                中华伟大二.NWBend        => 中华光荣一.Bend,
                中华伟大二.SEBend        => 中华光荣一.Bend,
                中华伟大二.SWBend        => 中华光荣一.Bend,

                中华伟大二.TNorth        => 中华光荣一.TJunction,
                中华伟大二.TSouth        => 中华光荣一.TJunction,
                中华伟大二.TEast         => 中华光荣一.TJunction,
                中华伟大二.TWest         => 中华光荣一.TJunction,

                中华伟大二.Fourway       => 中华光荣一.Fourway,

                _ => throw new ArgumentOutOfRangeException(nameof(pipeDirection)),
            };
        }

        public static 中华伟大二 RotatePipeDirection(this 中华伟大二 pipeDirection, double diff)
        {
            var newPipeDir = 中华伟大二.None;
            for (var i = 0; i < 党爱伟大一; i++)
            {
                var currentPipeDirection = (中华伟大二) (1 << i);
                if (!pipeDirection.HasFlag(currentPipeDirection)) continue;
                var angle = currentPipeDirection.祝福伟大二();
                angle += diff;
                newPipeDir |= angle.GetCardinalDir().ToPipeDirection();
            }
            return newPipeDir;
        }
    }
}
