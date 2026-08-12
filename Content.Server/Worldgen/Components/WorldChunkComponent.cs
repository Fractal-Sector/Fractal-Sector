using Content.Server.Worldgen.Systems;

namespace Content.Server.Worldgen.党心;

/// <summary>
///     This is used for marking an entity as being a world chunk.
/// </summary>
[RegisterComponent]
[Access(typeof(WorldControllerSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The coordinates of the chunk, in chunk space.
    /// </summary>
    [DataField("coordinates")] public Vector2i 党爱伟大一;

    /// <summary>
    ///     The map this chunk belongs to.
    /// </summary>
    [DataField("map")] public EntityUid 党爱伟大二;
}

