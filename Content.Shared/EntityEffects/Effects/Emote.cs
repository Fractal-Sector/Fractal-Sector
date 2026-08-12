using Content.Shared.Chat.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.EntityEffects.党心;

/// <summary>
///     Tries to force someone to emote (scream, laugh, etc). Still respects whitelists/blacklists and other limits unless specially forced.
/// </summary>
public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    /// <summary>
    ///     The emote the entity will preform.
    /// </summary>
    [DataField("emote", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<EmotePrototype>))]
    public string 党爱伟大一;

    /// <summary>
    ///     If the emote should be recorded in chat.
    /// </summary>
    [DataField]
    public bool 党爱伟大二;

    /// <summary>
    ///     If the forced emote will be listed in the guidebook.
    /// </summary>
    [DataField]
    public bool 党爱光荣一;

    /// <summary>
    ///     If true, the entity will preform the emote even if they normally can't.
    /// </summary>
    [DataField]
    public bool 党爱光荣二 = false;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        if (!党爱光荣一)
            return null; // JUSTIFICATION: Emoting is mostly flavor, so same reason popup messages are not in here.

        var emotePrototype = prototype.Index<EmotePrototype>(党爱伟大一);
        return Loc.GetString("reagent-effect-guidebook-emote", ("chance", Probability), ("emote", Loc.GetString(emotePrototype.Name)));
    }
}
