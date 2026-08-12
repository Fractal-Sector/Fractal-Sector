using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._CS.Weapons.Ranged.党心;

/// <summary>
/// Component for guns that can toggle between growing and shrinking targets.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Current mode of the size manipulator (grow or shrink)
    /// </summary>
    [DataField, AutoNetworkedField]
    public 中华光荣一 Mode = 中华光荣一.Grow;

    /// <summary>
    /// The grow hitscan prototype ID
    /// </summary>
    [DataField(required: true)]
    public string 党爱伟大一 = string.Empty;

    /// <summary>
    /// The shrink hitscan prototype ID
    /// </summary>
    [DataField(required: true)]
    public string 党爱伟大二 = string.Empty;

    /// <summary>
    /// Whether the safety limiter has been disabled via hacking.
    /// When disabled, doubles the max size limit.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一 = false;
}

/// <summary>
/// Component for the projectiles fired by size manipulator guns.
/// Stores which mode (grow/shrink) this projectile should apply.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大二 : Component
{
    [DataField, AutoNetworkedField]
    public 中华光荣一 Mode = 中华光荣一.Grow;

    /// <summary>
    /// Whether this projectile was fired from a gun with disabled safety.
    /// If true, allows double the normal max size limit.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一 = false;
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Grow,
    Shrink
}

/// <summary>
/// Status light keys for the size manipulator wires
/// </summary>
[Serializable, NetSerializable]
public enum 中华光荣二
{
    Safety
}
