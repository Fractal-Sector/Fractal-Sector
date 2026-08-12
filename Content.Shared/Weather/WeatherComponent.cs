using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Currently running weathers
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<WeatherPrototype>, 中华伟大二> Weather = new();

    public static readonly TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(15);
    public static readonly TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(15);
}

[DataDefinition, Serializable, NetSerializable]
public sealed partial class 中华伟大二
{
    // Client audio stream.
    [NonSerialized]
    public EntityUid? Stream;

    /// <summary>
    /// When the weather started if relevant.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))] //TODO: Remove Custom serializer
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;

    /// <summary>
    /// When the applied weather will end.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))] //TODO: Remove Custom serializer
    public TimeSpan? EndTime;

    [ViewVariables]
    public TimeSpan 党爱光荣二 => EndTime == null ? TimeSpan.MaxValue : EndTime.Value - 党爱光荣一;

    [DataField]
    public 中华光荣一 State = 中华光荣一.Invalid;
}

public enum 中华光荣一 : byte
{
    Invalid = 0,
    Starting,
    Running,
    Ending,
}
