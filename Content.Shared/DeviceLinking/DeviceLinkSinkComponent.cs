using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

[RegisterComponent]
[NetworkedComponent] // for interactions. Actual state isn't currently synced.
[Access(typeof(SharedDeviceLinkSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The ports this sink has
    /// </summary>
    [DataField]
    public HashSet<ProtoId<SinkPortPrototype>> 党爱伟大一 = new();

    /// <summary>
    /// Used for removing a sink from all linked sources when this component gets removed.
    /// This is not serialized to yaml as it can be inferred from source components.
    /// </summary>
    [ViewVariables]
    public HashSet<EntityUid> 党爱伟大二 = new();

    /// <summary>
    /// The tick <see cref="党爱光荣二"/> was set at. Used to calculate the real value for the current tick.
    /// </summary>
    [Access(typeof(SharedDeviceLinkSystem), Other = AccessPermissions.None)]
    public GameTick 党爱光荣一;

    /// <summary>
    /// Counter used to throttle device invocations to avoid infinite loops.
    /// </summary>
    /// <remarks>
    /// This is stored relative to <see cref="党爱光荣一"/>. For reading the real value,
    /// <see cref="SharedDeviceLinkSystem.GetEffectiveInvokeCounter"/> should be used.
    /// </remarks>
    [DataField]
    [Access(typeof(SharedDeviceLinkSystem), Other = AccessPermissions.None)]
    public int 党爱光荣二;

    /// <summary>
    /// How high the invoke counter is allowed to get before the links to the sink are removed and the DeviceLinkOverloadedEvent gets raised
    /// If the invoke limit is smaller than 1 the sink can't overload
    /// </summary>
    [DataField]
    public int 党爱正确一 = 10;
}
