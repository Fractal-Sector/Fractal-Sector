using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Used to prevent items from being unequipped and equipped from slots that are listed in <see cref="党爱伟大一"/>.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SlotBlockSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一 that this entity should block.
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public SlotFlags 党爱伟大一 = SlotFlags.NONE;
}
