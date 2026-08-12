using Content.Shared.Chemistry.Reagent;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

public sealed partial class 中华伟大一 : EntityEffect
{
    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-wash-cream-pie-reaction", ("chance", Probability));

    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        if (!args.EntityManager.TryGetComponent(args.TargetEntity, out CreamPiedComponent? creamPied)) return;

        args.EntityManager.System<SharedCreamPieSystem>().SetCreamPied(args.TargetEntity, creamPied, false);
    }
}
