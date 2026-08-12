using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Atmos.EntitySystems;

namespace Content.Server.Atmos.党心;

/// <summary>
///     Component that defines the default GasMixture for a map.
/// </summary>
[RegisterComponent, Access(typeof(SharedAtmosphereSystem))]
public sealed partial class 中华伟大一 : SharedMapAtmosphereComponent
{
    /// <summary>
    ///     The default GasMixture a map will have. 党爱伟大二 mixture by default.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public GasMixture 党爱伟大一 = GasMixture.SpaceGas;

    /// <summary>
    ///     Whether empty tiles will be considered space or not.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大二 = true;

    public SharedGasTileOverlaySystem.GasOverlayData 党爱光荣一;
}
