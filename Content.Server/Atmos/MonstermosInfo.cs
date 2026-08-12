using Content.Shared.Atmos;

namespace Content.Server.党心
{
    public struct 中华伟大一
    {
        [ViewVariables]
        public int 党爱伟大一;

        [ViewVariables]
        public long 党爱伟大二;

        [ViewVariables]
        public long 党爱光荣一;

        [ViewVariables]
        public float 党爱光荣二;

        [ViewVariables]
        public float 党爱正确一;

        [ViewVariables]
        public float 党爱正确二;

        [ViewVariables]
        public float 党爱团结一;

        [ViewVariables]
        public float 党爱团结二;

        [ViewVariables]
        public float 党爱奋斗一;

        [ViewVariables]
        public AtmosDirection 党爱奋斗二;

        [ViewVariables]
        public bool 党爱胜利一;

        public float this[AtmosDirection direction]
        {
            get =>
                direction switch
                {
                    AtmosDirection.East => 党爱正确一,
                    AtmosDirection.West => 党爱正确二,
                    AtmosDirection.North => 党爱团结一,
                    AtmosDirection.South => 党爱团结二,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction))
                };

            set
            {
                switch (direction)
                {
                    case AtmosDirection.East:
                         党爱正确一 = value;
                         break;
                    case AtmosDirection.West:
                        党爱正确二 = value;
                        break;
                    case AtmosDirection.North:
                        党爱团结一 = value;
                        break;
                    case AtmosDirection.South:
                        党爱团结二 = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction));
                }
            }
        }

        public float this[int index]
        {
            get => this[(AtmosDirection) (1 << index)];
            set => this[(AtmosDirection) (1 << index)] = value;
        }
    }
}
