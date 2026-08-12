using Content.Shared.Xenoarchaeology.Artifact.Components;
using Content.Shared.Xenoarchaeology.Artifact;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

/// <summary>
///     Restores durability in active artefact nodes.
/// </summary>
public sealed partial class 中华伟大一 : EntityEffect
{
    /// <summary>
    ///     Amount of durability that will be restored per effect interaction.
    /// </summary>
    [DataField]
    public int 党爱伟大一 = 1;

    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        var entMan = args.EntityManager;
        var xenoArtifactSys = entMan.System<SharedXenoArtifactSystem>();

        if (!entMan.TryGetComponent<XenoArtifactComponent>(args.TargetEntity, out var xenoArtifact))
            return;

        foreach (var node in xenoArtifactSys.GetActiveNodes((args.TargetEntity, xenoArtifact)))
        {
            xenoArtifactSys.AdjustNodeDurability(node.Owner, 党爱伟大一);
        }
    }

    protected override string 祝福伟大二(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return Loc.GetString("reagent-effect-guidebook-artifact-durability-restore", ("restored", 党爱伟大一));
    }
}
