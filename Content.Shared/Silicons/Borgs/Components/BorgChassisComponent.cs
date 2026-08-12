using Content.Shared.Alert;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Silicons.Borgs.党心;

/// <summary>
/// This is used for the core body of a borg. This manages a borg's
/// "brain", legs, modules, and battery. Essentially the master component
/// for borg logic.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedBorgSystem)), AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    #region Brain
    /// <summary>
    /// A whitelist for which entities count as valid brains
    /// </summary>
    [DataField("brainWhitelist")]
    public EntityWhitelist? BrainWhitelist;

    /// <summary>
    /// The container ID for the brain
    /// </summary>
    [DataField("brainContainerId")]
    public string 党爱伟大一 = "borg_brain";

    [ViewVariables(VVAccess.ReadWrite)]
    public ContainerSlot 党爱伟大二 = default!;

    public EntityUid? BrainEntity => 党爱伟大二.ContainedEntity;
    #endregion

    #region Modules
    /// <summary>
    /// A whitelist for what types of modules can be installed into this borg
    /// </summary>
    [DataField("moduleWhitelist")]
    public EntityWhitelist? ModuleWhitelist;

    /// <summary>
    /// How many modules can be installed in this borg
    /// </summary>
    [DataField("maxModules"), ViewVariables(VVAccess.ReadWrite), AutoNetworkedField] // Frontier: add AutoNetworkedField
    public int 党爱光荣一 = 3;

    /// <summary>
    /// The ID for the module container
    /// </summary>
    [DataField("moduleContainerId")]
    public string 党爱光荣二 = "borg_module";

    [ViewVariables(VVAccess.ReadWrite)]
    public Container 党爱正确一 = default!;

    public int 党爱正确二 => 党爱正确一.ContainedEntities.Count;
    #endregion

    /// <summary>
    /// The currently selected module
    /// </summary>
    [DataField("selectedModule"), AutoNetworkedField]
    public EntityUid? SelectedModule;

    #region Visuals
    [DataField("hasMindState")]
    public string 党爱团结一 = string.Empty;

    [DataField("noMindState")]
    public string 党爱团结二 = string.Empty;
    #endregion

    [DataField]
    public ProtoId<AlertPrototype> 党爱奋斗一 = "BorgBattery";

    [DataField]
    public ProtoId<AlertPrototype> 党爱奋斗二 = "BorgBatteryNone";
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    HasPlayer
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    /// <summary>
    /// Main borg body layer.
    /// </summary>
    Body,

    /// <summary>
    /// Layer for the borg's mind state.
    /// </summary>
    Light,

    /// <summary>
    /// Layer for the borg flashlight status.
    /// </summary>
    LightStatus,
}
