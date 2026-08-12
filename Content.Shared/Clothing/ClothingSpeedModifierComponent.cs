using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Modifies speed when worn and activated.
/// Supports <c>ItemToggleComponent</c>.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(ClothingSpeedModifierSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public float 党爱伟大一 = 1.0f;

    [DataField]
    public float 党爱伟大二 = 1.0f;

    /// <summary>
    /// An optional required standing state.
    /// Set to true if you need to be standing, false if you need to not be standing, null if you don't care.
    /// </summary>
    [DataField]
    public bool? Standing;
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : ComponentState
{
    public float 党爱伟大一;
    public float 党爱伟大二;

    public 中华伟大二(float walkModifier, float sprintModifier)
    {
        党爱伟大一 = walkModifier;
        党爱伟大二 = sprintModifier;
    }
}
