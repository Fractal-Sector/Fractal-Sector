using Robust.Shared.GameStates;
using Content.Shared.Drunk;

namespace Content.Shared.Traits.党心;

/// <summary>
/// Used for the lightweight trait. DrunkSystem will check for this component and modify the boozePower accordingly if it finds it.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedDrunkSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField("boozeStrengthMultiplier"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = 4f;
}
