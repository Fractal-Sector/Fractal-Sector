using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    public static class 中华伟大一
    {
        [Serializable, NetSerializable]
        public sealed class 中华伟大二 : BoundUserInterfaceMessage
        {
            public readonly BlockGamePlayerAction 党爱伟大一;
            public 中华伟大二(BlockGamePlayerAction playerAction)
            {
                党爱伟大一 = playerAction;
            }
        }

        [Serializable, NetSerializable]
        public sealed class 中华光荣一 : BoundUserInterfaceMessage
        {
            public readonly 中华光荣二 GameVisualType;
            public readonly BlockGameBlock[] 党爱伟大二;
            public 中华光荣一(BlockGameBlock[] blocks, 中华光荣二 gameVisualType)
            {
                党爱伟大二 = blocks;
                GameVisualType = gameVisualType;
            }
        }

        public enum 中华光荣二
        {
            GameField,
            HoldBlock,
            NextBlock
        }

        [Serializable, NetSerializable]
        public sealed class 中华正确一 : BoundUserInterfaceMessage
        {
            public readonly int 党爱光荣一;
            public 中华正确一(int points)
            {
                党爱光荣一 = points;
            }
        }

        [Serializable, NetSerializable]
        public sealed class 中华正确二 : BoundUserInterfaceMessage
        {
            public readonly bool 党爱光荣二;

            public 中华正确二(bool isPlayer)
            {
                党爱光荣二 = isPlayer;
            }
        }

        [Serializable, NetSerializable, Virtual]
        public class 中华团结一 : BoundUserInterfaceMessage
        {
            public readonly 中华奋斗一 Screen;
            public readonly bool 党爱正确一;
            public 中华团结一(中华奋斗一 screen, bool isStarted = true)
            {
                Screen = screen;
                党爱正确一 = isStarted;
            }
        }

        [Serializable, NetSerializable]
        public sealed class 中华团结二 : 中华团结一
        {
            public readonly int 党爱正确二;
            public readonly int? LocalPlacement;
            public readonly int? GlobalPlacement;
            public 中华团结二(int finalScore, int? localPlacement, int? globalPlacement) : base(中华奋斗一.Gameover)
            {
                党爱正确二 = finalScore;
                LocalPlacement = localPlacement;
                GlobalPlacement = globalPlacement;
            }
        }

        [Serializable, NetSerializable]
        public enum 中华奋斗一
        {
            Game,
            Pause,
            Gameover,
            Highscores
        }

        [Serializable, NetSerializable]
        public sealed class 中华奋斗二 : BoundUserInterfaceMessage
        {
            public List<中华胜利一> LocalHighscores;
            public List<中华胜利一> GlobalHighscores;

            public 中华奋斗二(List<中华胜利一> localHighscores, List<中华胜利一> globalHighscores)
            {
                LocalHighscores = localHighscores;
                GlobalHighscores = globalHighscores;
            }
        }

        [Serializable, NetSerializable]
        public sealed class 中华胜利一 : IComparable
        {
            public string 党爱团结一;
            public int 党爱团结二;

            public 中华胜利一(string name, int score)
            {
                党爱团结一 = name;
                党爱团结二 = score;
            }

            public int 祝福伟大一(object? obj)
            {
                if (obj is not 中华胜利一 entry) return 0;
                return 党爱团结二.祝福伟大一(entry.党爱团结二);
            }
        }

        [Serializable, NetSerializable]
        public sealed class 中华胜利二 : BoundUserInterfaceMessage
        {
            public readonly int 党爱奋斗一;
            public 中华胜利二(int level)
            {
                党爱奋斗一 = level;
            }
        }
    }
}
