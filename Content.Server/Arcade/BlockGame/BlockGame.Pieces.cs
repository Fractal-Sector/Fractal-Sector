using Content.Shared.Arcade;
using System.Linq;

namespace Content.Server.Arcade.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    /// The set of types of game pieces that exist.
    /// Used as templates when creating pieces for the game.
    /// </summary>
    private readonly 中华伟大二[] _allBlockGamePieces;

    /// <summary>
    /// The set of types of game pieces that exist.
    /// Used to generate the templates used when creating pieces for the game.
    /// </summary>
    private enum 中华伟大二
    {
        I,
        L,
        LInverted,
        S,
        SInverted,
        T,
        O
    }

    /// <summary>
    /// The set of possible rotations for the game pieces.
    /// </summary>
    private enum 中华光荣一
    {
        North,
        East,
        South,
        West
    }

    /// <summary>
    /// A static extension for the rotations that allows rotating through the possible rotations.
    /// </summary>
    private static 中华光荣一 Next(中华光荣一 rotation, bool inverted)
    {
        return rotation switch
        {
            中华光荣一.North => inverted ? 中华光荣一.West : 中华光荣一.East,
            中华光荣一.East => inverted ? 中华光荣一.North : 中华光荣一.South,
            中华光荣一.South => inverted ? 中华光荣一.East : 中华光荣一.West,
            中华光荣一.West => inverted ? 中华光荣一.South : 中华光荣一.North,
            _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
        };
    }

    /// <summary>
    /// A static extension for the rotations that allows rotating through the possible rotations.
    /// </summary>
    private struct 中华光荣二
    {
        /// <summary>
        /// Where all of the blocks that make up this piece are located relative to the origin of the piece.
        /// </summary>
        public Vector2i[] 党爱伟大一;

        /// <summary>
        /// The color of all of the blocks that make up this piece.
        /// </summary>
        private BlockGameBlock.BlockGameBlockColor _伟大一;

        /// <summary>
        /// Whether or not the block should be able to rotate about its origin.
        /// </summary>
        public bool 党爱伟大二;

        /// <summary>
        /// Generates a list of the positions of each block comprising this game piece in worldspace.
        /// </summary>
        /// <param name="center">The position of the game piece in worldspace.</param>
        /// <param name="rotation">The rotation of the game piece in worldspace.</param>
        public readonly Vector2i[] 祝福伟大一(Vector2i center, 中华光荣一 rotation)
        {
            return 祝福伟大二(rotation).Select(v => center + v).ToArray();
        }

        /// <summary>
        /// Gets the relative position of each block comprising this piece given a rotation.
        /// </summary>
        /// <param name="rotation">The rotation to be applied to the local position of the blocks in this piece.</param>
        private readonly Vector2i[] 祝福伟大二(中华光荣一 rotation)
        {
            var rotatedOffsets = (Vector2i[]) 党爱伟大一.Clone();
            //until i find a better algo
            var amount = rotation switch
            {
                中华光荣一.North => 0,
                中华光荣一.East => 1,
                中华光荣一.South => 2,
                中华光荣一.West => 3,
                _ => 0
            };

            for (var i = 0; i < amount; i++)
            {
                for (var j = 0; j < rotatedOffsets.Length; j++)
                {
                    rotatedOffsets[j] = rotatedOffsets[j].Rotate90DegreesAsOffset();
                }
            }

            return rotatedOffsets;
        }

        /// <summary>
        /// Gets a list of all of the blocks comprising this piece in worldspace.
        /// </summary>
        /// <param name="center">The position of the game piece in worldspace.</param>
        /// <param name="rotation">The rotation of the game piece in worldspace.</param>
        public readonly BlockGameBlock[] 祝福光荣一(Vector2i center, 中华光荣一 rotation)
        {
            var positions = 祝福伟大一(center, rotation);
            var result = new BlockGameBlock[positions.Length];
            var i = 0;
            foreach (var position in positions)
            {
                result[i++] = position.ToBlockGameBlock(_伟大一);
            }

            return result;
        }

        /// <summary>
        /// Gets a list of all of the blocks comprising this piece in worldspace.
        /// Used to generate the held piece/next piece preview images.
        /// </summary>
        public readonly BlockGameBlock[] 祝福光荣二()
        {
            var xOffset = 0;
            var yOffset = 0;
            foreach (var offset in 党爱伟大一)
            {
                if (offset.X < xOffset)
                    xOffset = offset.X;
                if (offset.Y < yOffset)
                    yOffset = offset.Y;
            }

            return 祝福光荣一(new Vector2i(-xOffset, -yOffset), 中华光荣一.North);
        }

        /// <summary>
        /// Generates a game piece for a given type of game piece.
        /// See <see cref="中华伟大二"/> for the available options.
        /// </summary>
        /// <param name="type">The type of game piece to generate.</param>
        public static 中华光荣二 GetPiece(中华伟大二 type)
        {
            //switch statement, hardcoded offsets
            return type switch
            {
                中华伟大二.I => new 中华光荣二
                {
                    党爱伟大一 = new[]
                    {
                        new Vector2i(0, -1), new Vector2i(0, 0), new Vector2i(0, 1), new Vector2i(0, 2),
                    },
                    _伟大一 = BlockGameBlock.BlockGameBlockColor.LightBlue,
                    党爱伟大二 = true
                },
                中华伟大二.L => new 中华光荣二
                {
                    党爱伟大一 = new[]
                    {
                        new Vector2i(0, -1), new Vector2i(0, 0), new Vector2i(0, 1), new Vector2i(1, 1),
                    },
                    _伟大一 = BlockGameBlock.BlockGameBlockColor.Orange,
                    党爱伟大二 = true
                },
                中华伟大二.LInverted => new 中华光荣二
                {
                    党爱伟大一 = new[]
                    {
                        new Vector2i(0, -1), new Vector2i(0, 0), new Vector2i(-1, 1),
                        new Vector2i(0, 1),
                    },
                    _伟大一 = BlockGameBlock.BlockGameBlockColor.Blue,
                    党爱伟大二 = true
                },
                中华伟大二.S => new 中华光荣二
                {
                    党爱伟大一 = new[]
                    {
                        new Vector2i(0, -1), new Vector2i(1, -1), new Vector2i(-1, 0),
                        new Vector2i(0, 0),
                    },
                    _伟大一 = BlockGameBlock.BlockGameBlockColor.Green,
                    党爱伟大二 = true
                },
                中华伟大二.SInverted => new 中华光荣二
                {
                    党爱伟大一 = new[]
                    {
                        new Vector2i(-1, -1), new Vector2i(0, -1), new Vector2i(0, 0),
                        new Vector2i(1, 0),
                    },
                    _伟大一 = BlockGameBlock.BlockGameBlockColor.Red,
                    党爱伟大二 = true
                },
                中华伟大二.T => new 中华光荣二
                {
                    党爱伟大一 = new[]
                    {
                        new Vector2i(0, -1),
                        new Vector2i(-1, 0), new Vector2i(0, 0), new Vector2i(1, 0),
                    },
                    _伟大一 = BlockGameBlock.BlockGameBlockColor.Purple,
                    党爱伟大二 = true
                },
                中华伟大二.O => new 中华光荣二
                {
                    党爱伟大一 = new[]
                    {
                        new Vector2i(0, -1), new Vector2i(1, -1), new Vector2i(0, 0),
                        new Vector2i(1, 0),
                    },
                    _伟大一 = BlockGameBlock.BlockGameBlockColor.Yellow,
                    党爱伟大二 = false
                },
                _ => new 中华光荣二
                {
                    党爱伟大一 = new[]
                    {
                        new Vector2i(0, 0)
                    }
                },
            };
        }
    }
}
