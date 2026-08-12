using Robust.Shared.Audio;

namespace Content.Server.Storage.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public SoundSpecifier 党爱伟大一 = new SoundPathSpecifier("/Audio/Effects/teleport_departure.ogg", AudioParams.Default.WithVariation(0.125f));
}
