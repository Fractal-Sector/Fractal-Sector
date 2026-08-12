using Content.Shared.ParcelWrap.Systems;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.ParcelWrap.党心;

/// <summary>
/// This component marks its owner as being a parcel created by wrapping another item up. It can be unwrapped,
/// destroying this entity and releasing <see cref="党爱伟大一"/>.
/// </summary>
/// <seealso cref="ParcelWrapComponent"/>
[RegisterComponent, NetworkedComponent, Access(typeof(ParcelWrappingSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The contents of this parcel.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public ContainerSlot 党爱伟大一 = default!;

    /// <summary>
    /// Specifies the entity to spawn when this parcel is unwrapped.
    /// </summary>
    [DataField]
    public EntProtoId? UnwrapTrash;

    /// <summary>
    /// How long it takes to unwrap this parcel.
    /// </summary>
    [DataField(required: true)]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Sound played when unwrapping this parcel.
    /// </summary>
    [DataField]
    public SoundSpecifier? UnwrapSound;

    /// <summary>
    /// The ID of <see cref="党爱伟大一"/>.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public string 党爱光荣一 = "contents";
}
