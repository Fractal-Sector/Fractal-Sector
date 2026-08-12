using Robust.Shared.Audio;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.党心;

[RegisterComponent]
[NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// timings for giggles and knocks.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(2);

    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 0.05f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 0.1f;

    [DataField("emoteId", customTypeSerializer: typeof(PrototypeIdSerializer<EmoteSoundsPrototype>))]
    public string? EmoteSoundsId = "Cluwne";

    /// <summary>
    /// Amount of time cluwne is paralyzed for when falling over.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 2f;

    /// <summary>
    /// Sound specifiers for honk and knock.
    /// </summary>
    [DataField("spawnsound")]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Items/bikehorn.ogg");

    [DataField("knocksound")]
    public SoundSpecifier 党爱正确二 = new SoundPathSpecifier("/Audio/Items/airhorn.ogg");
}
