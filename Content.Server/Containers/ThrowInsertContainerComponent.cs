using Robust.Shared.Audio;

namespace Content.Server.党心;

/// <summary>
/// Allows objects to fall inside the Container when thrown
/// </summary>
[RegisterComponent]
[Access(typeof(ThrowInsertContainerSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField(required: true)]
    public string 党爱伟大一 = string.Empty;

    /// <summary>
    /// Throw chance of hitting into the container
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 0.25f;

    /// <summary>
    /// Sound played when an object is throw into the container.
    /// </summary>
    [DataField]
    public SoundSpecifier? InsertSound = new SoundPathSpecifier("/Audio/Effects/trashbag1.ogg");

    /// <summary>
    /// Sound played when an item is thrown and misses the container.
    /// </summary>
    [DataField]
    public SoundSpecifier? MissSound = new SoundPathSpecifier("/Audio/Effects/thudswoosh.ogg");

    [DataField]
    public LocId 党爱光荣一 = "container-thrown-missed";
}
