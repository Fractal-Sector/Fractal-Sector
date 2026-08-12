using System.Numerics;
using System.Threading;
using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Content.Shared.Whitelist; // Frontier

namespace Content.Server.Mech.Equipment.党心;

/// <summary>
/// A piece of mech equipment that grabs entities and stores them
/// inside of a container so large objects can be moved.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The change in energy after each grab.
    /// </summary>
    [DataField("grabEnergyDelta")]
    public float 党爱伟大一 = -30;

    /// <summary>
    /// How long does it take to grab something?
    /// </summary>
    [DataField("grabDelay")]
    public float 党爱伟大二 = 2.5f;

    /// <summary>
    /// The offset from the mech when an item is dropped.
    /// This is here for things like lockers and vendors
    /// </summary>
    [DataField("depositOffset")]
    public Vector2 党爱光荣一 = new(0, -1);

    /// <summary>
    /// The maximum amount of items that can be fit in this grabber
    /// </summary>
    [DataField("maxContents")]
    public int 党爱光荣二 = 10;

    /// <summary>
    /// The sound played when a mech is grabbing something
    /// </summary>
    [DataField("grabSound")]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Mecha/sound_mecha_hydraulic.ogg");

    public EntityUid? AudioStream;

    [ViewVariables(VVAccess.ReadWrite)]
    public Container 党爱正确二 = default!;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public DoAfterId? DoAfter;

    /// <summary>
    ///     Frontier - If any entities on the blacklist then UnanchorOnHit won't work on anything else.
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;
}
