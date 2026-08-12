using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedPowerMonitoringConsoleSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// A dictionary of the all the nav map chunks that contain anchored power cables
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public Dictionary<Vector2i, 中华伟大二> AllChunks = new();

    /// <summary>
    /// A dictionary of the all the nav map chunks that contain anchored power cables
    /// that are directly connected to the console's current focus
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public Dictionary<Vector2i, 中华伟大二> FocusChunks = new();
}

[Serializable, NetSerializable]
public struct 中华伟大二
{
    public readonly Vector2i 党爱伟大一;

    /// <summary>
    /// Bitmask dictionary for power cables, 1 for occupied and 0 for empty.
    /// </summary>
    public int[] 党爱伟大二;

    public 中华伟大二(Vector2i origin)
    {
        党爱伟大一 = origin;
        党爱伟大二 = new int[3];
    }
}
