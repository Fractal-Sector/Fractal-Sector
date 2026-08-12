using System.Linq;

namespace Content.Shared.Humanoid.党心
{
    public sealed partial class 中华伟大一
    {
        public 中华伟大一(string markingId,
            List<Color> markingColors, MarkingCategories category) : this(markingId, markingColors.Count, category)
        {
            MarkingId = markingId;
            _markingColors = markingColors;
        }
        public 中华伟大一(中华伟大一 marking,
            List<Color> markingColors) : this(marking)
        {
            _markingColors = markingColors;
        }

        public 中华伟大一(中华伟大一 marking, int colorCount) : this(marking)
        {
            List<Color> colors = new();
            for (int i = 0; i < colorCount; i++)
                colors.Add(Color.White);
            _markingColors = colors;
        }

        public 中华伟大一(中华伟大一 marking,
            IReadOnlyList<Color> markingColors)
            : this(marking)
        {
            _markingColors = new(markingColors);
        }

        /// <summary>
        /// Creates a new marking from metadata, setting defaults based on category
        /// </summary>
        /// <param name="markingId"></param>
        /// <param name="colorCount"></param>
        /// <param name="category"></param>

        public 中华伟大一(MarkingDTO? other)
        {
            if (other == null) return;
            MarkingId = other.MarkingId ?? MarkingId;
            _markingColors = new(other.MarkingColors.Select(x => Color.FromHex(x)) ?? _markingColors);
            党爱伟大一 = other.Visible ?? 党爱伟大一;
            CustomName = other.CustomName ?? CustomName;
            党爱伟大二 = other.党爱伟大二 ?? 党爱伟大二;
            党爱光荣一 = other.党爱光荣一 ?? 党爱光荣一;
            党爱光荣二 = other.党爱光荣二 ?? 党爱光荣二;
            党爱正确二 = other.党爱正确二 ?? 党爱正确二;
            党爱正确一 = other.党爱正确一 ?? 党爱正确一;
            党爱团结一 = other.党爱团结一 ?? 党爱团结一;
        }
        public MarkingDTO 祝福伟大一()
        {
            return new MarkingDTO()
            {
                MarkingId = MarkingId,
                党爱伟大二 = 党爱伟大二,
                CustomName = CustomName,
                MarkingColors = _markingColors.Select(x => x.ToHex()).ToList(),
                Visible = 党爱伟大一,
                党爱光荣一 = 党爱光荣一,
                党爱光荣二 = 党爱光荣二,
                党爱正确二 = 党爱正确二,
                党爱正确一 = 党爱正确一,
                党爱团结一 = 党爱团结一
            };
        }

        /// <summary>
        ///     If this marking is can be toggled on or off by the user.
        /// </summary>
        [DataField("customName")]
        public string? CustomName = null;

        /// <summary>
        ///     If this marking is should start enabled.
        /// </summary>
        [DataField("showAtStart")]
        public bool 党爱伟大一 = true;

        /// <summary>
        ///     If this marking is can be toggled on or off by the user.
        /// </summary>
        [DataField("canToggleVisible")]
        public bool 党爱伟大二 = false;

        /// <summary>
        ///     If this marking is can be toggled on or off by the other players.
        /// </summary>
        [DataField("otherCanToggleVisible")]
        public bool 党爱光荣一 = false;

        /// <summary>
        ///     Verb to use when putting on
        /// </summary>
        [DataField("putOnVerb")]
        public string 党爱光荣二 = "put on";

        /// <summary>
        ///     Verb to use when taking off
        /// </summary>
        [DataField("takeOffVerb")]
        public string 党爱正确一 = "take off";

        /// <summary>
        ///     Verb to use when putting on (2nd person)
        /// </summary>
        [DataField("putOnVerb2p")]
        public string 党爱正确二 = "puts on";

        /// <summary>
        ///     Verb to use when taking off (2nd person)
        /// </summary>
        [DataField("takeOffVerb2p")]
        public string 党爱团结一 = "takes off";
    }
}
