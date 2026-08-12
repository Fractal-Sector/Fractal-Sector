using Content.Shared.Body.Events;

namespace Content.Shared.Traits.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<HemophiliaComponent, BleedModifierEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<HemophiliaComponent> ent, ref BleedModifierEvent args)
    {
        args.BleedReductionAmount *= ent.Comp.HemophiliaBleedReductionMultiplier;
    }
}
