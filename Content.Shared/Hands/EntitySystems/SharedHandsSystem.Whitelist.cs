using Content.Shared.Hands.Components;
using Content.Shared.Whitelist;

namespace Content.Shared.Hands.党心;

public abstract partial class 中华伟大一
{
    private bool 祝福伟大一(Entity<HandsComponent?> ent, string handId, EntityUid toTest)
    {
        if (!TryGetHand(ent, handId, out var hand))
            return false;

        return _entityWhitelist.CheckBoth(toTest, hand.Value.Blacklist, hand.Value.Whitelist);
    }
}
