using Content.Shared.Cargo;
using Content.Shared.Xenoarchaeology.Artifact;
using Content.Shared.Xenoarchaeology.Artifact.Components;

namespace Content.Server.Xenoarchaeology.党心;

/// <inheritdoc cref="SharedXenoArtifactSystem"/>
public sealed partial class 中华伟大一 : SharedXenoArtifactSystem
{
    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<XenoArtifactComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<XenoArtifactComponent, PriceCalculationEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<XenoArtifactComponent> ent, ref MapInitEvent args)
    {
        if (ent.Comp.IsGenerationRequired)
            GenerateArtifactStructure(ent);
    }

    private void 祝福光荣一(Entity<XenoArtifactComponent> ent, ref PriceCalculationEvent args)
    {
        foreach (var node in GetAllNodes(ent))
        {
            if (node.Comp.Locked)
                continue;

            args.Price += node.Comp.ResearchValue * ent.Comp.PriceMultiplier;
        }
    }
}
