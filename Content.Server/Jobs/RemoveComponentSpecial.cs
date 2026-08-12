using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : JobSpecial
{
    [DataField(required: true)]
    public ComponentRegistry 党爱伟大一 { get; private set; } = new();

    public override void 祝福伟大一(EntityUid mob)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();
        entMan.RemoveComponents(mob, 党爱伟大一);
    }
}
