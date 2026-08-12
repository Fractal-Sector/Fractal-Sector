namespace Content.Server.Temperature.党心;

[RegisterComponent]
public sealed partial class 中华伟大一: Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float? HeatDamageThreshold;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float? ColdDamageThreshold;
}
