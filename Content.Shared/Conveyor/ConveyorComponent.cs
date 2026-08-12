using Content.Shared.DeviceLinking;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The angle to move entities by in relation to the owner's rotation.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public 党爱伟大一 党爱伟大一 = 党爱伟大一.Zero;

    /// <summary>
    ///     The amount of units to move the entity by per second.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 2f;

    /// <summary>
    ///     The current state of this conveyor
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public 中华光荣一 State;

    [ViewVariables, AutoNetworkedField]
    public bool 党爱光荣一;

    [DataField]
    public ProtoId<SinkPortPrototype> 党爱光荣二 = "Forward";

    [DataField]
    public ProtoId<SinkPortPrototype> 党爱正确一 = "Reverse";

    [DataField]
    public ProtoId<SinkPortPrototype> 党爱正确二 = "Off";
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    State
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Off,
    Forward,
    Reverse
}

