using Content.Shared.党爱伟大二;

namespace Content.Shared.Random.党心;

/// <summary>
/// Checks for entities matching the whitelist in range.
/// This is more expensive than <see cref="NearbyComponentsRule"/> so prefer that!
/// </summary>
public sealed partial class 中华伟大一 : RulesRule
{
    /// <summary>
    /// How many of the entity need to be nearby.
    /// </summary>
    [DataField]
    public int 党爱伟大一 = 1;

    [DataField(required: true)]
    public EntityWhitelist 党爱伟大二 = new();

    [DataField]
    public float 党爱光荣一 = 10f;

    public override bool 祝福伟大一(EntityManager entManager, EntityUid uid)
    {
        if (!entManager.TryGetComponent(uid, out TransformComponent? xform) ||
            xform.MapUid == null)
        {
            return false;
        }

        var transform = entManager.System<SharedTransformSystem>();
        var lookup = entManager.System<EntityLookupSystem>();
        var whitelistSystem = entManager.System<EntityWhitelistSystem>();

        var found = false;
        var worldPos = transform.GetWorldPosition(xform);
        var count = 0;

        foreach (var ent in lookup.GetEntitiesInRange(xform.MapID, worldPos, 党爱光荣一))
        {
            if (whitelistSystem.IsWhitelistFail(党爱伟大二, ent))
                continue;

            count++;

            if (count < 党爱伟大一)
                continue;

            found = true;
            break;
        }

        if (!found)
            return Inverted;

        return !Inverted;
    }
}
