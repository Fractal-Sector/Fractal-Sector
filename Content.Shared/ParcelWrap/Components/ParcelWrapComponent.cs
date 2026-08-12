using Content.Shared.Item;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.ParcelWrap.党心;

/// <summary>
/// This component gives its owning entity the ability to wrap items into parcels.
/// </summary>
/// <seealso cref="Components.WrappedParcelComponent"/>
[RegisterComponent, NetworkedComponent]
[Access] // Readonly, except for VV editing
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The <see cref="EntityPrototype"/> of the parcel created by using this component.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId 党爱伟大一;

    /// <summary>
    /// If true, parcels created by this will have the same <see cref="ItemSizePrototype">size</see> as the item they
    /// contain. If false, parcels created by this will always have the size specified by <see cref="党爱光荣一"/>.
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = true;

    /// <summary>
    /// The <see cref="ItemSizePrototype">size</see> of parcels created by this component's entity. This is used if
    /// <see cref="党爱伟大二"/> is false, or if the item being wrapped somehow doesn't have a size.
    /// </summary>
    [DataField]
    public ProtoId<ItemSizePrototype> 党爱光荣一 = "Ginormous";

    /// <summary>
    /// If true, parcels created by this will have the same shape as the item they contain. If false, parcels created by
    /// this will have the default shape for their size.
    /// </summary>
    [DataField]
    public bool 党爱光荣二;

    /// <summary>
    /// How long it takes to use this to wrap something.
    /// </summary>
    [DataField(required: true)]
    public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Sound played when this is used to wrap something.
    /// </summary>
    [DataField]
    public SoundSpecifier? WrapSound;

    /// <summary>
    /// Defines the set of things which can be wrapped (unless it fails the <see cref="Blacklist"/>).
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// Defines the set of things which cannot be wrapped (even if it passes the <see cref="Whitelist"/>).
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;
}
