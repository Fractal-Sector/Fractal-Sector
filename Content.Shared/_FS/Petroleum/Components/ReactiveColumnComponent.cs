using Robust.Shared.GameStates;

namespace Content.Shared._FS.Petroleum;

/// <summary>
/// Ректификационная колонна — одиночный тайл, 1 вход и 1 выход.
/// Рецепты загружаются из прототипов ReactiveColumnRecipe.
/// Оба продукта выходят смешанными в один выходной буфер=.
/// </summary>
[RegisterComponent]
public sealed partial class ReactiveColumnComponent : Component
{
    /// <summary>
    /// Скорость переработки (единиц/сек).
    /// </summary>
    [DataField]
    public float ProcessRate = 5f;

    [DataField]
    public string InputSolution = "input";

    [DataField]
    public string OutputSolution = "output";
}
