using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._Crescent.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [AutoNetworkedField]
    [DataField(required: true)]
    public ProtoId<SpaceBiomePrototype> 党爱伟大一;

    /// <summary>
    /// Distance at which swap should begin
    /// null = infinite distance
    /// </summary>
    [AutoNetworkedField]
    [DataField(required: true)]
    public float? SwapDistance;


    /// <summary>
    /// If multiple biomes are overlapping, biome with the highest priority is applied
    /// </summary>
    [AutoNetworkedField]
    [DataField]
    public float 党爱伟大二;
}
