using Robust.Shared.GameStates;
using Robust.Shared.Maths;

namespace Content.Shared._FS.Petroleum;

/// <summary>
/// Мастер-блок ректификационной колонны. Одиночный тайл.
/// Принимает нефтяные фракции через PlumbingInlet с Севера,
/// выдаёт первый продукт через PlumbingOutlet на Юг,
/// второй продукт толкает в буфер порта (ReactiveColumnPortComponent),
/// который ставится рядом (по умолчанию — на Восток).
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ReactiveColumnComponent : Component
{
    /// <summary>
    /// Скорость обработки (единиц реагента в секунду).
    /// </summary>
    [DataField]
    public float ProcessRate = 5f;

    /// <summary>
    /// Название раствора для входного сырья.
    /// </summary>
    [DataField]
    public string InputSolution = "input";

    /// <summary>
    /// Название раствора для первого выхода (основной продукт, South).
    /// </summary>
    [DataField]
    public string Output1Solution = "output1";

    /// <summary>
    /// Смещение тайла порта второго выхода относительно мастера.
    /// По умолчанию East (+1, 0).
    /// </summary>
    [DataField]
    public Vector2i PortOffset = new(1, 0);

    /// <summary>
    /// Рецепты. Порядок имеет значение — берётся первый подходящий.
    /// </summary>
    [DataField]
    public List<ReactiveColumnRecipe> Recipes = new();

    /// <summary>
    /// Кэшированная ссылка на порт второго выхода.
    /// </summary>
    [ViewVariables]
    public EntityUid? Port;
}

/// <summary>
/// Порт второго выхода ректификационной колонны.
/// Ставится рядом с мастером (по умолчанию — на Восток).
/// Имеет PlumbingOutlet → выдаёт второй продукт в трубу.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ReactiveColumnPortComponent : Component
{
    /// <summary>
    /// Название раствора второго выхода.
    /// </summary>
    [DataField]
    public string Output2Solution = "output2";

    /// <summary>
    /// Обратная ссылка на мастер. Заполняется системой при анкоринге.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? Master;
}

/// <summary>
/// Один рецепт ректификационной колонны.
/// input → output1 (fraction1) + output2 (fraction2)
/// fraction1 + fraction2 должны равняться 1.0, но система не проверяет - на совести YAML.
/// </summary>
[DataDefinition]
public sealed partial class ReactiveColumnRecipe
{
    /// <summary>
    /// Входной реагент (Naphtha, LightOil, HeavyOil, IndustrialOil...).
    /// </summary>
    [DataField(required: true)]
    public string Input = string.Empty;

    /// <summary>
    /// Первый выходной реагент.
    /// </summary>
    [DataField(required: true)]
    public string Output1 = string.Empty;

    [DataField]
    public float Output1Fraction = 0.6f;

    /// <summary>
    /// Второй выходной реагент.
    /// </summary>
    [DataField(required: true)]
    public string Output2 = string.Empty;

    [DataField]
    public float Output2Fraction = 0.4f;

    /// <summary>
    /// Минимальная температура входного раствора для обработки (К).
    /// </summary>
    [DataField]
    public float MinTemp = 279f;
}
