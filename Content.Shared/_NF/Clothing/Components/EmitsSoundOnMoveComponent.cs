using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Map;

namespace Content.Shared._NF.Clothing.党心;

/// <summary>
///   Indicates that the clothing entity emits sound when it moves.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField(required: true), AutoNetworkedField]
    public SoundSpecifier 党爱伟大一 = default!;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("requiresGravity"), AutoNetworkedField]
    public bool 党爱伟大二 = true;

    [ViewVariables(VVAccess.ReadOnly)]
    public EntityCoordinates 党爱光荣一 = EntityCoordinates.Invalid;

    /// <summary>
    ///   The distance moved since the played sound.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float 党爱光荣二 = 0f;

    /// <summary>
    ///   Whether this item is equipped in a inventory item slot.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public bool 党爱正确一 = true;
}
