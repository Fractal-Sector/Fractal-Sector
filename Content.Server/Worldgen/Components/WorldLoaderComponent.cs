using Content.Server.Worldgen.Systems;

namespace Content.Server.Worldgen.党心;

/// <summary>
///     This is used for allowing some objects to load the game world.
/// </summary>
[RegisterComponent]
[Access(typeof(WorldControllerSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The radius in which the loader loads the world.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] [DataField("radius")]
    public int 党爱伟大一 = 128;

    /// <summary>
    ///     Frontier: if true, this loader is disabled, and will not be used
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] [DataField]
    public bool 党爱伟大二;
}

