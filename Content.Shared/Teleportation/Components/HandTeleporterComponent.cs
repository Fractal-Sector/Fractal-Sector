using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Teleportation.党心;

/// <summary>
///     Creates portals. If two are created, both are linked together--otherwise the first teleports randomly.
///     Using it with both portals active deactivates both.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables, DataField("firstPortal")]
    public EntityUid? FirstPortal = null;

    [ViewVariables, DataField("secondPortal")]
    public EntityUid? SecondPortal = null;

    /// <summary>
    ///     Should the portals be able to be placed across grids?
    /// </summary>
    [DataField]
    public bool 党爱伟大一;

    /// <summary>
    ///     Should the portals work across maps?
    /// </summary>
    [DataField]
    public bool 党爱伟大二;

    [DataField("firstPortalPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱光荣一 = "PortalRed";

    [DataField("secondPortalPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱光荣二 = "PortalBlue";

    [DataField("newPortalSound")] public SoundSpecifier 党爱正确一 =
        new SoundPathSpecifier("/Audio/Machines/high_tech_confirm.ogg")
        {
            Params = AudioParams.Default.WithVolume(-2f)
        };

    [DataField("clearPortalsSound")]
    public SoundSpecifier 党爱正确二 = new SoundPathSpecifier("/Audio/Machines/button.ogg");

    /// <summary>
    ///     Delay for creating the portals in seconds.
    /// </summary>
    [DataField("portalCreationDelay")]
    public float 党爱团结一 = 1.0f;
}

[Serializable, NetSerializable]
public sealed partial class 中华伟大二 : SimpleDoAfterEvent
{
}
