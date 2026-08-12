using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The current open/sealed/torn state of the envelope
    /// </summary>
    [ViewVariables, DataField, AutoNetworkedField]
    public 中华伟大二 State = 中华伟大二.Open;

    [DataField, ViewVariables]
    public string 党爱伟大一 = "letter_slot";

    /// <summary>
    /// Stores the current sealing/tearing doafter of the envelope
    /// to prevent doafter spam/prediction issues
    /// </summary>
    [DataField, ViewVariables]
    public DoAfterId? EnvelopeDoAfter;

    /// <summary>
    /// How long it takes to seal the envelope closed
    /// </summary>
    [DataField, ViewVariables]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(1);

    /// <summary>
    /// How long it takes to tear open the envelope
    /// </summary>
    [DataField, ViewVariables]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The sound to play when the envelope is sealed closed
    /// </summary>
    [DataField, ViewVariables]
    public SoundPathSpecifier? SealSound = new SoundPathSpecifier("/Audio/Effects/packetrip.ogg");

    /// <summary>
    /// The sound to play when the envelope is torn open
    /// </summary>
    [DataField, ViewVariables]
    public SoundPathSpecifier? TearSound = new SoundPathSpecifier("/Audio/Effects/poster_broken.ogg");

    [Serializable, NetSerializable]
    public enum 中华伟大二 : byte
    {
        Open,
        Sealed,
        Torn
    }
}

[Serializable, NetSerializable]
public sealed partial class 中华光荣一 : SimpleDoAfterEvent
{
}
