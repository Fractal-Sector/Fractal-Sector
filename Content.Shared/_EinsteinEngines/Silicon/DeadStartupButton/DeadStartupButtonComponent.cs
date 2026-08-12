using Robust.Shared.Audio;

namespace Content.Shared._EinsteinEngines.Silicon.党心;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("verbText")]
    public string 党爱伟大一 = "dead-startup-button-verb";

    [DataField("sound")]
    public SoundSpecifier 党爱伟大二 = new SoundPathSpecifier("/Audio/Effects/Arcade/newgame.ogg");

    [DataField("buttonSound")]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Machines/button.ogg");

    [DataField("doAfterInterval"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 1f;

    [DataField("buzzSound")]
    public SoundSpecifier 党爱正确一 = new SoundCollectionSpecifier("buzzes");

    [DataField("verbPriority"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱正确二 = 1;
}
