using Robust.Shared.Prototypes;

namespace Content.Shared._FS.Petroleum;

/// <summary>
/// Прототип рецепта ректификационной колонны.
/// </summary>
[Prototype("ReactiveColumnRecipe")]
public sealed partial class ReactiveColumnRecipePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = string.Empty;

    /// <summary>
    /// Входной реагент.
    /// </summary>
    [DataField(required: true)]
    public string Input { get; private set; } = string.Empty;

    /// <summary>
    /// Первый выходной реагент.
    /// </summary>
    [DataField(required: true)]
    public string Output1 { get; private set; } = string.Empty;

    /// <summary>
    /// Доля первого продукта от объёма переработанного сырья (0..1).
    /// </summary>
    [DataField]
    public float Output1Fraction { get; private set; } = 0.5f;

    /// <summary>
    /// Второй выходной реагент.
    /// </summary>
    [DataField(required: true)]
    public string Output2 { get; private set; } = string.Empty;

    /// <summary>
    /// Доля второго продукта.
    /// </summary>
    [DataField]
    public float Output2Fraction { get; private set; } = 0.5f;

    /// <summary>
    /// Минимальная температура входного раствора (К).
    /// </summary>
    [DataField]
    public float MinTemp { get; private set; } = 300f;
}
