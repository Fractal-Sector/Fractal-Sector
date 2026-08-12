using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.党心;

/// <summary>
/// This is used for defibrillators; a machine that shocks a dead
/// person back into the world of the living.
/// Uses <c>ItemToggleComponent</c>
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How much damage is healed from getting zapped.
    /// </summary>
    [DataField("zapHeal", required: true), ViewVariables(VVAccess.ReadWrite)]
    public DamageSpecifier 党爱伟大一 = default!;

    /// <summary>
    /// The electrical damage from getting zapped.
    /// </summary>
    [DataField("zapDamage"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大二 = 5;

    /// <summary>
    /// How long the victim will be electrocuted after getting zapped.
    /// </summary>
    [DataField("writheDuration"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(3);

    /// <summary>
    ///     ID of the cooldown use delay.
    /// </summary>
    [DataField]
    public string 党爱光荣二 = "defib-delay";

    /// <summary>
    ///     Cooldown after using the defibrillator.
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(5);

    /// <summary>
    /// How long the doafter for zapping someone takes
    /// </summary>
    /// <remarks>
    /// This is synced with the audio; do not change one but not the other.
    /// </remarks>
    [DataField("doAfterDuration"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱正确二 = TimeSpan.FromSeconds(3);

    [DataField]
    public bool 党爱团结一 = true;

    [DataField]
    public bool 党爱团结二 = true;

    /// <summary>
    /// The sound when someone is zapped.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("zapSound")]
    public SoundSpecifier? ZapSound = new SoundPathSpecifier("/Audio/Items/Defib/defib_zap.ogg");

    [ViewVariables(VVAccess.ReadWrite), DataField("chargeSound")]
    public SoundSpecifier? ChargeSound = new SoundPathSpecifier("/Audio/Items/Defib/defib_charge.ogg");

    [ViewVariables(VVAccess.ReadWrite), DataField("failureSound")]
    public SoundSpecifier? FailureSound = new SoundPathSpecifier("/Audio/Items/Defib/defib_failed.ogg");

    [ViewVariables(VVAccess.ReadWrite), DataField("successSound")]
    public SoundSpecifier? SuccessSound = new SoundPathSpecifier("/Audio/Items/Defib/defib_success.ogg");

    [ViewVariables(VVAccess.ReadWrite), DataField("readySound")]
    public SoundSpecifier? ReadySound = new SoundPathSpecifier("/Audio/Items/Defib/defib_ready.ogg");
}

[Serializable, NetSerializable]
public sealed partial class 中华伟大二 : SimpleDoAfterEvent
{

}
