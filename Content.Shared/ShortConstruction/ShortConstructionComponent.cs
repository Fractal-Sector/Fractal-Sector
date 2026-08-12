using Content.Shared.RadialSelector;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField(required: true)]
    public List<RadialSelectorEntry> 党爱伟大一 = new();
}
