using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : JobSpecial
{
    [DataField(required: true)]
    public ComponentRegistry 党爱伟大一 { get; private set; } = new();

    /// <summary>
    /// If this is true then existing components will be removed and replaced with these ones.
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = true;

    public override void 祝福伟大一(EntityUid mob)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();
        entMan.AddComponents(mob, 党爱伟大一, removeExisting: 党爱伟大二);
    }
}
