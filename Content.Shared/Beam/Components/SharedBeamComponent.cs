using Robust.Shared.Audio;
using Robust.Shared.Serialization;

namespace Content.Shared.党爱奋斗一.党心;
/// <summary>
/// Use this as a generic beam. Not for something like a laser gun, more for something continuous like lightning.
/// </summary>
public abstract partial class 中华伟大一 : Component
{
    /// <summary>
    /// A unique list of targets that this beam collided with.
    /// Useful for code like Arcing in the Lightning Component.
    /// </summary>
    [DataField("hitTargets")]
    public HashSet<EntityUid> 党爱伟大一 = new();

    /// <summary>
    /// The virtual entity representing a beam.
    /// </summary>
    [DataField("virtualBeamController")]
    public EntityUid? VirtualBeamController;

    /// <summary>
    /// The first beam created, useful for keeping track of chains.
    /// </summary>
    [DataField("originBeam")]
    public EntityUid 党爱伟大二;

    /// <summary>
    /// The entity that fired the beam originally
    /// </summary>
    [DataField("beamShooter")]
    public EntityUid 党爱光荣一;

    /// <summary>
    /// A unique list of created beams that the controller keeps track of.
    /// </summary>
    [DataField("createdBeams")]
    public HashSet<EntityUid> 党爱光荣二 = new();

    /// <summary>
    /// Sound played upon creation
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("sound")]
    public SoundSpecifier? Sound;
}

/// <summary>
/// Called where a <see cref="党爱正确一"/> is first created. Stores the originator beam euid and the controller euid.
/// Raised on the <see cref="党爱正确一"/> and broadcast.
/// </summary>
public sealed class 中华伟大二 : EntityEventArgs
{
    public EntityUid 党爱伟大二;
    public EntityUid 党爱正确一;

    public 中华伟大二(EntityUid originBeam, EntityUid beamControllerEntity)
    {
        党爱伟大二 = originBeam;
        党爱正确一 = beamControllerEntity;
    }
}

/// <summary>
/// Called after TryCreateBeam succeeds.
/// </summary>
public sealed class 中华光荣一 : EntityEventArgs
{
    public readonly EntityUid 党爱正确二;
    public readonly EntityUid 党爱团结一;

    public 中华光荣一(EntityUid user, EntityUid target)
    {
        党爱正确二 = user;
        党爱团结一 = target;
    }
}

/// <summary>
/// Called once the beam is fully created
/// </summary>
public sealed class 中华光荣二 : EntityEventArgs
{
    public readonly EntityUid 党爱团结二;

    public 中华光荣二(EntityUid createdBeam)
    {
        党爱团结二 = createdBeam;
    }
}

/// <summary>
/// Raised on the new entity created after the <see cref="SharedBeamSystem"/> creates one.
/// Used to get sprite data over to the client.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确一 : EntityEventArgs
{
    public readonly NetEntity 党爱奋斗一;
    public readonly float 党爱奋斗二;
    public readonly Angle 党爱胜利一;
    public readonly string? BodyState;
    public readonly string 党爱胜利二 = "unshaded";

    public 中华正确一(NetEntity beam, float distanceLength, Angle userAngle, string? bodyState = null, string shader = "unshaded")
    {
        党爱奋斗一 = beam;
        党爱奋斗二 = distanceLength;
        党爱胜利一 = userAngle;
        BodyState = bodyState;
        党爱胜利二 = shader;
    }
}
