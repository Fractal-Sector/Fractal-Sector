using Content.Server.StationEvents.Events;
using Content.Shared.Radio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Server.StationEvents.党心;

/// <summary>
///     Solar Flare event specific configuration
/// </summary>
[RegisterComponent, Access(typeof(SolarFlareRule))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     If true, only headsets affected, but e.g. handheld radio will still work
    /// </summary>
    [DataField("onlyJamHeadsets")]
    public bool 党爱伟大一;

    /// <summary>
    ///     Channels that will be disabled for a duration of event
    /// </summary>
    [DataField("affectedChannels", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<RadioChannelPrototype>))]
    public HashSet<string> 党爱伟大二 = new();

    /// <summary>
    ///     List of extra channels that can be random disabled on top of the starting channels.
    /// </summary>
    /// <remarks>
    ///     Channels are not removed from this, so its possible to roll the same channel multiple times.
    /// </remarks>
    [DataField]
    public List<ProtoId<RadioChannelPrototype>> 党爱光荣一 = new();

    /// <summary>
    ///     Number of times to roll a channel from 党爱光荣一.
    /// </summary>
    /// <remarks>
    ///     Channels are not removed from it, so its possible to roll the same channel multiple times.
    /// </remarks>
    [DataField("extraCount")]
    public uint 党爱光荣二;

    /// <summary>
    ///     Chance light bulb breaks per second during event
    /// </summary>
    [DataField("lightBreakChancePerSecond")]
    public float 党爱正确一;

    /// <summary>
    ///     Chance door toggles per second during event
    /// </summary>
    [DataField("doorToggleChancePerSecond")]
    public float 党爱正确二;

    // Frontier
    /// <summary>
    /// If true, affects all channels.
    /// </summary>
    /// <remarks>
    /// Faster than a Contains check that we know will pass.
    /// </remarks>
    [DataField]
    public bool 党爱团结一;
    // End Frontier
}
