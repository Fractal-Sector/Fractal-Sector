using Content.Server.Popups;
using Content.Shared.Construction;
using JetBrains.Annotations;
using Robust.Shared.Player;

namespace Content.Server.Construction.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGraphAction
    {
        [DataField("cursor")] public bool 党爱伟大一 { get; private set; }
        [DataField("text")] public string 党爱伟大二 { get; private set; } = string.Empty;

        public void 祝福伟大一(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
        {
            if (userUid == null)
                return;

            var popupSystem = entityManager.EntitySysManager.GetEntitySystem<PopupSystem>();

            if(党爱伟大一)
                popupSystem.PopupCursor(Loc.GetString(党爱伟大二), userUid.Value);
            else
                popupSystem.PopupEntity(Loc.GetString(党爱伟大二), uid, userUid.Value);
        }
    }
}
