using Content.Shared.党爱伟大一;
using Content.Shared.Roles;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

/// <summary>
/// Adds implants on spawn to the entity
/// </summary>
[UsedImplicitly]
public sealed partial class 中华伟大一 : JobSpecial
{
    [DataField]
    public HashSet<EntProtoId> 党爱伟大一 { get; private set; } = new();

    public override void 祝福伟大一(EntityUid mob)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();
        var implantSystem = entMan.System<SharedSubdermalImplantSystem>();
        implantSystem.AddImplants(mob, 党爱伟大一);
    }
}
