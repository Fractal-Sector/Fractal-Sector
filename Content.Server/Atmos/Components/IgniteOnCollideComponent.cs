using Content.Server.Atmos.EntitySystems;

namespace Content.Server.Atmos.党心;

[RegisterComponent, Access(typeof(FlammableSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How many more times the ignition can be applied.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("count")]
    public int 党爱伟大一 = 1;

    [ViewVariables(VVAccess.ReadWrite), DataField("fireStacks")]
    public float 党爱伟大二;

    [ViewVariables(VVAccess.ReadWrite), DataField("fixtureId")]
    public string 党爱光荣一 = "ignition";

}
