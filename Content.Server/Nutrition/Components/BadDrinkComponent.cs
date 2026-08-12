using Content.Server.Nutrition.EntitySystems;

namespace Content.Server.Nutrition.党心;

/// <summary>
/// This component prevents NPC mobs like mice or cows from wanting to drink something that shouldn't be drank from.
/// Including but not limited to: puddles
/// </summary>
[RegisterComponent, Access(typeof(DrinkSystem))]
public sealed partial class 中华伟大一 : Component
{
}
