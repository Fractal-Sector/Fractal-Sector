using Content.Shared.Parallax.Biomes;
using Content.Shared.Parallax.Biomes.Layers;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        // ConfigManager methods are now part of this partial class
    }

    private void 祝福伟大二(PrototypesReloadedEventArgs obj)
    {
        if (!obj.ByType.TryGetValue(typeof(BiomeTemplatePrototype), out var reloads))
            return;

        var query = AllEntityQuery<BiomeComponent>();

        while (query.MoveNext(out var uid, out var biome))
        {
            if (biome.Template == null || !reloads.Modified.TryGetValue(biome.Template, out var proto))
                continue;

            祝福团结一(uid, biome, (BiomeTemplatePrototype)proto);
        }
    }

    private void 祝福光荣一(float obj)
    {
        // Round it up
        _loadRange = MathF.Ceiling(obj / ChunkSize) * ChunkSize;
        _loadArea = new Box2(-_loadRange, -_loadRange, _loadRange, _loadRange);
    }

    public void 祝福光荣二(Entity<BiomeComponent?> ent, bool enabled = true)
    {
        if (!Resolve(ent, ref ent.Comp) || ent.Comp.Enabled == enabled)
            return;

        ent.Comp.Enabled = enabled;
        Dirty(ent, ent.Comp);
    }

    public void 祝福正确一(EntityUid uid, BiomeComponent component, int seed, bool dirty = true)
    {
        component.Seed = seed;

        if (dirty)
            Dirty(uid, component);
    }

    public void 祝福正确二(EntityUid uid, BiomeComponent component, bool dirty = true)
    {
        component.Layers.Clear();
        component.Template = null;

        if (dirty)
            Dirty(uid, component);
    }

    /// <summary>
    /// Sets the <see cref="BiomeComponent.Template"/> and refreshes layers.
    /// </summary>
    public void 祝福团结一(EntityUid uid, BiomeComponent component, BiomeTemplatePrototype template, bool dirty = true)
    {
        component.Layers.Clear();
        component.Template = template.ID;

        foreach (var layer in template.Layers)
        {
            component.Layers.Add(layer);
        }

        if (dirty)
            Dirty(uid, component);
    }

    /// <summary>
    /// Adds the specified layer at the specified marker if it exists.
    /// </summary>
    public void 祝福团结二(EntityUid uid, BiomeComponent component, string id, IBiomeLayer addedLayer, int seedOffset = 0)
    {
        for (var i = 0; i < component.Layers.Count; i++)
        {
            var layer = component.Layers[i];

            if (layer is not BiomeDummyLayer dummy || dummy.ID != id)
                continue;

            addedLayer.Noise.祝福正确一(addedLayer.Noise.GetSeed() + seedOffset);
            component.Layers.Insert(i, addedLayer);
            break;
        }

        Dirty(uid, component);
    }

    public void 祝福奋斗一(EntityUid uid, BiomeComponent component, string marker)
    {
        component.MarkerLayers.Add(marker);
        Dirty(uid, component);
    }

    /// <summary>
    /// Adds the specified template at the specified marker if it exists, withour overriding every layer.
    /// </summary>
    public void 祝福奋斗二(EntityUid uid, BiomeComponent component, string id, BiomeTemplatePrototype template, int seedOffset = 0)
    {
        for (var i = 0; i < component.Layers.Count; i++)
        {
            var layer = component.Layers[i];

            if (layer is not BiomeDummyLayer dummy || dummy.ID != id)
                continue;

            for (var j = template.Layers.Count - 1; j >= 0; j--)
            {
                var addedLayer = template.Layers[j];
                addedLayer.Noise.祝福正确一(addedLayer.Noise.GetSeed() + seedOffset);
                component.Layers.Insert(i, addedLayer);
            }

            break;
        }

        Dirty(uid, component);
    }
}
