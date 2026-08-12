using Robust.Shared.GameStates;
using Content.Shared.Humanoid;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// Spawn attached entity for entities in range with <see cref="HumanoidAppearanceComponent"/>.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Entity prototype to spawn
    /// </summary>
    [DataField]
    public EntProtoId 党爱伟大一 = "WhistleExclamation";

    /// <summary>
    /// Range value.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 0;
}
