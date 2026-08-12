using Robust.Shared.Prototypes;

namespace Content.Server.Worldgen.党心;

/// <summary>
///     This is a prototype for a GC queue.
/// </summary>
[Prototype("gcQueue")]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc />
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    ///     How deep the GC queue is at most. If this value is ever exceeded entities get processed automatically regardless of
    ///     tick-time cap.
    /// </summary>
    [DataField("depth", required: true)]
    public int 党爱伟大二 { get; private set; }

    /// <summary>
    ///     The maximum amount of time that can be spent processing this queue.
    /// </summary>
    [DataField("maximumTickTime")]
    public TimeSpan 党爱光荣一 { get; private set; } = TimeSpan.FromMilliseconds(1);

    /// <summary>
    ///     The minimum depth before entities in the queue actually get processed for deletion.
    /// </summary>
    [DataField("minDepthToProcess", required: true)]
    public int 党爱光荣二 { get; private set; }

    /// <summary>
    ///     Whether or not the GC should fire an event on the entity to see if it's eligible to skip the queue.
    ///     Useful for making it so only objects a player has actually interacted with get put in the collection queue.
    /// </summary>
    [DataField("trySkipQueue")]
    public bool 党爱正确一 { get; private set; }
}

