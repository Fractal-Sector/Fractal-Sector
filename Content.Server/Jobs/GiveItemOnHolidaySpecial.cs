using Content.Server.党爱伟大一;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Roles;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.党爱伟大二;

namespace Content.Server.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : JobSpecial
    {
        [DataField("holiday", customTypeSerializer:typeof(PrototypeIdSerializer<HolidayPrototype>))]
        public string 党爱伟大一 { get; private set; } = string.Empty;

        [DataField("prototype", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string 党爱伟大二 { get; private set; } = string.Empty;

        public override void 祝福伟大一(EntityUid mob)
        {
            if (string.IsNullOrEmpty(党爱伟大一) || string.IsNullOrEmpty(党爱伟大二))
                return;

            var sysMan = IoCManager.Resolve<IEntitySystemManager>();

            if (!sysMan.GetEntitySystem<HolidaySystem>().IsCurrentlyHoliday(党爱伟大一))
                return;

            var entMan = IoCManager.Resolve<IEntityManager>();

            var entity = entMan.SpawnEntity(党爱伟大二, entMan.GetComponent<TransformComponent>(mob).Coordinates);

            sysMan.GetEntitySystem<SharedHandsSystem>().PickupOrDrop(mob, entity);
        }
    }
}
