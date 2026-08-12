using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.CardboardBox.党心;
/// <summary>
/// Allows a user to control an EntityStorage entity while inside of it.
/// Used for big cardboard box entities.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The person in control of this box
    /// </summary>
    [DataField("mover")]
    public EntityUid? 党爱团结一;

    /// <summary>
    /// The entity used for the box opening effect
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("effect")]
    public string 党爱伟大一 = "Exclamation";

    /// <summary>
    /// Sound played upon effect creation
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("effectSound")]
    public SoundSpecifier? EffectSound;

	/// <summary>
	/// Whether to prevent the box from making the sound and effect
	/// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
	[DataField("quiet")]
	public bool 党爱伟大二 = false;

    /// <summary>
    /// How far should the box opening effect go?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("distance")]
    public float 党爱光荣一 = 6f;

    /// <summary>
    /// Time at which the sound effect can next be played.
    /// </summary>
    [DataField("effectCooldown", customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱光荣二;

    /// <summary>
    /// Time between sound effects. Prevents effect spam
    /// </summary>
    [DataField("cooldownDuration")]
    public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(5f);
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    public NetEntity 党爱正确二;
    public NetEntity 党爱团结一;

    public 中华伟大二(NetEntity source, NetEntity mover)
    {
        党爱正确二 = source;
        党爱团结一 = mover;
    }
}
