using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Chemistry.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 党爱伟大一 { get; set; } = FixedPoint2.New(10);

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public 中华伟大二 TankType { get; set; } = 中华伟大二.Unspecified;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Unspecified,
    Fuel
}
