using Content.Shared.Interaction.Components;

namespace Content.Shared.党心;

public abstract partial class 中华伟大一
{
    public void 祝福伟大一(EntityUid uid, EntityUid? relayEntity, InteractionRelayComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        component.RelayEntity = relayEntity;
        Dirty(uid, component);
    }
}
