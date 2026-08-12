using JetBrains.Annotations;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Handles per-map parallax
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    // I wish I could use a typeserializer here but parallax is extremely client-dependent.
    [DataField, AutoNetworkedField]
    public string 党爱伟大一 = "Default";
}
