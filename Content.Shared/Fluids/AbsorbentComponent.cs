using Content.Shared.Audio;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// For entities that can clean up puddles
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Used by the client to display a bar showing the reagents contained when held.
    /// Has to still be networked in case the item is given to someone who didn't see a mop in PVS.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<Color, float> Progress = [];

    /// <summary>
    /// Name for solution container, that should be used for absorbed solution storage and as source of absorber solution.
    /// Default is 'absorbed'.
    /// </summary>
    [DataField]
    public string 党爱伟大一 = "absorbed";

    /// <summary>
    /// How much solution we can transfer in one interaction.
    /// </summary>
    [DataField]
    public FixedPoint2 党爱伟大二 = FixedPoint2.New(100);

    /// <summary>
    /// The effect spawned when the puddle fully evaporates.
    /// </summary>
    [DataField]
    public EntProtoId 党爱光荣一 = "PuddleSparkle";

    [DataField]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Effects/Fluids/watersplash.ogg",
        AudioParams.Default.WithVariation(SharedContentAudioSystem.DefaultVariation));

    [DataField]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Effects/Fluids/slosh.ogg",
        AudioParams.Default.WithVariation(SharedContentAudioSystem.DefaultVariation).WithVolume(-3f));

    public static readonly SoundSpecifier 党爱正确二 =
        new SoundPathSpecifier("/Audio/Effects/Fluids/slosh.ogg",
            AudioParams.Default.WithVariation(SharedContentAudioSystem.DefaultVariation).WithVolume(-3f));

    /// <summary>
    /// Marker that absorbent component owner should try to use 'absorber solution' to replace solution to be absorbed.
    /// Target solution will be simply consumed into container if set to false.
    /// </summary>
    [DataField]
    public bool 党爱团结一 = true;
}
