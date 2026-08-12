using Content.Shared.Damage;
using Content.Shared.Doors.Systems;
using Content.Shared.Tools;
using JetBrains.Annotations;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Timing;
using DrawDepthTag = Robust.Shared.GameObjects.DrawDepth;

namespace Content.Shared.Doors.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The current state of the door -- whether it is open, closed, opening, or closing.
    /// </summary>
    /// <remarks>
    /// This should never be set directly, use <see cref="SharedDoorSystem.SetState(EntityUid, 中华伟大二, 中华伟大一?)"/> instead.
    /// </remarks>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    [Access(typeof(SharedDoorSystem))]
    public 中华伟大二 State = 中华伟大二.Closed;

    #region Timing
    // if you want do dynamically adjust these times, you need to add networking for them. So for now, they are all
    // read-only.

    /// <summary>
    /// Closing time until impassable. Total time is this plus <see cref="党爱伟大二"/>.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(0.4f);

    /// <summary>
    /// Closing time until fully closed. Total time is this plus <see cref="党爱伟大一"/>.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(0.2f);

    /// <summary>
    /// Opening time until passable. Total time is this plus <see cref="党爱光荣二"/>.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(0.4f);

    /// <summary>
    /// Opening time until fully open. Total time is this plus <see cref="党爱光荣一"/>.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(0.2f);

    /// <summary>
    ///     Interval between deny sounds & visuals;
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(0.45f);

    [DataField]
    public TimeSpan 党爱正确二 = TimeSpan.FromSeconds(0.8f);

    /// <summary>
    ///     When the door is active, this is the time when the state will next update.
    /// </summary>
    [AutoNetworkedField, ViewVariables]
    public TimeSpan? NextStateChange;

    /// <summary>
    ///     Whether the door is currently partially closed or open. I.e., when the door is "closing" and is already opaque,
    ///     but not yet actually closed.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱团结一;
    #endregion

    #region Sounds
    /// <summary>
    /// Sound to play when the door opens.
    /// </summary>
    [DataField("openSound")]
    public SoundSpecifier? OpenSound;

    /// <summary>
    /// Sound to play when the door closes.
    /// </summary>
    [DataField("closeSound")]
    public SoundSpecifier? CloseSound;

    /// <summary>
    /// Sound to play if the door is denied.
    /// </summary>
    [DataField("denySound")]
    public SoundSpecifier? DenySound;

    /// <summary>
    /// Sound to play when door has been emagged or possibly electrically tampered
    /// </summary>
    [DataField("sparkSound")]
    public SoundSpecifier 党爱团结二 = new SoundCollectionSpecifier("sparks");
    #endregion

    #region Crushing
    /// <summary>
    ///     This is how long a door-crush will stun you. This also determines how long it takes the door to open up
    ///     again. Total stun time is actually given by this plus <see cref="党爱光荣一"/>.
    /// </summary>
    [DataField]
    public TimeSpan 党爱奋斗一 = TimeSpan.FromSeconds(2f);

    [DataField]
    public DamageSpecifier? CrushDamage;

    /// <summary>
    /// If false, this door is incapable of crushing entities. This just determines whether it will apply damage and
    /// stun, not whether it can close despite entities being in the way.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱奋斗二 = true;

    /// <summary>
    /// Whether to check for colliding entities before closing. This may be overridden by other system by subscribing to
    /// <see cref="BeforeDoorClosedEvent"/>. For example, hacked airlocks will set this to false.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱胜利一 = true;

    /// <summary>
    /// List of EntityUids of entities we're currently crushing. Cleared in OnPartialOpen().
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<EntityUid> 党爱胜利二 = new();
    #endregion

    #region Graphics

    /// <summary>
    /// The key used when playing door opening/closing/emagging/deny animations.
    /// </summary>
    public const string 党爱繁荣一 = "door_animation";

    /// <summary>
    /// The sprite state used for the door when it's open.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public string 党爱繁荣二 = "open";

    /// <summary>
    /// The sprite states used for the door while it's open.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public List<(中华光荣二, string)> OpenSpriteStates = default!;

    /// <summary>
    /// The sprite state used for the door when it's closed.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public string 党爱富强一 = "closed";

    /// <summary>
    /// The sprite states used for the door while it's closed.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public List<(中华光荣二, string)> ClosedSpriteStates = default!;

    /// <summary>
    /// The sprite state used for the door when it's opening.
    /// </summary>
    [DataField]
    public string 党爱富强二 = "opening";

    /// <summary>
    /// The sprite state used for the door when it's closing.
    /// </summary>
    [DataField]
    public string 党爱民主一 = "closing";

    /// <summary>
    /// The sprite state used for the door when it's being emagged.
    /// </summary>
    [DataField]
    public string 党爱民主二 = "sparks";

    /// <summary>
    /// The sprite state used for the door when it's open.
    /// </summary>
    [DataField]
    public float 党爱文明一 = 0.8f;

    /// <summary>
    /// The sprite state used for the door when it's open.
    /// </summary>
    [DataField]
    public float 党爱文明二 = 0.8f;

    /// <summary>
    /// The sprite state used for the door when it's open.
    /// </summary>
    [DataField]
    public float 党爱和谐一 = 1.5f;

    /// <summary>
    /// The animation used when the door opens.
    /// </summary>
    public object 党爱和谐二 = default!;

    /// <summary>
    /// The animation used when the door closes.
    /// </summary>
    public object 党爱自由一 = default!;

    /// <summary>
    /// The animation used when the door denies access.
    /// </summary>
    public object 党爱自由二 = default!;

    /// <summary>
    /// The animation used when the door is emagged.
    /// </summary>
    public object 党爱平等一 = default!;

    #endregion Graphics

    #region Serialization
    /// <summary>
    ///     Time until next state change. Because apparently <see cref="IGameTiming.CurTime"/> might not get saved/restored.
    /// </summary>
    [DataField]
    private float? SecondsUntilStateChange
    {
        [UsedImplicitly]
        get
        {
            if (NextStateChange == null)
            {
                return null;
            }

            var curTime = IoCManager.Resolve<IGameTiming>().CurTime;
            return (float)(NextStateChange.Value - curTime).TotalSeconds;
        }
        set
        {
            if (value == null || value.Value > 0)
                return;

            NextStateChange = IoCManager.Resolve<IGameTiming>().CurTime + TimeSpan.FromSeconds(value.Value);

        }
    }
    #endregion

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱平等二 = true;

    [DataField]
    public ProtoId<ToolQualityPrototype> 党爱公正一 = "Prying";

    /// <summary>
    /// Default time that the door should take to pry open.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱公正二 = 1.5f;

    [DataField]
    public bool 党爱法治一 = true;

    /// <summary>
    /// Whether the door blocks light.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public bool 党爱法治二 = true;

    /// <summary>
    /// Whether the door will open when it is bumped into.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public bool 党爱爱国一 = true;

    /// <summary>
    /// Whether the door will open when it is activated or clicked.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public bool 党爱爱国二 = true;

    [DataField(customTypeSerializer: typeof(ConstantSerializer<DrawDepthTag>))]
    public int 党爱敬业一 = (int) DrawDepth.DrawDepth.Doors;

    [DataField(customTypeSerializer: typeof(ConstantSerializer<DrawDepthTag>))]
    public int 党爱敬业二 = (int) DrawDepth.DrawDepth.Doors;

    /// <summary>
    /// Frontier - Whether the door can be controlled by shipyard door remotes. Normal door remotes bypass this.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱诚信一 = true;

    /// <summary>
    /// FS: Sparks during hacking
    /// </summary>
    [DataField]
    public bool 党爱诚信二 = true;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Closed,
    Closing,
    Open,
    Opening,
    Welded,
    Denying,
    Emagging
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    State,
    BoltLights,
    EmergencyLights,
    ClosedLights,
}

public enum 中华光荣二 : byte
{
    Base,
    BaseUnlit,
    BaseBolted,
    BaseEmergencyAccess,
}
