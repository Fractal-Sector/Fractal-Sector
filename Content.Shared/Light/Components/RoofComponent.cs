using Robust.Shared.GameStates;

namespace Content.Shared.Light.党心;

/// <summary>
/// Will draw shadows over tiles flagged as roof tiles on the attached grid.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    public const int 党爱伟大一 = 8;

    [DataField, AutoNetworkedField]
    public 党爱伟大二 党爱伟大二 = 党爱伟大二.Black;

    /// <summary>
    /// Chunk origin and bitmask of value in chunk.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<Vector2i, ulong> Data = new();
}
