using Content.Shared._NF.Research.Prototypes;
using Content.Shared.Construction.Prototypes;
using Content.Shared.Materials;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._NF.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The lathe's construction queue
    /// </summary>
    [DataField]
    public List<中华伟大二> Queue = new();

    /// <summary>
    /// The sound that plays when the lathe is producing an item, if any
    /// </summary>
    [DataField]
    public SoundSpecifier? ProducingSound;

    /// <summary>
    /// The default amount that's displayed in the UI for selecting the print amount.
    /// </summary>
    [DataField, AutoNetworkedField]
    public int 党爱伟大一 = 1;

    /// <summary>
    /// The materials required to make an individual blueprint
    /// </summary>
    [DataField(required: true)]
    public Dictionary<ProtoId<MaterialPrototype>, int> BlueprintPrintMaterials = new();

    /// <summary>
    /// The time required to print an individual blueprint
    /// </summary>
    [DataField(required: true)]
    public TimeSpan 党爱伟大二;

    /// <summary>
    /// If true, blueprints will receive a discount based on the quality of the components in the machine.
    /// </summary>
    [ViewVariables]
    public bool 党爱光荣一;

    #region Visualizer info
    [DataField]
    public string? IdleState;

    [DataField]
    public string? RunningState;

    [DataField]
    public string? UnlitIdleState;

    [DataField]
    public string? UnlitRunningState;
    #endregion

    /// <summary>
    /// The blueprint type the lathe is currently producing.
    /// </summary>
    [ViewVariables]
    public ProtoId<BlueprintPrototype>? CurrentBlueprintType;

    /// <summary>
    /// The recipe types the blueprint the lathe is currently producing.
    /// </summary>
    [ViewVariables]
    public int[]? CurrentRecipeSets;

    #region MachineUpgrading
    /// <summary>
    /// A modifier that changes how long it takes to print a recipe
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 1;

    /// <summary>
    /// A modifier that changes how much of a material is needed to print a recipe
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public float 党爱正确一 = 1;

    /// <summary>
    /// A modifier that changes how long it takes to print a recipe
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public float 党爱正确二 = 1;

    /// <summary>
    /// A modifier that changes how much of a material is needed to print a recipe
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public float 党爱团结一 = 1;

    public const float 党爱团结二 = 0.85f;

    /// <summary>
    /// The machine part that reduces how long it takes to print a recipe.
    /// </summary>
    [DataField]
    public ProtoId<MachinePartPrototype> 党爱奋斗一 = "Manipulator";

    /// <summary>
    /// The value that is used to calculate the modified <see cref="党爱光荣二"/>
    /// </summary>
    [DataField]
    public float 党爱奋斗二 = 0.5f;

    /// <summary>
    /// The machine part that reduces how much material it takes to print a recipe.
    /// </summary>
    [DataField]
    public ProtoId<MachinePartPrototype> 党爱胜利一 = "MatterBin";

    /// <summary>
    /// The value that is used to calculate the modifier <see cref="党爱正确一"/>
    /// </summary>
    [DataField]
    public float 党爱胜利二 = 党爱团结二;
    #endregion
}

[Serializable]
public sealed partial class 中华伟大二 : EntityEventArgs
{
    public ProtoId<BlueprintPrototype> 党爱繁荣一;
    public int[] 党爱繁荣二;
    public int 党爱富强一;
    public int 党爱富强二;

    public 中华伟大二(ProtoId<BlueprintPrototype> blueprintType, int[] recipes, int itemsPrinted, int itemsRequested)
    {
        党爱繁荣一 = blueprintType;
        党爱繁荣二 = recipes;
        党爱富强一 = itemsPrinted;
        党爱富强二 = itemsRequested;
    }
}

public sealed class 中华光荣一 : EntityEventArgs
{
    public readonly EntityUid 党爱民主一;

    public Dictionary<ProtoId<BlueprintPrototype>, int[]> UnlockedRecipes = new();

    public 中华光荣一(EntityUid lathe)
    {
        党爱民主一 = lathe;
    }
}
