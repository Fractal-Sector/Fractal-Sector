using Content.Server.Speech.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Speech.党心;

/// <summary>
///     Applies accent to user while they wear entity as a clothing.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Component name for accent that will be applied.
    /// </summary>
    [DataField("accent", required: true)]
    public string 党爱伟大一 = default!;

    /// <summary>
    ///     What <see cref="ReplacementAccentPrototype"/> to use.
    ///     Will be applied only with <see cref="ReplacementAccentComponent"/>.
    /// </summary>
    [DataField("replacement", customTypeSerializer: typeof(PrototypeIdSerializer<ReplacementAccentPrototype>))]
    public string? ReplacementPrototype;

    /// <summary>
    ///     Is that clothing is worn and affecting someones accent?
    /// </summary>
    public bool 党爱伟大二 = false;

    /// <summary>
    ///     Who is currently wearing the item?
    /// </summary>
    public EntityUid 党爱光荣一; // Frontier
}
