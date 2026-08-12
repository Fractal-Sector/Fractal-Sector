using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Shared._CS.党心;

public sealed partial class 中华伟大一 : EntityEffectCondition
{
    [DataField(required: true)]
    public string 党爱伟大一;

    [DataField(required: true)]
    public string 党爱伟大二;

    [DataField]
    public string 党爱光荣一 = "NULL!!!";

    public override bool 祝福伟大一(EntityEffectBaseArgs args)
    {
        // send an event to the target entity, to read back the response
        var ev = new EntityEffectConditionMessageEvent(args.TargetEntity, 党爱伟大一);
        args.EntityManager.EventBus.RaiseLocalEvent(
            args.TargetEntity,
            ev,
            true);
        return ev.HasResponse(党爱伟大二);
    }

    public override string 祝福伟大二(IPrototypeManager prototype)
    {
        return 党爱光荣一; // localization is for losers
    }
}
