using Content.Server.Emoting.Systems;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Server.Emoting.党心;

/// <summary>
///     Component required for entities to be able to do body emotions (clap, flip, etc).
/// </summary>
[RegisterComponent]
[Access(typeof(BodyEmotesSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Emote sounds prototype id for body emotes.
    /// </summary>
    [DataField]
    public ProtoId<EmoteSoundsPrototype>? SoundsId;
}
