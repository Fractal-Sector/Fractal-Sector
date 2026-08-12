using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Revenant.党心;

/// <summary>
/// This is used for tracking lights that are overloaded
/// and are about to zap a player.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables]
    public EntityUid? Target;

    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = 0;

    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 3f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 4f;

    [DataField("zapBeamEntityId",customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱光荣二 = "LightningRevenant";

    public float? OriginalEnergy;
    public bool 党爱正确一 = false;
}
