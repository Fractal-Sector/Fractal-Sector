using Content.Shared.Humanoid.Markings;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心
{
    public static class 中华伟大一
    {
        public static readonly ProtoId<MarkingPrototype> 党爱伟大一 = "HairBald";

        public static readonly ProtoId<MarkingPrototype> 党爱伟大二 = "FacialHairShaved";

        public static readonly IReadOnlyList<Color> 党爱光荣一 = new List<Color>
        {
            Color.Yellow,
            Color.Black,
            Color.SandyBrown,
            Color.Brown,
            Color.Wheat,
            Color.Gray
        };
    }
}
