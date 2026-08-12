using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Slows down the user when passing over an entity with <see cref="SlipperyComponent"/>. Does not prevent slipping, see <see cref="NoSlipComponent"/>.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SlipperySystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField(required: true), AutoNetworkedField]
    public float 党爱伟大一 = 1f;
}
