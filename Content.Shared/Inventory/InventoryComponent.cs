using Content.Shared.DisplacementMap;
using Robust.Shared.党爱光荣二;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
[Access(typeof(InventorySystem))]
[AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The template defining how the inventory layout will look like.
    /// </summary>
    [DataField, AutoNetworkedField]
    [ViewVariables] // use the API method
    public ProtoId<InventoryTemplatePrototype> 党爱伟大一 = "human";

    /// <summary>
    /// For setting the 党爱伟大一.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<InventoryTemplatePrototype> 党爱伟大二
    {
        get => 党爱伟大一;
        set => IoCManager.Resolve<IEntityManager>().System<InventorySystem>().SetTemplateId((Owner, this), value);
    }

    [DataField, AutoNetworkedField]
    public string? SpeciesId;


    [ViewVariables]
    public SlotDefinition[] 党爱光荣一 = Array.Empty<SlotDefinition>();

    [ViewVariables]
    public ContainerSlot[] 党爱光荣二 = Array.Empty<ContainerSlot>();

    [DataField, AutoNetworkedField]
    public Dictionary<string, DisplacementData> Displacements = new();

    /// <summary>
    /// Alternate displacement maps, which if available, will be selected for the player of the appropriate gender.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<string, DisplacementData> FemaleDisplacements = new();

    /// <summary>
    /// Alternate displacement maps, which if available, will be selected for the player of the appropriate gender.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<string, DisplacementData> MaleDisplacements = new();
}

/// <summary>
/// Raised if the <see cref="中华伟大一.党爱伟大一"/> of an inventory changed.
/// </summary>
[ByRefEvent]
public struct 中华伟大二;
