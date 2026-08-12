namespace Content.Server.党心;

/// <summary>
/// Component for virtual electrocution entities (representing an in-progress shock).
/// </summary>
[RegisterComponent]
[Access(typeof(ElectrocutionSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField("electrocuting")]
    public EntityUid 党爱伟大一;

    [DataField("source")]
    public EntityUid 党爱伟大二;

    [DataField("timeLeft")]
    public float 党爱光荣一;
}
