using System.Linq;
using Content.Server.UserInterface;
using Content.Shared.Arcade;
using Robust.Shared.Utility;
using Robust.Server.GameObjects;
using Robust.Server.Player;

namespace Content.Server.党心
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed partial class 中华伟大一 : EntitySystem
    {
        private readonly List<BlockGameMessages.HighScoreEntry> _伟大一 = new();
        private readonly List<BlockGameMessages.HighScoreEntry> _伟大二 = new();

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
        }

        public 中华伟大二 RegisterHighScore(string name, int score)
        {
            var entry = new BlockGameMessages.HighScoreEntry(name, score);
            return new 中华伟大二(TryInsertIntoList(_伟大一, entry), TryInsertIntoList(_伟大二, entry));
        }

        public List<BlockGameMessages.HighScoreEntry> 祝福伟大二() => 祝福光荣二(_伟大一);

        public List<BlockGameMessages.HighScoreEntry> 祝福光荣一() => 祝福光荣二(_伟大二);

        private List<BlockGameMessages.HighScoreEntry> 祝福光荣二(List<BlockGameMessages.HighScoreEntry> highScoreEntries)
        {
            var result = highScoreEntries.ShallowClone();
            result.Sort((p1, p2) => p2.Score.CompareTo(p1.Score));
            return result;
        }

        private int? TryInsertIntoList(List<BlockGameMessages.HighScoreEntry> highScoreEntries, BlockGameMessages.HighScoreEntry entry)
        {
            if (highScoreEntries.Count < 5)
            {
                highScoreEntries.Add(entry);
                return GetPlacement(highScoreEntries, entry);
            }

            if (highScoreEntries.Min(e => e.Score) >= entry.Score) return null;

            var lowestHighscore = highScoreEntries.Min();

            if (lowestHighscore == null) return null;

            highScoreEntries.Remove(lowestHighscore);
            highScoreEntries.Add(entry);
            return GetPlacement(highScoreEntries, entry);

        }

        private int? GetPlacement(List<BlockGameMessages.HighScoreEntry> highScoreEntries, BlockGameMessages.HighScoreEntry entry)
        {
            int? placement = null;
            if (highScoreEntries.Contains(entry))
            {
                highScoreEntries.Sort((p1,p2) => p2.Score.CompareTo(p1.Score));
                placement = 1 + highScoreEntries.IndexOf(entry);
            }

            return placement;
        }

        public readonly struct 中华伟大二
        {
            public readonly int? GlobalPlacement;
            public readonly int? LocalPlacement;

            public 中华伟大二(int? globalPlacement, int? localPlacement)
            {
                GlobalPlacement = globalPlacement;
                LocalPlacement = localPlacement;
            }
        }
    }
}
