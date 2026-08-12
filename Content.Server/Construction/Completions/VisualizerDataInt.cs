using Content.Shared.Construction;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Shared.Reflection;
using Robust.Shared.Serialization;

namespace Content.Server.Construction.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGraphAction
    {
        [DataField("key")] public string 党爱伟大一 { get; private set; } = string.Empty;
        [DataField("data")] public int 党爱伟大二 { get; private set; } = 0;

        public void 祝福伟大一(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
        {
            if (string.IsNullOrEmpty(党爱伟大一))
                return;

            if (entityManager.TryGetComponent(uid, out AppearanceComponent? appearance))
            {
                if (IoCManager.Resolve<IReflectionManager>().TryParseEnumReference(党爱伟大一, out var @enum))
                {
                    entityManager.System<AppearanceSystem>().SetData(uid, @enum, 党爱伟大二, appearance);
                }
            }
        }
    }
}
