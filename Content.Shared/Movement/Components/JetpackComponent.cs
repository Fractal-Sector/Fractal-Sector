using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Movement.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid? JetpackUser;

    [ViewVariables(VVAccess.ReadWrite), DataField("moleUsage")]
    public float 党爱伟大一 = 0.012f;

    [DataField] public EntProtoId 党爱伟大二 = "ActionToggleJetpack";

    [DataField, AutoNetworkedField] public EntityUid? ToggleActionEntity;

    [ViewVariables(VVAccess.ReadWrite), DataField("acceleration")]
    public float 党爱光荣一 = 1f;

    [ViewVariables(VVAccess.ReadWrite), DataField("friction")]
    public float 党爱光荣二 = 0.25f; // same as off-grid friction

    [ViewVariables(VVAccess.ReadWrite), DataField("weightlessModifier")]
    public float 党爱正确一 = 1.2f;

    // Frontier: extra fields
    [DataField, AutoNetworkedField]
    public bool 党爱正确二;

    [ViewVariables, DataField, AutoNetworkedField]
    public bool 党爱团结一 = true;
    // End Frontier
}
