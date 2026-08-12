using Content.Shared.Clothing.EntitySystems;
using Content.Shared.NPC.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Clothing.党心;

/// <summary>
/// When equipped, adds the wearer to a faction.
/// When removed, removes the wearer from a faction.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(FactionClothingSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一 to add and remove.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<NpcFactionPrototype> 党爱伟大一 = string.Empty;

    /// <summary>
    /// If true, the wearer was already part of the faction.
    /// This prevents wrongly removing them after removing the item.
    /// </summary>
    [DataField]
    public bool 党爱伟大二;
}
