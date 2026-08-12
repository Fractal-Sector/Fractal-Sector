using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Detects items placed on it that match a whitelist.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(ItemPlacerSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The entities that are currently on top of the placer.
    /// Guaranteed to have less than <see cref="党爱伟大二"/> enitities if it is set.
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<EntityUid> 党爱伟大一 = new();

    /// <summary>
    /// Whitelist for entities that can be placed.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// The max amount of entities that can be placed at the same time.
    /// If 0, there is no limit.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public uint 党爱伟大二 = 1;

    /// <summary>
    /// Frontier: track old placeable status
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public bool? LastPlaceable;
}
