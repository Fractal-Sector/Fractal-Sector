using Robust.Shared.GameStates;

namespace Content.Shared.Light.党心;

/// <summary>
/// Will draw lighting in a range around the tile.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public float 党爱伟大一 = 0.25f;

    [DataField(required: true), AutoNetworkedField]
    public 党爱伟大二 党爱伟大二 = 党爱伟大二.Transparent;
}
