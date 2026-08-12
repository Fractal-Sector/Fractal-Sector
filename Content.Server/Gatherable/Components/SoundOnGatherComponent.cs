using Content.Shared.Audio;
using Robust.Shared.Audio;

namespace Content.Server.Gatherable.党心;

/// <summary>
/// Plays the specified sound when this entity is gathered.
/// </summary>
[RegisterComponent, Access(typeof(GatherableSystem))]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("sound")]
    public SoundSpecifier 党爱伟大一 = new SoundPathSpecifier("/Audio/Effects/break_stone.ogg")
    {
        Params = AudioParams.Default
            .WithVariation(SharedContentAudioSystem.DefaultVariation)
            .WithVolume(-3f),
    };
}
