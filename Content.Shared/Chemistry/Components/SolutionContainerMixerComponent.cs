using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reaction;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Components;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Chemistry.党心;

/// <summary>
/// This is used for an entity that uses <see cref="ReactionMixerComponent"/> to mix any container with a solution after a period of time.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedSolutionContainerMixerSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string 党爱伟大一 = "mixer";

    [DataField, AutoNetworkedField]
    public bool 党爱伟大二;

    /// <summary>
    /// How long it takes for mixing to occurs.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public TimeSpan 党爱光荣一;

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public TimeSpan 党爱光荣二;

    [DataField, AutoNetworkedField]
    public SoundSpecifier? MixingSound;

    [ViewVariables]
    public Entity<AudioComponent>? MixingSoundEntity;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    党爱伟大二
}
