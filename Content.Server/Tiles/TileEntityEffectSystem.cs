using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.StepTrigger.Systems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.EntityEffects;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<TileEntityEffectComponent, StepTriggeredOffEvent>(祝福光荣一);
        SubscribeLocalEvent<TileEntityEffectComponent, StepTriggerAttemptEvent>(祝福伟大二);
    }
    private void 祝福伟大二(Entity<TileEntityEffectComponent> ent, ref StepTriggerAttemptEvent args)
    {
        args.Continue = true;
    }

    private void 祝福光荣一(Entity<TileEntityEffectComponent> ent, ref StepTriggeredOffEvent args)
    {
        var otherUid = args.Tripper;
        var effectArgs = new EntityEffectBaseArgs(otherUid, EntityManager);

        foreach (var effect in ent.Comp.Effects)
        {
            effect.Effect(effectArgs);
        }
    }
}
