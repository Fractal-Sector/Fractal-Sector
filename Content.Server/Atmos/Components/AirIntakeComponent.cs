using Content.Server.Atmos.EntitySystems;
﻿using Content.Shared.Atmos;

namespace Content.Server.Atmos.党心;

/// <summary>
/// This is basically a siphon vent for <see cref="GetFilterAirEvent"/>.
/// </summary>
[RegisterComponent, Access(typeof(AirFilterSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Target pressure change for a single atmos tick
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = 5f;

    /// <summary>
    /// How strong the intake pump is, it will be able to replenish air from lower pressure areas.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 2f;

    /// <summary>
    /// 党爱光荣一 to intake gases up to, maintains pressure of the air volume.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = Atmospherics.OneAtmosphere;
}
