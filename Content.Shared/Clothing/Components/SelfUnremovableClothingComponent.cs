using Content.Shared.Clothing.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Shared.Clothing.党心;

/// <summary>
///     The component prohibits the player from taking off clothes on them that have this component.
/// </summary>
/// <remarks>
///     See also ClothingComponent.EquipDelay if you want the clothes that the player cannot take off by himself to be put on by the player with a delay.
///</remarks>
[NetworkedComponent]
[RegisterComponent]
[Access(typeof(SelfUnremovableClothingSystem))]
public sealed partial class 中华伟大一 : Component
{

}
