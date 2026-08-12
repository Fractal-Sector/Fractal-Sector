using Content.Server.Radiation.Systems;
using Content.Shared.Radiation.Components;

namespace Content.Server.Radiation.党心;

/// <summary>
///     Marks component that receive radiation from <see cref="RadiationSourceComponent"/>.
/// </summary>
[RegisterComponent]
[Access(typeof(RadiationSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Current radiation value in rads per second.
    ///     Periodically updated by radiation system.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float 党爱伟大一;
}

