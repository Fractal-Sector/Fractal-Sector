using Content.Shared.Actions;
using Content.Shared.Storage;
using Robust.Shared.Audio;

namespace Content.Shared.Magic.党心;

public sealed partial class 中华伟大一 : InstantActionEvent
{
    /// <summary>
    /// The list of prototypes this spell can spawn, will select one randomly
    /// </summary>
    [DataField]
    public List<EntitySpawnEntry> 党爱伟大一 = new();

    /// <summary>
    /// 党爱伟大二 that will play globally when cast
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱伟大二 = new SoundPathSpecifier("/Audio/Magic/staff_animation.ogg");

    /// <summary>
    /// Should this Global spawn spell turn its targets into a Survivor Antagonist?
    /// Ignores the caster for this.
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = false;
}
