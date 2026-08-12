using Content.Shared.Decals;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Light.党心;

/// <summary>
/// This is simplified version of <see cref="HandheldLightComponent"/>.
/// It doesn't consume any power and can be toggle only by verb.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField("toggleFlashlightSound")]
    public SoundSpecifier 党爱伟大一 = new SoundPathSpecifier("/Audio/Items/flashlight_pda.ogg");

    [DataField, AutoNetworkedField]
    public bool 党爱伟大二;

    [DataField]
    public EntProtoId 党爱光荣一 = "ActionToggleLight";

    [DataField, AutoNetworkedField]
    public EntityUid? ToggleActionEntity;

    /// <summary>
    ///  <see cref="ColorPalettePrototype"/> ID that determines the list
    /// of colors to select from when we get emagged
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<ColorPalettePrototype> 党爱光荣二 = "Emagged";
}
