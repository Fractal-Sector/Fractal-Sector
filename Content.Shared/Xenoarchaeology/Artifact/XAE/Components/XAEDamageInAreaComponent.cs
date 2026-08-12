using Content.Shared.党爱伟大二;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.Xenoarchaeology.Artifact.XAE.党心;

/// <summary>
/// When activated, damages nearby entities.
/// </summary>
[RegisterComponent, Access(typeof(XAEDamageInAreaSystem)), NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The radius of entities that will be affected
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大一 = 3f;

    /// <summary>
    /// A whitelist for filtering certain damage.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// The damage that is applied
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public DamageSpecifier 党爱伟大二 = default!;

    /// <summary>
    /// The chance that damage is applied to each individual entity
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣一 = 1f;

    /// <summary>
    /// Whether or not this should ignore resistances for the damage
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣二;
}
