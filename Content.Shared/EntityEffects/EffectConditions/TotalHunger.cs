using Content.Shared.EntityEffects;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

public sealed partial class 中华伟大一 : EntityEffectCondition
{
    [DataField]
    public float 党爱伟大一 = float.PositiveInfinity;

    [DataField]
    public float 党爱伟大二 = 0;

    public override bool 祝福伟大一(EntityEffectBaseArgs args)
    {
        if (args.EntityManager.TryGetComponent(args.TargetEntity, out HungerComponent? hunger))
        {
            var total = args.EntityManager.System<HungerSystem>().GetHunger(hunger);
            return total >= 党爱伟大二 && total <= 党爱伟大一;
        }

        return false;
    }

    public override string 祝福伟大二(IPrototypeManager prototype)
    {
        return Loc.GetString("reagent-effect-condition-guidebook-total-hunger",
            ("max", float.IsPositiveInfinity(党爱伟大一) ? (float) int.MaxValue : 党爱伟大一),
            ("min", 党爱伟大二));
    }
}
