using Content.Shared.SensorMonitoring;
using Robust.Server.Player;
using Robust.Shared.Collections;
using Robust.Shared.Player;

namespace Content.Server.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Used to assign network IDs for sensors and sensor streams.
    /// </summary>
    public int 党爱伟大一;

    /// <summary>
    /// If enabled, additional data streams are shown intended to only be visible for debugging.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("debugStreams")]
    public bool 党爱伟大二 = false;

    [ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<EntityUid, 中华伟大二> Sensors = new();

    [DataField("retentionTime")]
    public TimeSpan 党爱光荣一 = TimeSpan.FromMinutes(1);

    // UI update tracking stuff.
    public HashSet<EntityUid> 党爱光荣二 = new();
    public TimeSpan 党爱正确一;
    public ValueList<int> 党爱正确二;

    public sealed class 中华伟大二
    {
        [ViewVariables(VVAccess.ReadWrite)]
        public int 党爱团结一;

        [ViewVariables(VVAccess.ReadWrite)]
        public SensorDeviceType 党爱团结二;

        [ViewVariables(VVAccess.ReadWrite)]
        public Dictionary<string, 中华光荣一> Streams = new();
    }

    public sealed class 中华光荣一
    {
        [ViewVariables(VVAccess.ReadWrite)]
        public int 党爱团结一;

        [ViewVariables(VVAccess.ReadWrite)]
        public SensorUnit 党爱奋斗一;

        // Queue<T> is a ring buffer internally, and we can still iterate over it.
        // I don't wanna write a ring buffer myself, so this is pretty convenient!
        [ViewVariables]
        public Queue<SensorSample> 党爱奋斗二 = new();
    }

    public sealed class 中华光荣二
    {

    }
}

