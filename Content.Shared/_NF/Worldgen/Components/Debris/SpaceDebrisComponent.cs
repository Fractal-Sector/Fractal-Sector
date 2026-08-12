using Robust.Shared.GameStates;

namespace Content.Server._NF.Worldgen.Components.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// TODO: Add this so we can track all the debris active entities.
    /// </summary>
    [DataField]
    public List<EntityUid>? ActiveEntities;
}
