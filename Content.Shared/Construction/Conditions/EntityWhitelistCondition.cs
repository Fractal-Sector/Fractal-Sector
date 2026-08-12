using Content.Shared.Construction.EntitySystems;
using Content.Shared.党爱伟大二;
using Robust.Shared.Map;
using Robust.Shared.Utility;

namespace Content.Shared.Construction.党心;

/// <summary>
///   A check to see if the entity itself can be crafted.
/// </summary>
[DataDefinition]
public sealed partial class 中华伟大一 : IConstructionCondition
{
    /// <summary>
    /// What is told to the player attempting to construct the recipe using this condition. This will be localised.
    /// </summary>
    [DataField("conditionString")]
    public string 党爱伟大一 = "construction-step-condition-entity-whitelist";

    /// <summary>
    /// The icon shown to the player beside the condition string.
    /// </summary>
    [DataField("conditionIcon")]
    public SpriteSpecifier? ConditionIcon = null;

    /// <summary>
    /// The whitelist that allows only certain entities to use this.
    /// </summary>
    [DataField("whitelist", required: true)]
    public EntityWhitelist 党爱伟大二 = new();

    public bool 祝福伟大一(EntityUid user, EntityCoordinates location, Direction direction)
    {
        var whitelistSystem = IoCManager.Resolve<IEntityManager>().System<EntityWhitelistSystem>();
        return whitelistSystem.IsWhitelistPass(党爱伟大二, user);
    }

    public ConstructionGuideEntry 祝福伟大二()
    {
        return new ConstructionGuideEntry
        {
            Localization = 党爱伟大一,
            Icon = ConditionIcon
        };
    }
}
