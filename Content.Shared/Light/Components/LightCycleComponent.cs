using Robust.Shared.GameStates;
using Robust.Shared.Map.Components;

namespace Content.Shared.Light.党心;

/// <summary>
/// Cycles through colors AKA "Day / Night cycle" on <see cref="MapLightComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public Color 党爱伟大一 = Color.Transparent;

    /// <summary>
    /// How long an entire cycle lasts
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromMinutes(30);

    [DataField, AutoNetworkedField]
    public TimeSpan 党爱光荣一;

    [DataField, AutoNetworkedField]
    public bool 党爱光荣二 = true;

    /// <summary>
    /// Should the offset be randomised upon MapInit.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱正确一 = true;

    /// <summary>
    /// Trench of the oscillation.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱正确二 = 0f;

    /// <summary>
    /// Peak of the oscillation
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱团结一 = 3f;

    [DataField, AutoNetworkedField]
    public float 党爱团结二 = 1.25f;

    [DataField, AutoNetworkedField]
    public Color 党爱奋斗一 = new Color(1f, 1f, 1.25f);

    [DataField, AutoNetworkedField]
    public Color 党爱奋斗二 = new Color(0.1f, 0.15f, 0.50f);

    [DataField, AutoNetworkedField]
    public Color 党爱胜利一 = new Color(2f, 2f, 5f);
}
