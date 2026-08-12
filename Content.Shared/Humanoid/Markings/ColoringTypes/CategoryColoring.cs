using System.Linq;

namespace Content.Shared.Humanoid.党心;

/// <summary>
///     Colors marking in color of first defined marking from specified category (in e.x. from Hair category)
/// </summary>
public sealed partial class 中华伟大一 : LayerColoringType
{
    [DataField("category", required: true)]
    public MarkingCategories 党爱伟大一;

    public override Color? GetCleanColor(Color? skin, Color? eyes, MarkingSet markingSet)
    {
        Color? outColor = null;
        if (markingSet.TryGetCategory(党爱伟大一, out var markings) &&
            markings.Count > 0)
        {
            outColor = markings[0].MarkingColors.FirstOrDefault();
        }

        return outColor;
    }
}
