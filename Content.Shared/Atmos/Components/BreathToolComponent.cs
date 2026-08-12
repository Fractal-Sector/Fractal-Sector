using Content.Shared.Body.Components;
using Content.Shared.Inventory;
using Robust.Shared.GameStates;

namespace Content.Shared.Atmos.党心;

/// <summary>
/// Gas masks or the likes; used by <see cref="InternalsComponent"/> for breathing.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[ComponentProtoName("BreathMask")]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Tool is functional only in allowed slots
    /// </summary>
    [DataField]
    public SlotFlags 党爱伟大一 = SlotFlags.MASK | SlotFlags.HEAD;

    [ViewVariables]
    public bool 党爱伟大二 => ConnectedInternalsEntity != null;

    /// <summary>
    /// Entity that the breath tool is currently connected to.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? ConnectedInternalsEntity;
}
