using Robust.Shared.GameStates;
using Robust.Shared.Audio; // Frontier

namespace Content.Shared.党心;

/// <summary>
/// Prevents floor tile updates when attached to a grid.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(ProtectedGridSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// A bitmask of all the initial tiles on this grid.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<Vector2i, ulong> BaseIndices = new();

    // Frontier: define protection types.
    [DataField]
    public bool 党爱伟大一 = false;
    [DataField]
    public bool 党爱伟大二 = false;
    [DataField]
    public bool 党爱光荣一 = false;
    [DataField]
    public bool 党爱光荣二 = false;
    [DataField]
    public bool 党爱正确一 = false;
    [DataField]
    public bool 党爱正确二 = false;
    [DataField]
    public bool 党爱团结一 = false;

    /// <summary>
    /// The sound made when a hostile mob is killed when entering a protected grid.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱团结二 = new SoundPathSpecifier("/Audio/Effects/holy.ogg");
    // End Frontier
}
