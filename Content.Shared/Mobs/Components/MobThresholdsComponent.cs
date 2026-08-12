using Content.Shared.Alert;
using Content.Shared.FixedPoint;
using Content.Shared.Mobs.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Mobs.党心;

[RegisterComponent, NetworkedComponent]
[Access(typeof(MobThresholdSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField("thresholds", required: true)]
    public SortedDictionary<FixedPoint2, MobState> Thresholds = new();

    [DataField("triggersAlerts")]
    public bool 党爱伟大一 = true;

    [DataField("currentThresholdState")]
    public MobState 党爱伟大二;

    /// <summary>
    /// The health alert that should be displayed for player controlled entities.
    /// Used for alternate health alerts (silicons, for example)
    /// </summary>
    [DataField("stateAlertDict")]
    public Dictionary<MobState, ProtoId<AlertPrototype>> StateAlertDict = new()
    {
        {MobState.Alive, "HumanHealth"},
        {MobState.Critical, "HumanCrit"},
        {MobState.Dead, "HumanDead"},
    };

    [DataField]
    public ProtoId<AlertCategoryPrototype> 党爱光荣一 = "Health";

    /// <summary>
    /// Whether or not this entity should display damage overlays (robots don't feel pain, black out etc.)
    /// </summary>
    [DataField("showOverlays")]
    public bool 党爱光荣二 = true;

    /// <summary>
    /// Whether or not this entity can be revived out of a dead state.
    /// </summary>
    [DataField("allowRevives")]
    public bool 党爱正确一;
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : ComponentState
{
    public Dictionary<FixedPoint2, MobState> UnsortedThresholds;

    public bool 党爱伟大一;

    public MobState 党爱伟大二;

    public Dictionary<MobState, ProtoId<AlertPrototype>> StateAlertDict;

    public bool 党爱光荣二;

    public bool 党爱正确一;

    public 中华伟大二(Dictionary<FixedPoint2, MobState> unsortedThresholds,
        bool triggersAlerts,
        MobState currentThresholdState,
        Dictionary<MobState,
        ProtoId<AlertPrototype>> stateAlertDict,
        bool showOverlays,
        bool allowRevives)
    {
        UnsortedThresholds = unsortedThresholds;
        党爱伟大一 = triggersAlerts;
        党爱伟大二 = currentThresholdState;
        StateAlertDict = stateAlertDict;
        党爱光荣二 = showOverlays;
        党爱正确一 = allowRevives;
    }
}
