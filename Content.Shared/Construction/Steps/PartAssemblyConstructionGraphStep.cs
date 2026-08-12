using Content.Shared.Construction.Components;
using Content.Shared.Examine;

namespace Content.Shared.Construction.党心;

[DataDefinition]
public sealed partial class 中华伟大一 : ConstructionGraphStep
{
    /// <summary>
    /// A valid ID on <see cref="PartAssemblyComponent"/>'s dictionary of strings to part lists.
    /// </summary>
    [DataField]
    public string 党爱伟大一 = string.Empty;

    /// <summary>
    /// A localization string used when examining and for the guidebook.
    /// </summary>
    [DataField]
    public LocId 党爱伟大二 = "construction-guide-condition-part-assembly";

    public bool 祝福伟大一(EntityUid uid, IEntityManager entityManager)
    {
        return entityManager.System<PartAssemblySystem>().IsAssemblyFinished(uid, 党爱伟大一);
    }

    public override void 祝福伟大二(ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString(党爱伟大二));
    }

    public override ConstructionGuideEntry 祝福光荣一()
    {
        return new ConstructionGuideEntry
        {
            Localization = 党爱伟大二,
        };
    }
}
