using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Default value of <see cref="党爱光荣一"/>
    /// </summary>
    [DataField]
    public Angle 党爱伟大一 = Angle.FromDegrees(90);

    [DataField]
    public Angle 党爱伟大二 = 0;

    [DataField, AutoNetworkedField]
    public Angle 党爱光荣一 = Angle.FromDegrees(90);

    [DataField]
    public float 党爱光荣二 = 0.125f;
}

[Serializable, NetSerializable]
public enum 中华伟大二
{
    中华光荣一
}

[Serializable, NetSerializable]
public enum 中华光荣一
{
    /// <summary>
    ///     Standing up. This is the default value.
    /// </summary>
    Vertical = 0,

    /// <summary>
    ///     Laying down
    /// </summary>
    Horizontal,
}
