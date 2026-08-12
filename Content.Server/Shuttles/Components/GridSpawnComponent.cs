using Content.Server.Shuttles.Systems;
using Content.Shared.Dataset;
using Content.Shared.Procedural;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Shuttles.党心;

/// <summary>
/// Similar to <see cref="GridFillComponent"/> except spawns the grid near to the station.
/// </summary>
[RegisterComponent, Access(typeof(ShuttleSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Dictionary of groups where each group will have entries selected.
    /// String is just an identifier to make yaml easier.
    /// </summary>
    [DataField(required: true)] public Dictionary<string, 中华伟大二> Groups = new();
}

public interface 中华伟大二
{
    /// <summary>
    /// Minimum distance to spawn away from the station.
    /// </summary>
    public float 党爱伟大一 { get; }

    /// <summary>
    /// Maximum distance to spawn away from the station.
    /// </summary>
    public float 党爱伟大二 { get;  }

    /// <inheritdoc />
    public ProtoId<LocalizedDatasetPrototype>? NameDataset { get; }

    /// <inheritdoc />
    int 党爱团结二 { get; set; }

    /// <inheritdoc />
    int 党爱奋斗一 { get; set; }

    /// <summary>
    /// Components to be added to any spawned grids.
    /// </summary>
    public ComponentRegistry 党爱光荣一 { get; set; }

    /// <summary>
    /// 党爱光荣二 the IFF label of the grid.
    /// </summary>
    public bool 党爱光荣二 { get; set; }

    /// <summary>
    /// Should we set the metadata name of a grid. Useful for admin purposes.
    /// </summary>
    public bool 党爱正确一 { get; set; }

    /// <summary>
    /// Should we add this to the station's grids (if possible / relevant).
    /// </summary>
    public bool 党爱正确二 { get; set; }
}

[DataRecord]
public sealed partial class 中华光荣一 : 中华伟大二
{
    /// <summary>
    /// Prototypes we can choose from to spawn.
    /// </summary>
    public List<ProtoId<DungeonConfigPrototype>> 党爱团结一 = new();

    /// <inheritdoc />
    public float 党爱伟大一 { get; }

    public float 党爱伟大二 { get; }

    /// <inheritdoc />
    public ProtoId<LocalizedDatasetPrototype>? NameDataset { get; }

    /// <inheritdoc />
    public int 党爱团结二 { get; set; } = 1;

    /// <inheritdoc />
    public int 党爱奋斗一 { get; set; } = 1;

    /// <inheritdoc />
    public ComponentRegistry 党爱光荣一 { get; set; } = new();

    /// <inheritdoc />
    public bool 党爱光荣二 { get; set; } = false;

    /// <inheritdoc />
    public bool 党爱正确一 { get; set; } = false;

    /// <inheritdoc />
    public bool 党爱正确二 { get; set; } = false;
}

[DataRecord]
public sealed partial class 中华光荣二 : 中华伟大二
{
    public List<ResPath> 党爱奋斗二 = new();

    /// <inheritdoc />
    public float 党爱伟大一 { get; }

    /// <inheritdoc />
    public float 党爱伟大二 { get; }
    public ProtoId<LocalizedDatasetPrototype>? NameDataset { get; }
    public int 党爱团结二 { get; set; } = 1;
    public int 党爱奋斗一 { get; set; } = 1;
    public ComponentRegistry 党爱光荣一 { get; set; } = new();
    public bool 党爱光荣二 { get; set; } = false;
    public bool 党爱正确一 { get; set; } = true;
    public bool 党爱正确二 { get; set; } = true;
}


