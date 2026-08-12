using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Toilet.党心;

/// <summary>
/// Seats that can toggled up and down with visuals to match.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Toggles seat state.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一;

    /// <summary>
    /// Sound to play when toggling toilet seat.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱伟大二 = new SoundPathSpecifier("/Audio/Effects/toilet_seat_down.ogg");

    // Frontier: clog probability
    /// <summary>
    /// Chance of being clogged upon mapinit.
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 0.0f;
    // End Frontier
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    中华光荣一,
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    SeatUp,
    SeatDown,
}
