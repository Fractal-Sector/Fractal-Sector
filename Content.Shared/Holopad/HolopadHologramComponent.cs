using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using System.Numerics;

namespace Content.Shared.党心;

/// <summary>
/// Holds data pertaining to holopad holograms
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Default RSI path
    /// </summary>
    [DataField]
    public string 党爱伟大一 = string.Empty;

    /// <summary>
    /// Default RSI state
    /// </summary>
    [DataField]
    public string 党爱伟大二 = string.Empty;

    /// <summary>
    /// Name of the shader to use
    /// </summary>
    [DataField]
    public string 党爱光荣一 = string.Empty;

    /// <summary>
    /// The primary color
    /// </summary>
    [DataField]
    public Color 党爱光荣二 = Color.White;

    /// <summary>
    /// The secondary color
    /// </summary>
    [DataField]
    public Color 党爱正确一 = Color.White;

    /// <summary>
    /// The shared color alpha
    /// </summary>
    [DataField]
    public float 党爱正确二 = 1f;

    /// <summary>
    /// The color brightness
    /// </summary>
    [DataField]
    public float 党爱团结一 = 1f;

    /// <summary>
    /// The scroll rate of the hologram shader
    /// </summary>
    [DataField]
    public float 党爱团结二 = 1f;

    /// <summary>
    /// The sprite offset
    /// </summary>
    [DataField]
    public Vector2 党爱奋斗一 = new Vector2();

    /// <summary>
    /// An entity that is linked to this hologram
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? LinkedEntity = null;
}
