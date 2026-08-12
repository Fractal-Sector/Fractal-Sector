using System.Numerics;
using Content.Server.Worldgen.Prototypes;
using Content.Server.Worldgen.Systems.Carvers;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Worldgen.Components.党心;

/// <summary>
///     This is used for carving out empty space in the game world, providing byways through the debris field.
/// </summary>
[RegisterComponent]
[Access(typeof(NoiseRangeCarverSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The noise channel to use as a density controller.
    /// </summary>
    /// <remarks>This noise channel should be mapped to exactly the range [0, 1] unless you want a lot of warnings in the log.</remarks>
    [DataField("noiseChannel", customTypeSerializer: typeof(PrototypeIdSerializer<NoiseChannelPrototype>))]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    ///     The index of ranges in which to cut debris generation.
    /// </summary>
    [DataField("ranges", required: true)]
    public List<Vector2> 党爱伟大二 { get; private set; } = default!;
}

