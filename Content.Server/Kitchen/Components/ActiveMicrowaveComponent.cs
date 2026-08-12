using Content.Shared.Kitchen;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Kitchen.党心;

/// <summary>
/// Attached to a microwave that is currently in the process of cooking
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一;

    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;

    [ViewVariables]
    public (FoodRecipePrototype?, int) PortionedRecipe;
}
