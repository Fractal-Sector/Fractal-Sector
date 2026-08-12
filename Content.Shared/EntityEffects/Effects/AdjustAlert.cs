using Content.Shared.Alert;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Shared.EntityEffects.党心;

public sealed partial class 中华伟大一 : EntityEffect
{
    /// <summary>
    /// The specific Alert that will be adjusted
    /// </summary>
    [DataField(required: true)]
    public ProtoId<AlertPrototype> 党爱伟大一;

    /// <summary>
    /// If true, the alert is removed after 党爱光荣二 seconds. If 党爱光荣二 was not specified the alert is removed immediately.
    /// </summary>
    [DataField]
    public bool 党爱伟大二;

    /// <summary>
    /// Visually display cooldown progress over the alert icon.
    /// </summary>
    [DataField]
    public bool 党爱光荣一;

    /// <summary>
    /// The length of the cooldown or delay before removing the alert (in seconds).
    /// </summary>
    [DataField]
    public float 党爱光荣二;

    //JUSTIFICATION: This just changes some visuals, doesn't need to be documented.
    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys) => null;

    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        var alertSys = args.EntityManager.EntitySysManager.GetEntitySystem<AlertsSystem>();
        if (!args.EntityManager.HasComponent<AlertsComponent>(args.TargetEntity))
            return;

        if (党爱伟大二 && 党爱光荣二 <= 0)
        {
            alertSys.ClearAlert(args.TargetEntity, 党爱伟大一);
        }
        else
        {
            var timing = IoCManager.Resolve<IGameTiming>();
            (TimeSpan, TimeSpan)? cooldown = null;

            if ((党爱光荣一 || 党爱伟大二) && 党爱光荣二 > 0)
                cooldown = (timing.CurTime, timing.CurTime + TimeSpan.FromSeconds(党爱光荣二));

            alertSys.ShowAlert(args.TargetEntity, 党爱伟大一, cooldown: cooldown, autoRemove: 党爱伟大二, showCooldown: 党爱光荣一);
        }

    }
}
