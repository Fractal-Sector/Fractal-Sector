using Content.Shared.Atmos;
using Robust.Shared.GameStates;
using Robust.Shared.Maths;

namespace Content.Shared._FS.Petroleum;

public enum OilRefineryPartType : byte
{
    Input   = 0,
    Naphtha = 1,
    Light   = 2,
    Heavy   = 3,
    Gas     = 4,
}

/// <summary>
/// Мастер-блок НПЗ. Сам он не подключён ни к каким трубам - только обрабатывает
/// нефть и раздаёт продукты в буферы соседних модулей. Ссылки на модули
/// кэшируются один раз при анкоринге и никогда не ищутся по радиусу в рантайме.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class OilRefineryComponent : Component
{
    /// <summary>
    /// Минимальная температура нефти для начала переработки (К). Нефть должна быть горячей!
    /// </summary>
    [DataField]
    public float MinProcessTemp = 500f;

    /// <summary>
    /// Максимальная скорость переработки (единиц в секунду).
    /// </summary>
    [DataField]
    public float ProcessRate = 10f;

    [DataField, AutoNetworkedField]
    public float SulfurGunk = 0f;

    [DataField]
    public float MaxSulfurGunk = 100f;

    /// <summary>
    /// Молей атмосферного газа на единицу переработанной нефти.
    /// </summary>
    [DataField]
    public float GasMolesPerUnit = 0.5f;


    /// <summary>
    /// Смещения тайлов от мастера к каждому модулю
    /// </summary>
    [DataField]
    public Vector2i InputOffset = new( 0,  1);

    [DataField]
    public Vector2i NaphthaOffset = new( 0, -1);

    [DataField]
    public Vector2i LightOffset = new( 1,  0);

    [DataField]
    public Vector2i HeavyOffset = new(-1,  0);

    [DataField]
    public Vector2i GasOffset = new( 1,  1);

    /// <summary>
    /// Кэшированные ссылки на модули
    /// </summary>
    [ViewVariables] public EntityUid? InputPart;
    [ViewVariables] public EntityUid? NaphthaPart;
    [ViewVariables] public EntityUid? LightPart;
    [ViewVariables] public EntityUid? HeavyPart;
    [ViewVariables] public EntityUid? GasPart;
}

/// <summary>
/// Спутниковый модуль НПЗ. Знает свой тип и хранит обратную ссылку на мастер,
/// чтобы вербы на любом тайле завода работали через мастера.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class OilRefineryPartComponent : Component
{
    [DataField(required: true)]
    public OilRefineryPartType PartType;

    /// <summary>
    /// Ссылка на мастер-блок. null если модуль ещё не привязан.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? Master;
}

/// <summary>
/// Ставится на газовый тайл вместе с OilRefineryPartComponent.
/// Накапливает моли газа, которые мастер записывает в PendingMoles,
/// и сбрасывает их в атмос-трубу при AtmosDeviceUpdateEvent.
///
/// На этом тайле НЕТ SolutionContainerManager и PlumbingOutlet -
/// попутный газ это настоящая атмосфера, а не химический реагент.
/// </summary>
[RegisterComponent]
public sealed partial class OilRefineryGasOutletComponent : Component
{
    /// <summary>
    /// Имя PipeNode в NodeContainer этого энтити.
    /// </summary>
    [DataField]
    public string PipeNodeName = "pipe";

    /// <summary>
    /// Какой атмосферный газ выходит.
    /// </summary>
    [DataField]
    public Gas GasType = Gas.Petroleum;

    /// <summary>
    /// Температура газа при выходе в трубу (К).
    /// </summary>
    [DataField]
    public float GasReleaseTemp = 400f;

    /// <summary>
    /// Моли, накопленные с последнего тика атмосферы.
    /// </summary>
    [ViewVariables]
    public float PendingMoles;
}
