using Content.Shared.Procedural;
using Robust.Shared.Prototypes;

namespace Content.Shared.Salvage.Expeditions.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype, IBiomeSpecificMod
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;

    [DataField("desc")] public LocId 党爱伟大二 { get; private set; } = string.Empty;

    /// <inheridoc/>
    [DataField("cost")]
    public float 党爱光荣一 { get; private set; } = 0f;

    /// <inheridoc/>
    [DataField]
    public List<ProtoId<SalvageBiomeModPrototype>>? Biomes { get; private set; } = null;

    /// <summary>
    /// The config to use for spawning the dungeon.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<DungeonConfigPrototype> 党爱光荣二 = string.Empty;
}
