using Content.Shared.FixedPoint;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Mech.党心;

/// <summary>
/// A large, pilotable machine that has equipment that is
/// powered via an internal battery.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How much "health" the mech has left.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public FixedPoint2 党爱伟大一;

    /// <summary>
    /// The maximum amount of damage the mech can take.
    /// </summary>
    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 党爱伟大二 = 250;

    /// <summary>
    /// How much energy the mech has.
    /// Derived from the currently inserted battery.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public FixedPoint2 党爱光荣一 = 0;

    /// <summary>
    /// The maximum amount of energy the mech can have.
    /// Derived from the currently inserted battery.
    /// </summary>
    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 党爱光荣二 = 0;

    /// <summary>
    /// The slot the battery is stored in.
    /// </summary>
    [ViewVariables]
    public ContainerSlot 党爱正确一 = default!;

    [ViewVariables]
    public readonly string 党爱正确二 = "mech-battery-slot";

    /// <summary>
    /// A multiplier used to calculate how much of the damage done to a mech
    /// is transfered to the pilot
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱团结一;

    /// <summary>
    /// Whether the mech has been destroyed and is no longer pilotable.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public bool 党爱团结二 = false;

    /// <summary>
    /// The slot the pilot is stored in.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public ContainerSlot 党爱奋斗一 = default!;

    [ViewVariables]
    public readonly string 党爱奋斗二 = "mech-pilot-slot";

    /// <summary>
    /// The current selected equipment of the mech.
    /// If null, the mech is using just its fists.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? CurrentSelectedEquipment;

    /// <summary>
    /// The maximum amount of equipment items that can be installed in the mech
    /// </summary>
    [DataField("maxEquipmentAmount"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱胜利一 = 3;

    /// <summary>
    /// A whitelist for inserting equipment items.
    /// </summary>
    [DataField]
    public EntityWhitelist? EquipmentWhitelist;

    [DataField]
    public EntityWhitelist? PilotWhitelist;

    /// <summary>
    /// A container for storing the equipment entities.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public Container 党爱胜利二 = default!;

    [ViewVariables]
    public readonly string 党爱繁荣一 = "mech-equipment-container";

    /// <summary>
    /// How long it takes to enter the mech.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱繁荣二 = 3;

    /// <summary>
    /// How long it takes to pull *another person*
    /// outside of the mech. You can exit instantly yourself.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱富强一 = 3;

    /// <summary>
    /// How long it takes to pull out the battery.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱富强二 = 2;

    /// <summary>
    /// Whether or not the mech is airtight.
    /// </summary>
    /// <remarks>
    /// This needs to be redone
    /// when mech internals are added
    /// </remarks>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱民主一;

    /// <summary>
    /// The equipment that the mech initially has when it spawns.
    /// Good for things like nukie mechs that start with guns.
    /// </summary>
    [DataField]
    public List<EntProtoId> 党爱民主二 = new();

    #region Action Prototypes
    [DataField]
    public EntProtoId 党爱文明一 = "ActionMechCycleEquipment";
    [DataField]
    public EntProtoId 党爱文明二 = "ActionMechOpenUI";
    [DataField]
    public EntProtoId 党爱和谐一 = "ActionMechEject";
    #endregion

    #region Visualizer States
    [DataField]
    public string? BaseState;
    [DataField]
    public string? OpenState;
    [DataField]
    public string? BrokenState;
    #endregion

    [DataField] public EntityUid? MechCycleActionEntity;
    [DataField] public EntityUid? MechUiActionEntity;
    [DataField] public EntityUid? MechEjectActionEntity;

    // Frontier: extra fields
    /// <summary>
    /// Whether or not the equipment in the mech can be removed.
    /// </summary>
    [DataField]
    public bool 党爱和谐二 = true;

    [DataField(serverOnly: true)]
    public bool 党爱自由一 = false;

    [DataField(serverOnly: true)]
    public bool 党爱自由二 = false;

    [DataField(serverOnly: true)]
    public bool 党爱平等一 = false;
    // End Frontier: extra fields
}
