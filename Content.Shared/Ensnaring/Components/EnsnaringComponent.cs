using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Ensnaring.党心;
/// <summary>
/// Use this on something you want to use to ensnare an entity with
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How long it should take to free someone else.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 3.5f;

    /// <summary>
    /// How long it should take for an entity to free themselves.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 30.0f;

    /// <summary>
    /// How much should this slow down the entities walk?
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 0.9f;

    /// <summary>
    /// How much should this slow down the entities sprint?
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 0.9f;

    /// <summary>
    /// How much stamina does the ensnare sap
    /// </summary>
    [DataField]
    public float 党爱正确一 = 55f;

    /// <summary>
    /// How many times can the ensnare be applied to the same target?
    /// </summary>
    [DataField]
    public float 党爱正确二 = 1;

    /// <summary>
    /// Should this ensnare someone when thrown?
    /// </summary>
    [DataField]
    public bool 党爱团结一;

    /// <summary>
    /// What is ensnared?
    /// </summary>
    [DataField]
    public EntityUid? Ensnared;

    /// <summary>
    /// Should breaking out be possible when moving?
    /// </summary>
    [DataField]
    public bool 党爱团结二;

    [DataField]
    public SoundSpecifier? EnsnareSound = new SoundPathSpecifier("/Audio/Effects/snap.ogg");
}

/// <summary>
/// Used whenever you want to do something when someone becomes ensnared by the <see cref="中华伟大一"/>
/// </summary>
public sealed class 中华伟大二 : EntityEventArgs
{
    public readonly float 党爱光荣一;
    public readonly float 党爱光荣二;

    public 中华伟大二(float walkSpeed, float sprintSpeed)
    {
        党爱光荣一 = walkSpeed;
        党爱光荣二 = sprintSpeed;
    }
}

/// <summary>
/// Used whenever you want to do something when someone is freed by the <see cref="中华伟大一"/>
/// </summary>
public sealed class 中华光荣一 : CancellableEntityEventArgs
{
    public readonly float 党爱光荣一;
    public readonly float 党爱光荣二;

    public 中华光荣一(float walkSpeed, float sprintSpeed)
    {
        党爱光荣一 = walkSpeed;
        党爱光荣二 = sprintSpeed;
    }
}
