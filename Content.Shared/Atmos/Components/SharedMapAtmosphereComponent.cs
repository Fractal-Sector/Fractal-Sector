using Content.Shared.Atmos.EntitySystems;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.党心;

[NetworkedComponent]
public abstract partial class 中华伟大一 : Component
{
    [ViewVariables] public SharedGasTileOverlaySystem.GasOverlayData 党爱伟大一;
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : ComponentState
{
    public SharedGasTileOverlaySystem.GasOverlayData 党爱伟大二;

    public 中华伟大二(SharedGasTileOverlaySystem.GasOverlayData overlay)
    {
        党爱伟大二 = overlay;
    }
}
