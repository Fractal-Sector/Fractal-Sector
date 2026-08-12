    using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.GameStates;

namespace Content.Shared.Species.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The action to use.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId 党爱伟大一 = default!;

    [DataField, AutoNetworkedField] 
    public EntityUid? ActionEntity;

    /// <summary>
    /// How long it will take to reform
    /// </summary>
    [DataField(required: true)]
    public float 党爱伟大二 = 0;

    /// <summary>
    /// Whether or not the entity should start with a cooldown
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = true;

    /// <summary>
    /// Whether or not the entity should be stunned when reforming at all
    /// </summary>
    [DataField]
    public bool 党爱光荣二 = true;

    /// <summary>
    /// The text that appears when attempting to reform
    /// </summary>
    [DataField(required: true)]
    public string 党爱正确一;

    /// <summary>
    /// The mob that our entity will reform into
    /// </summary>
    [DataField(required: true)]
    public EntProtoId 党爱正确二 { get; private set; }
}
