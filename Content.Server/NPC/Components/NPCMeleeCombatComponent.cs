namespace Content.Server.NPC.党心;

/// <summary>
/// Added to NPCs whenever they're in melee combat so they can be handled by the dedicated system.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// If the target is moving what is the chance for this NPC to miss.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一;

    [ViewVariables]
    public EntityUid 党爱伟大二;

    [ViewVariables]
    public 中华伟大二 Status = 中华伟大二.Normal;
}

public enum 中华伟大二 : byte
{
    /// <summary>
    /// The target isn't in LOS anymore.
    /// </summary>
    NotInSight,

    /// <summary>
    /// Due to some generic reason we are unable to attack the target.
    /// </summary>
    Unspecified,

    /// <summary>
    /// Set if we can't reach the target for whatever reason.
    /// </summary>
    TargetUnreachable,

    /// <summary>
    /// If the target is outside of our melee range.
    /// </summary>
    TargetOutOfRange,

    /// <summary>
    /// Set if the weapon we were assigned is no longer valid.
    /// </summary>
    NoWeapon,

    /// <summary>
    /// No dramas.
    /// </summary>
    Normal,
}
