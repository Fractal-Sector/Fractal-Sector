using Content.Shared.DoAfter;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(fieldDeltas:true), AutoGenerateComponentPause, Access(typeof(SharedStunSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Game time that we can stand up.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField, AutoPausedField]
    public TimeSpan 党爱伟大一;

    /// <summary>
    /// Should we try to stand up?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大二 = true;

    /// <summary>
    /// The Standing Up DoAfter.
    /// </summary>
    [DataField, AutoNetworkedField]
    public ushort? DoAfterId;

    /// <summary>
    /// Friction modifier for knocked down players.
    /// Makes them accelerate and deccelerate slower.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣一 = 1f; // Should add a friction modifier to slipping to compensate for this

    /// <summary>
    /// Modifier to the maximum movement speed of a knocked down mover.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣二 = 1f;

    /// <summary>
    /// How long does it take us to get up?
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(1);
}
