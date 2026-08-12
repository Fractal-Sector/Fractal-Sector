using Robust.Shared.Audio;

namespace Content.Server._EinsteinEngines.Silicon.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The max amount of power this source can provide in one sip.
    ///     No limit if null.
    /// </summary>
    [DataField]
    public int? MaxAmount = null;

    /// <summary>
    ///     The multiplier for the drink speed.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 1f;

    /// <summary>
    ///     The sound to play when the battery gets drunk from.
    /// </summary>
    [DataField]
    public SoundSpecifier? DrinkSound = new SoundCollectionSpecifier("sparks");
}
