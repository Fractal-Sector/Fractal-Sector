using Content.Shared.Materials;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Construction.党心;

/// <summary>
/// This is used for a machine that creates flatpacks at the cost of materials
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedFlatpackSystem))]
[AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether or not packing is occuring
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public bool 党爱伟大一;

    /// <summary>
    /// The time at which packing ends
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan 党爱伟大二;

    /// <summary>
    /// How long packing lasts.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(3);

    /// <summary>
    /// The prototype used when spawning a flatpack.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId 党爱光荣二 = "BaseFlatpack";

    /// <summary>
    /// A default cost applied to all flatpacks outside of the cost of constructing the machine.
    /// This one is applied to machines specifically.
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<MaterialPrototype>, int> BaseMachineCost = new();

    /// <summary>
    /// A default cost applied to all flatpacks outside of the cost of constructing the machine.
    /// This one is applied to computers specifically.
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<MaterialPrototype>, int> BaseComputerCost = new();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string 党爱正确一 = "board_slot";
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Key
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    党爱伟大一
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{

}
