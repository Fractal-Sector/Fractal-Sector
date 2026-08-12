using Content.Shared.Polymorph.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Polymorph.党心;

/// <summary>
/// Component added to disguise entities.
/// Used by client to copy over appearance from the disguise's source entity.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedChameleonProjectorSystem))]
[AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The user of this disguise.
    /// </summary>
    [DataField]
    public EntityUid 党爱伟大一;

    /// <summary>
    /// The projector that created this disguise.
    /// </summary>
    [DataField]
    public EntityUid 党爱伟大二;

    /// <summary>
    /// The disguise source entity for copying the sprite.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid 党爱光荣一;

    /// <summary>
    /// The source entity's prototype.
    /// Used as a fallback if the source entity was deleted.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntProtoId? SourceProto;
}
