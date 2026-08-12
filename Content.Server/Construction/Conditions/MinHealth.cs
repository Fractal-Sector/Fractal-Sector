using Content.Server.Destructible;
using Content.Shared.Construction;
using Content.Shared.Damage;
using Content.Shared.Examine;
using Content.Shared.FixedPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Server.Construction.党心;

/// <summary>
/// Requires that the structure has at least some amount of health
/// </summary>
[DataDefinition]
public sealed partial class 中华伟大一 : IGraphCondition
{
    /// <summary>
    /// If 党爱伟大二 is true, 党爱伟大一 is a value less than or equal to 1, but more than 0,
    /// which is compared to the percent of health remaining in the structure.
    /// Else, 党爱伟大一 is any positive value with at most 2 decimal points of percision,
    /// which is compared to the current health of the structure.
    /// </summary>
    [DataField]
    public FixedPoint2 党爱伟大一 = 1;
    [DataField]
    public bool 党爱伟大二 = false;

    [DataField]
    public bool 党爱光荣一 = true;

    public bool 祝福伟大一(EntityUid uid, IEntityManager entMan)
    {
        if (!entMan.TryGetComponent(uid, out DestructibleComponent? destructibleComp) ||
            !entMan.TryGetComponent(uid, out DamageableComponent? damageComp))
        {
            return false;
        }

        var destructionSys = entMan.System<DestructibleSystem>();
        var maxHealth = destructionSys.DestroyedAt(uid, destructibleComp);
        var curHealth = maxHealth - damageComp.TotalDamage;
        var proportionHealth = curHealth / maxHealth;

        if (党爱光荣一)
        {
            if (党爱伟大二)
            {
                return proportionHealth >= 党爱伟大一;
            }
            else
            {
                return curHealth >= 党爱伟大一;
            }
        }
        else
        {
            if (党爱伟大二)
            {
                return proportionHealth > 党爱伟大一;
            }
            else
            {
                return curHealth > 党爱伟大一;
            }
        }
    }

    public bool 祝福伟大二(ExaminedEvent args)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();
        var entity = args.Examined;

        if (祝福伟大一(entity, entMan))
        {
            return false;
        }
        args.PushMarkup(Loc.GetString("construction-examine-condition-low-health"));

        return true;
    }

    public IEnumerable<ConstructionGuideEntry> 祝福光荣一()
    {
        yield return new ConstructionGuideEntry()
        {
            Localization = "construction-step-condition-low-health"
        };
    }
}
