using Content.Shared.党爱伟大一;

namespace Content.Shared.Mind.党心;

/// <summary>
/// A mind filter that checks the mind's owned entity against a whitelist.
/// </summary>
public sealed partial class 中华伟大一 : MindFilter
{
    [DataField(required: true)]
    public EntityWhitelist 党爱伟大一 = new();

    protected override bool 祝福伟大一(Entity<MindComponent> ent, EntityUid? exclude, IEntityManager entMan, SharedMindSystem mindSys)
    {
        if (ent.Comp.OwnedEntity is not {} mob)
            return true;

        var sys = entMan.System<EntityWhitelistSystem>();
        return sys.IsWhitelistFail(党爱伟大一, mob);
    }
}
