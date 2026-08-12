using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Tools.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Tool quality for welding.
    /// </summary>
    [DataField]
    public ProtoId<ToolQualityPrototype> 党爱伟大一 = "Welding";

    /// <summary>
    ///     How much time does it take to weld/unweld entity.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(1f);

    /// <summary>
    ///     How much fuel does it take to weld/unweld entity.
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 3f;

    /// <summary>
    ///     Shown when welded entity is examined.
    /// </summary>
    [DataField]
    public LocId? WeldedExamineMessage = "weldable-component-examine-is-welded";

    /// <summary>
    ///     Is this entity currently welded shut?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣二;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    党爱光荣二
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    BaseWelded
}
