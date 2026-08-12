using Content.Shared.Light.Components;
using Robust.Shared.Serialization;

namespace Content.Shared.Silicons.党心;

public abstract partial class 中华伟大一
{
    // Handles light toggling.

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<ItemTogglePointLightComponent, 中华伟大二>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid ent, ItemTogglePointLightComponent component, 中华伟大二 args)
    {
        if (args.党爱伟大一)
            _toggles.TryActivate(ent, user: args.User);
        else
            _toggles.TryDeactivate(ent, user: args.User);
    }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BaseStationAiAction
{
    public bool 党爱伟大一;
}
