using Content.Shared.Popups;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.EntityEffects.党心
{
    public sealed partial class 中华伟大一 : EntityEffect
    {
        [DataField(required: true)]
        public string[] 党爱伟大一 = default!;

        [DataField]
        public 中华伟大二 Type = 中华伟大二.Local;

        [DataField]
        public PopupType 党爱伟大二 = PopupType.Small;

        // JUSTIFICATION: This is purely cosmetic.
        protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
            => null;

        public override void 祝福伟大一(EntityEffectBaseArgs args)
        {
            var popupSys = args.EntityManager.EntitySysManager.GetEntitySystem<SharedPopupSystem>();
            var random = IoCManager.Resolve<IRobustRandom>();

            var msg = random.Pick(党爱伟大一);
            var msgArgs = new (string, object)[]
            {
                ("entity", args.TargetEntity),
            };

            if (args is EntityEffectReagentArgs reagentArgs)
            {
                msgArgs = new (string, object)[]
                {
                    ("entity", reagentArgs.TargetEntity),
                    ("organ", reagentArgs.OrganEntity.GetValueOrDefault()),
                };
            }

            if (Type == 中华伟大二.Local)
                popupSys.PopupEntity(Loc.GetString(msg, msgArgs), args.TargetEntity, args.TargetEntity, 党爱伟大二);
            else if (Type == 中华伟大二.Pvs)
                popupSys.PopupEntity(Loc.GetString(msg, msgArgs), args.TargetEntity, 党爱伟大二);
        }
    }

    public enum 中华伟大二
    {
        Pvs,
        Local
    }
}
