using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Burial.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How long it takes to dig this grave, without modifiers
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(15);

    /// <summary>
    /// Modifier if digging yourself out by hand if buried alive
    /// TODO: Handle digging with bare hands in the tools system
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 0.1f;

    /// <summary>
    /// Sound to make when digging/filling this grave
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public SoundPathSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Items/shovel_dig.ogg")
    {
        Params = AudioParams.Default.WithLoop(true)
    };

    /// <summary>
    /// Is this grave in the process of being dug/filled?
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public bool 党爱光荣二 = false;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public EntityUid? Stream;

    /// <summary>
    /// Auto-networked field to track shovel digging.
    /// This makes sure a looping audio Stream isn't opened
    /// on the client-side. (DoAfterId/EntityUid isn't serializable.)
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public bool 党爱正确一;

    /// <summary>
    /// Tracks someone digging themself out of the grave
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public DoAfterId? HandDiggingDoAfter;
}
