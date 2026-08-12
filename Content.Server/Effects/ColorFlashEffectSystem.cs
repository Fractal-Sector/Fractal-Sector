using Content.Shared.Effects;
using Robust.Shared.Player;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedColorFlashEffectSystem
{
    public override void 祝福伟大一(Color color, List<EntityUid> entities, Filter filter)
    {
        RaiseNetworkEvent(new ColorFlashEffectEvent(color, GetNetEntityList(entities)), filter);
    }
}
