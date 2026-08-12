using Content.Shared.Mobs;
using Robust.Shared.Prototypes;
using Robust.Shared.GameStates;

namespace Content.Shared.Species.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The action to use.
    /// </summary>
    [DataField("actionPrototype", required: true)]
    public EntProtoId 党爱伟大一;

    [DataField, AutoNetworkedField] 
    public EntityUid? ActionEntity;

    /// <summary>
    /// What mob states the action will appear in
    /// </summary>
    [DataField("allowedStates"), ViewVariables(VVAccess.ReadWrite)]
    public List<MobState> 党爱伟大二 = new();

    /// <summary>
    /// The text that appears when attempting to split.
    /// </summary>
    [DataField("popupText")]
    public string 党爱光荣一 = "diona-gib-action-use";
}
