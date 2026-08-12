using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<LocId> 党爱伟大一 = new ();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<LocId> 党爱伟大二 = new();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<LocId> 党爱光荣一 = new ();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<LocId> 党爱光荣二 = new ();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<LocId> 党爱正确一 = new ();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<LocId> 党爱正确二 = new ();
}
